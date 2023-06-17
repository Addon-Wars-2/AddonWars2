// ==================================================================================================
// <copyright file="AnimatedItemsControlBehavior.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Extensions.Behaviors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using Microsoft.Xaml.Behaviors;

    // ***
    // This code is fragile as hell and ready to nuke itself any moment you change it. You've been WARNED!
    // ***

    // TODO: There is a bug when items loading overlap another one if you switch between tabs too fast.
    //       It's unclear yet, is a problem lies within the behavior class or there is a conflict with ScrollViewer.

    /// <summary>
    /// Defines a behavior that animates <see cref="ItemsControl"/> items.
    /// </summary>
    public class AnimatedItemsControlBehavior : Behavior<ItemsControl>
    {
        #region Fields

        private readonly List<object> _animatedItems = new List<object>();
        private readonly Queue<object> _animationQueue = new Queue<object>();
        private bool _isControlVisible = false;
        private bool _isControlLoaded = false;
        private bool _isAnimating = false;

        #endregion Fields

        #region Dependency Properties

        /// <summary>
        /// DependencyProperty for <see cref="Storyboard"/> property.
        /// </summary>
        public static readonly DependencyProperty AnimationsProperty =
            DependencyProperty.Register(
                nameof(Animations),
                typeof(Storyboard),
                typeof(AnimatedItemsControlBehavior),
                new FrameworkPropertyMetadata(default));

        /// <summary>
        /// Gets or sets a storyboard animation to play.
        /// </summary>
        public Storyboard Animations
        {
            get { return (Storyboard)GetValue(AnimationsProperty); }
            set { SetValue(AnimationsProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Delay"/> property.
        /// </summary>
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register(
                nameof(Delay),
                typeof(double),
                typeof(AnimatedItemsControlBehavior),
                new FrameworkPropertyMetadata(
                    0d,
                    FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnDelayChanged),
                    new CoerceValueCallback(CoerceDelay)));

        /// <summary>
        /// Gets or sets a delay value in seconds.
        /// </summary>
        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        // PropertyChangedCallback for Delay property.
        private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Blank.
        }

        // CoerceValueCallback for Delay property.
        private static object CoerceDelay(DependencyObject d, object baseValue)
        {
            var delay = (double)baseValue;
            if (delay < 0d)
            {
                return 0d;
            }

            return baseValue;
        }

        #endregion Dependency Properties

        #region Methods

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                base.OnAttached();

                AssociatedObject.Loaded += AssociatedObject_Loaded;
                AssociatedObject.Unloaded += AssociatedObject_Unloaded;
                AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                base.OnDetaching();

                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
                AssociatedObject.IsVisibleChanged -= AssociatedObject_IsVisibleChanged;
                UnsubscribeFromCollectionChanged(AssociatedObject.Items);
            }
        }

        // Performs internal clear before running a new animation sequence.
        private void ClearAnimatedItems()
        {
            _animatedItems.Clear();
            _animationQueue.Clear();
        }

        // Iterates thorugh item containers and makes them visible.
        private void ShowItemContainers()
        {
            foreach (var item in AssociatedObject.Items)
            {
                if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(item) is UIElement itemContainer)
                {
                    itemContainer.Visibility = Visibility.Visible;
                }
            }
        }

        // Iterates thorugh item containers and hides them.
        private void HideItemContainers()
        {
            foreach (var item in AssociatedObject.Items)
            {
                if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(item) is UIElement itemContainer)
                {
                    itemContainer.Visibility = Visibility.Collapsed;
                }
            }
        }

        // This is required to avoid "flickering" when items are rendered for the first time.
        private void ItemContainerStyleHideByDefault()
        {
            if (!AssociatedObject.ItemContainerStyle.IsSealed)
            {
                AssociatedObject.ItemContainerStyle.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            }
        }

        // Handles IsVisibleChanged event for the AssociatedObject.
        private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // This method covers the case, when items were loaded and animated,
            // then the ItemsControl became invisible (i.e. we switched to a different tab),
            // and then it is visible and loaded again. In this case a user will see
            // previously rendered items for a brief moment leading to a visual "flickering".

            // IsVisibleChanged fires before Loaded.
            // Determination of the IsVisible value takes all factors of layout into account.
            // This event is not raised if the element is not being rendered by the layout system.

            var isVisible = (bool)e.NewValue;

            // UI reports the control is visible.
            if (isVisible && !_isControlVisible)
            {
                // If not loaded - item will be animated and shown from Loaded handler.
                if (_isControlLoaded)
                {
                    ShowItemContainers();
                }

                _isControlVisible = true; // update the flag to match with isVisible
            }

            // UI reports the control is hidden.
            else if (!isVisible && _isControlVisible)
            {
                // The control is hidden now (i.e. moved to a different tab).
                HideItemContainers();

                _isControlVisible = false; // update the flag to match with isVisible
            }
        }

        // Handles Loaded event for the AssociatedObject.
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            _isControlLoaded = true;

            var itemsControl = (ItemsControl)sender;

            ClearAnimatedItems();
            ItemContainerStyleHideByDefault();
            UnsubscribeFromCollectionChanged(itemsControl.Items);
            SubscribeToCollectionChanged(itemsControl.Items);

            RunAnimationTask(itemsControl.ItemsSource);
        }

        // Handles Unloaded event for the AssociatedObject.
        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            _isControlLoaded = false;
            _isAnimating = false;

            ClearAnimatedItems();
        }

        // Handles CollectionChanged event for the ItemsControl items collection.
        private void ItemsSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems?.Count > 0)
                {
                    // Animate only newly added items.
                    var newItems = e.NewItems.OfType<object>().Except(_animatedItems);
                    RunAnimationTask(newItems);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Just clear everything.
                ClearAnimatedItems();
                UnsubscribeFromCollectionChanged(sender);
                SubscribeToCollectionChanged(AssociatedObject.Items);

                RunAnimationTask(AssociatedObject.ItemsSource);
            }
        }

        // Subscribes to CollectionChanged event.
        private void SubscribeToCollectionChanged(object? sender)
        {
            if (sender is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += ItemsSource_CollectionChanged;
            }
        }

        // Unsbscribes to CollectionChanged event.
        private void UnsubscribeFromCollectionChanged(object? sender)
        {
            if (sender is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged -= ItemsSource_CollectionChanged;
            }
        }

        // Runs a new animation task.
        private void RunAnimationTask(IEnumerable items)
        {
            // Ensures ItemContainerGenerator status is ContainersGenerated.
            AssociatedObject.UpdateLayout();

            Task.Run(() => AnimateItemsAsync(items));
        }

        // Sequentially animates items.
        private async Task AnimateItemsAsync(IEnumerable items)
        {
            foreach (var item in items)
            {
                // We remember which items has been animated already, to avoid
                // animating all items every time the collection changes.
                if (!_animatedItems.Contains(item))
                {
                    await Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        _animationQueue.Enqueue(item);

                        // Make sure the previous item's animation is finished before staring a new one.
                        if (!_isAnimating)
                        {
                            _isAnimating = true;
                            await ProcessAnimationQueueAsync(Delay, Animations);
                        }
                    });
                }
            }
        }

        // Processes the animation queue.
        private async Task ProcessAnimationQueueAsync(double delay, Storyboard storyboard)
        {
            while (_animationQueue.Count > 0)
            {
                var item = _animationQueue.Dequeue();
                if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(item) is UIElement itemContainer)
                {
                    _animatedItems.Add(item);

                    foreach (var animation in storyboard.Children)
                    {
                        Storyboard.SetTarget(animation, itemContainer);
                    }

                    storyboard.Begin();

                    await Task.Delay((int)(delay * 1000));  // sec to ms

                    // Since we hide items through ItemContainerStyle, we want to show them back.
                    if (itemContainer.Visibility == Visibility.Hidden || itemContainer.Visibility == Visibility.Collapsed)
                    {
                        itemContainer.Visibility = Visibility.Visible;
                    }
                }
            }

            _isAnimating = false;
        }

        #endregion Methods
    }
}
