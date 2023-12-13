// ==================================================================================================
// <copyright file="AddonInstallerBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading;
    using System.Threading.Tasks;
    using AddonWars2.Core.Enums;
    using AddonWars2.Core.Events;
    using AddonWars2.Installers.Enums;
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Installers.Models;
    using AddonWars2.Installers.Queue;
    using AddonWars2.SharedData.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the <see cref="ProgressEventArgs"/> event handler.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void InstallProgressChangedEventHandler(object? sender, ProgressEventArgs e);

    /// <summary>
    /// Represents a base class for addon installers.
    /// </summary>
    public abstract class AddonInstallerBase : IAddonInstaller
    {
        #region Fields

        private static ILogger _logger;
        private readonly IGameSharedData _gameSharedData;
        private readonly IInstallerCustomActionFactory _installerCustomActionFactory;
        private readonly Dictionary<string, IProgress<double>> _progressCollection = new Dictionary<string, IProgress<double>>();
        private InstallerQueue _installerQueue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonInstallerBase"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="gameSharedData">A reference to <see cref="IGameSharedData"/>.</param>
        /// <param name="installerCustomActionFactory">A reference to <see cref="IInstallerCustomActionFactory"/>.</param>
        public AddonInstallerBase(
            ILogger<AddonInstallerBase> logger,
            IGameSharedData gameSharedData,
            IInstallerCustomActionFactory installerCustomActionFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameSharedData = gameSharedData ?? throw new ArgumentNullException(nameof(gameSharedData));
            _installerCustomActionFactory = installerCustomActionFactory ?? throw new ArgumentNullException(nameof(installerCustomActionFactory));

            InstallProgressChanged += AddonInstallerBase_InstallProgressChanged;
            InstallationStarted += AddonInstallerBase_InstallationStarted;
            InstallationCompleted += AddonInstallerBase_InstallationCompleted;
        }

        #endregion Constructors

        #region Events

        /// <inheritdoc/>
        public event InstallProgressChangedEventHandler? InstallProgressChanged;

        /// <inheritdoc/>
        public event EventHandler? InstallationStarted;

        /// <inheritdoc/>
        public event EventHandler? InstallationCompleted;

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

        /// <inheritdoc/>
        public abstract string Entrypoint { get; }

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        /// <summary>
        /// Gets a reference to <see cref="IGameSharedData"/>.
        /// </summary>
        protected IGameSharedData GameSharedData => _gameSharedData;

        /// <summary>
        /// Gets the rule applier factory.
        /// </summary>
        protected IInstallerCustomActionFactory InstallerCustomActionFactory => _installerCustomActionFactory;

        /// <summary>
        /// Gets the installer queue.
        /// </summary>
        protected InstallerQueue InstallerQueue
        {
            get => _installerQueue;
            private set => _installerQueue = value;
        }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public virtual async Task<InstallResult> InstallAsync(InstallRequest request, CancellationToken cancellationToken)
        {
            InstallerQueue = new InstallerQueue((ILogger<AddonInstallerBase>)Logger, cancellationToken);

            InstallerQueue.Enqueued += InstallerQueue_Enqueued;
            InstallerQueue.Dequeued += InstallerQueue_Dequeued;
            InstallerQueue.Results.CollectionChanged += Results_CollectionChanged;

            // Check if we have enough disk space.
            var hasEnoughSpace = HasEnoughDriveSpace(request);
            if (!hasEnoughSpace)
            {
                return new InstallResult(InstallResultStatus.Failed);
            }

            try
            {
                OnInstallationStarted();

                EnqueuePriorInstallationActions(request);
                EnqueueInstallActions(request);
                EnqueuePostInstallationActions(request);

                InstallerQueue.ExecuteAll();

                OnInstallationCompleted();

                return new InstallResult(InstallerQueue.Results.ToList(), InstallResultStatus.Success);
            }
            catch (TaskCanceledException)
            {
                Logger.LogWarning("A task cancellation was requested.");
                return new InstallResult(InstallerQueue.Results.ToList(), InstallResultStatus.Aborted);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception has occured during the installation process.\n");
                return new InstallResult(InstallerQueue.Results.ToList(), InstallResultStatus.Failed);
            }
            finally
            {
                ProgressCollection.Clear();

                InstallerQueue.Enqueued += InstallerQueue_Enqueued;
                InstallerQueue.Dequeued += InstallerQueue_Dequeued;
                InstallerQueue.Results.CollectionChanged += Results_CollectionChanged;
            }
        }

        /// <inheritdoc/>
        public void AttachProgressItem(string token, IProgress<double> progress)
        {
            ProgressCollection.Add(token, progress);
        }

        /// <summary>
        /// Raises <see cref="InstallProgressChanged"/> event to inform subscribers the installation progress value has changed.
        /// </summary>
        /// <param name="total">The total number of items.</param>
        /// <param name="processed">The number of items processed.</param>
        protected virtual void OnInstallProgressChanged(int total, int processed)
        {
            var handler = InstallProgressChanged;
            handler?.Invoke(this, new ProgressEventArgs(total, processed));
        }

        /// <summary>
        /// Checks if a provided drive has sufficient space for the requested addon.
        /// </summary>
        /// <param name="request">The install request.</param>
        /// <returns><see langword="true"/> if there is enough space, otherwise <see langword="false"/>.</returns>
        protected bool HasEnoughDriveSpace(InstallRequest request)
        {
            var fileInfo = new FileInfo(request.Entrypoint);
            var driveLetter = Path.GetPathRoot(fileInfo.FullName);
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                if (drive.Name == driveLetter)
                {
                    if (drive.AvailableFreeSpace >= request.RequiredDiskSpace)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Enqueues install actions.
        /// </summary>
        /// <param name="request">An installation request to process.</param>
        protected void EnqueueInstallActions(InstallRequest request)
        {
            foreach (var file in request.InstallItems)
            {
                var action = new InstallFileInstallerAction(request, file, InstallerQueue.Results);
                InstallerQueue.Enqueue(action);
            }
        }

        /// <summary>
        /// Enqueues optional action to be applied prior installing the files.
        /// </summary>
        /// <param name="request">An installation request to process.</param>
        protected void EnqueuePriorInstallationActions(InstallRequest request)
        {
            foreach (var action in request.Instructions.InstallActions)
            {
                if (action.WhenApplyAddonAction == WhenApplyAddonAction.PreInstall)
                {
                    var iAction = InstallerCustomActionFactory.GetInstance(action, request, InstallerQueue.Results);
                    InstallerQueue.Enqueue(iAction);
                }
            }
        }

        /// <summary>
        /// Enqueues optional action to be applied after installing the files.
        /// </summary>
        /// <param name="request">An installation request to process.</param>
        protected void EnqueuePostInstallationActions(InstallRequest request)
        {
            foreach (var action in request.Instructions.InstallActions)
            {
                if (action.WhenApplyAddonAction == WhenApplyAddonAction.PostInstall)
                {
                    var iAction = InstallerCustomActionFactory.GetInstance(action, request, InstallerQueue.Results);
                    InstallerQueue.Enqueue(iAction);
                }
            }
        }

        /// <summary>
        /// Raises <see cref="InstallationStarted"/> event to inform subscribers the installation process has started.
        /// </summary>
        protected virtual void OnInstallationStarted()
        {
            var handler = InstallationStarted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="InstallationCompleted"/> event to inform subscribers the installation process has completed.
        /// </summary>
        protected virtual void OnInstallationCompleted()
        {
            var handler = InstallationCompleted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        // Updates all items in the progress collection.
        private void AddonInstallerBase_InstallProgressChanged(object? sender, ProgressEventArgs e)
        {
            if (ProgressCollection.Count > 0)
            {
                foreach (var key in ProgressCollection.Keys)
                {
                    ProgressCollection.TryGetValue(key, out var progress);
                    progress?.Report(e.Progress);
                }
            }
        }

        private void AddonInstallerBase_InstallationStarted(object? sender, EventArgs e)
        {
            // Blank.
        }

        private void AddonInstallerBase_InstallationCompleted(object? sender, EventArgs e)
        {
            InstallProgressChanged -= AddonInstallerBase_InstallProgressChanged;
            InstallationStarted -= AddonInstallerBase_InstallationStarted;
            InstallationCompleted -= AddonInstallerBase_InstallationCompleted;
        }

        // Indicates an item has been enqueued for the installation.
        private void InstallerQueue_Enqueued(object? sender, EventArgs e)
        {
            // Blank.
        }

        // Indicates an item has been processed.
        private void InstallerQueue_Dequeued(object? sender, EventArgs e)
        {
            OnInstallProgressChanged(InstallerQueue.Total, InstallerQueue.Processed);
        }

        private void Results_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (var item in e.NewItems)
                        {
                            var irf = (InstallResultFile)item;
                            Logger.LogInformation($"File installed: {irf.FilePath}");
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            var irf = (InstallResultFile)item;
                            Logger.LogInformation($"Removed from the installed: {irf.FilePath}");
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null && e.NewItems != null)
                    {
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            var oldIrf = (InstallResultFile)e.OldItems[i] !;  // InstallResultFile is a struct
                            var newIrf = (InstallResultFile)e.NewItems[i] !;  // InstallResultFile is a struct
                            var oldIndex = e.OldStartingIndex + i;
                            var newIndex = e.NewStartingIndex + i;
                            Logger.LogInformation($"Replaced in the installed: {oldIrf.FilePath} @ oldIndex={oldIndex} -> {newIrf.FilePath} @ newIndex={newIndex}");
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems != null && e.NewItems != null)
                    {
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            var oldIrf = (InstallResultFile)e.OldItems[i] !;  // InstallResultFile is a struct
                            var newIrf = (InstallResultFile)e.NewItems[i] !;  // InstallResultFile is a struct
                            var oldIndex = e.OldStartingIndex + i;
                            var newIndex = e.NewStartingIndex + i;
                            Logger.LogInformation($"Moved in the installed: {oldIrf.FilePath} @ oldIndex={oldIndex} -> {newIrf.FilePath} @ newIndex={newIndex}");
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    Logger.LogWarning($"Installed collection was reset!");
                    break;
                default:
                    break;
            }
        }

        #endregion Methods
    }
}
