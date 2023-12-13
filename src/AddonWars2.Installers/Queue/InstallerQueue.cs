// ==================================================================================================
// <copyright file="InstallerQueue.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Queue
{
    using System.Collections.ObjectModel;
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Installers.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a queue of items to process for installing or uninstalling.
    /// </summary>
    public class InstallerQueue
    {
        #region Fields

        private static ILogger _logger;
        private readonly Queue<IInstallerAction> _queue = new Queue<IInstallerAction>();
        private readonly ObservableCollection<InstallResultFile> _results = new ObservableCollection<InstallResultFile>();
        private readonly CancellationToken _ct;
        private int _total = 0;
        private int _processed = 0;
        private bool _isQueueExecuted = false;
        private bool _isQueueProcessed = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerQueue"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        public InstallerQueue(ILogger<AddonInstallerBase> logger, CancellationToken cancellationToken)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ct = cancellationToken;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Is raised whenever an item in enqueued.
        /// </summary>
        public event EventHandler? Enqueued;

        /// <summary>
        /// Is raised whenever an item in dequeued.
        /// </summary>
        public event EventHandler? Dequeued;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets a collection of installed items.
        /// </summary>
        public ObservableCollection<InstallResultFile> Results => _results;

        /// <summary>
        /// Gets a total of items in the queue.
        /// </summary>
        /// <remarks>
        /// This value cannot be changed after calling <see cref="ExecuteAll"/>.
        /// </remarks>
        public int Total => _total;

        /// <summary>
        /// Gets a number of remaining items in the queue.
        /// </summary>
        public int Remained => _queue.Count;

        /// <summary>
        /// Gets a number of processed items.
        /// </summary>
        public int Processed
        {
            get => _processed;
            private set => _processed = value;
        }

        /// <summary>
        /// Gets a value indicating whether the queue was executed.
        /// </summary>
        public bool WasExecuted => _isQueueExecuted;

        /// <summary>
        /// Gets a value indicating whether the queue was processed.
        /// </summary>
        public bool WasProcessed => _isQueueProcessed;

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        /// <summary>
        /// Gets a queue.
        /// </summary>
        protected Queue<IInstallerAction> Queue => _queue;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the action to the end of <see cref="Queue"/>.
        /// </summary>
        /// <param name="action">An action to add.</param>
        public virtual void Enqueue(IInstallerAction action)
        {
            _queue.Enqueue(action);

            if (!_isQueueExecuted)
            {
                _total = _queue.Count;
            }

            OnEnqueued();
        }

        /// <summary>
        /// Removes and returns the action at the beginnging of the <see cref="Queue"/>.
        /// </summary>
        /// <returns>A dequeued <see cref="IInstallerAction"/>.</returns>
        public virtual IInstallerAction Dequeue()
        {
            var action = _queue.Dequeue();

            if (!_isQueueExecuted)
            {
                _total = _queue.Count;
            }

            OnDequeued();

            return action;
        }

        /// <summary>
        /// Executes all actions in the installer queue.
        /// </summary>
        public virtual void ExecuteAll()
        {
            _isQueueExecuted = true;
            while (Queue.Count > 0)
            {
                if (_ct.IsCancellationRequested)
                {
                    Logger.LogWarning("A task cancellation was requested.");
                    throw new TaskCanceledException();
                }

                var action = Queue.Dequeue();
                action?.Execute();

                Processed++;

                OnDequeued();
            }

            _isQueueProcessed = true;
        }

        /// <summary>
        /// Raises <see cref="Enqueued"/> event to inform subscribers that an action was enqueued.
        /// </summary>
        protected virtual void OnEnqueued()
        {
            var handler = Enqueued;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Dequeued"/> event to inform subscribers that an action was dequeued.
        /// </summary>
        protected virtual void OnDequeued()
        {
            var handler = Dequeued;
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion Methods
    }
}
