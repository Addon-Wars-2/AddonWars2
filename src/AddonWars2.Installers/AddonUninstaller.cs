// ==================================================================================================
// <copyright file="AddonUninstaller.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AddonWars2.Core.Events;
    using AddonWars2.Installers.Enums;
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Installers.Models;
    using AddonWars2.Installers.Queue;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the <see cref="ProgressEventArgs"/> event handler.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void UninstallProgressChangedEventHandler(object? sender, ProgressEventArgs e);

    /// <summary>
    /// Represents a class responsible for uninstalling the reqested files.
    /// </summary>
    public class AddonUninstaller : IAddonUninstaller
    {
        #region Fields

        private static ILogger _logger;
        private readonly Dictionary<string, IProgress<double>> _progressCollection = new Dictionary<string, IProgress<double>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonUninstaller"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        public AddonUninstaller(ILogger<AddonUninstaller> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            UninstallProgressChanged += AddonUninstaller_UninstallProgressChanged;
            UninstallationStarted += AddonUninstaller_InstallationStarted;
            UninstallationCompleted += AddonUninstaller_InstallationCompleted;
        }

        #endregion Constructors

        #region Events

        /// <inheritdoc/>
        public event UninstallProgressChangedEventHandler? UninstallProgressChanged;

        /// <inheritdoc/>
        public event EventHandler? UninstallationStarted;

        /// <inheritdoc/>
        public event EventHandler? UninstallationCompleted;

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public virtual async Task<UninstallResult> UninstallAsync(UninstallRequest uninstallRequest)
        {
            OnUnnstallationStarted();

            var uninstalled = new List<UninstallResultFile>();
            var failed = new List<UninstallResultFile>();

            foreach (var file in uninstallRequest.FilesToUninstall)
            {
                if (File.Exists(file.FilePath))
                {
                    try
                    {
                        File.Delete(file.FilePath);
                        uninstalled.Add(new UninstallResultFile(file.FilePath));
                        Logger.LogInformation($"File uninstalled: {file.FilePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"Failed to uninstall the file due to an exception thrown: {file.FilePath}\n");
                        failed.Add(new UninstallResultFile(file.FilePath));
                    }
                }
                else
                {
                    Logger.LogError($"Failed to uninstall the file, because it doesn't exists: {file.FilePath}");
                    failed.Add(new UninstallResultFile(file.FilePath));
                }

                OnUninstallProgressChanged(uninstallRequest.FilesToUninstall.Count, uninstalled.Count + failed.Count);
            }

            OnUninstallationCompleted();

            var status = failed.Count == 0 ? UninstallResultStatus.Success : UninstallResultStatus.Failed;

            return new UninstallResult(uninstalled, failed, status);
        }

        /// <inheritdoc/>
        public void AttachProgressItem(string token, IProgress<double> progress)
        {
            ProgressCollection.Add(token, progress);
        }

        /// <summary>
        /// Raises <see cref="UninstallProgressChanged"/> event to inform subscribers the uninstallation progress value has changed.
        /// </summary>
        /// <param name="total">The total number of items.</param>
        /// <param name="processed">The number of items processed.</param>
        protected virtual void OnUninstallProgressChanged(int total, int processed)
        {
            var handler = UninstallProgressChanged;
            handler?.Invoke(this, new ProgressEventArgs(total, processed));
        }

        /// <summary>
        /// Raises <see cref="UninstallationStarted"/> event to inform subscribers the installation process has started.
        /// </summary>
        protected virtual void OnUnnstallationStarted()
        {
            var handler = UninstallationStarted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="UninstallationCompleted"/> event to inform subscribers the installation process has completed.
        /// </summary>
        protected virtual void OnUninstallationCompleted()
        {
            var handler = UninstallationCompleted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        // Updates all items in the progress collection.
        private void AddonUninstaller_UninstallProgressChanged(object? sender, ProgressEventArgs e)
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

        private void AddonUninstaller_InstallationStarted(object? sender, EventArgs e)
        {
            // Blank.
        }

        private void AddonUninstaller_InstallationCompleted(object? sender, EventArgs e)
        {
            UninstallProgressChanged -= AddonUninstaller_UninstallProgressChanged;
            UninstallationStarted -= AddonUninstaller_InstallationStarted;
            UninstallationCompleted -= AddonUninstaller_InstallationCompleted;
        }

        #endregion Methods
    }
}
