// ==================================================================================================
// <copyright file="RenameFileInstallerAction.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Queue
{
    using System.Collections.ObjectModel;
    using AddonWars2.Core.DTO.Actions;
    using AddonWars2.Core.Extensions;
    using AddonWars2.Installers.Models;

    /// <summary>
    /// Represents an action that renames a file.
    /// </summary>
    public class RenameFileInstallerAction : InstallerCustomAction
    {
        #region Fields

        private readonly Action _action;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameFileInstallerAction"/> class.
        /// </summary>
        /// <param name="request">The installation request.</param>
        /// <param name="addonAction">An action to apply.</param>
        /// <param name="result">A collection of installed items.</param>
        public RenameFileInstallerAction(InstallRequest request, AddonActionBase addonAction, ObservableCollection<InstallResultFile> result)
            : base(request, addonAction, result)
        {
            _action = new Action(() =>
            {
                ActionApply(request, addonAction, result);
            });
        }

        #endregion Constructors

        #region Events

        #endregion Events

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override void Execute()
        {
            base.Execute();
        }

        /// <inheritdoc/>
        protected override void ExecuteCore()
        {
            _action?.Invoke();
        }

        // Applies the action.
        private void ActionApply(InstallRequest request, AddonActionBase addonAction, ObservableCollection<InstallResultFile> result)
        {
            if (addonAction is not RenameFileAddonAction act)
            {
                return;
            }

            var oldFilepath = string.Empty;
            var newFilepath = string.Empty;

            // Single-file addon.
            if (string.Equals(act.OldName, RenameFileAddonAction.Self, StringComparison.OrdinalIgnoreCase))
            {
                if (request.InstallItems.Count == 1 && request.InstallItems[0].Filename == request.InstallItems[0].RelativePath)
                {
                    oldFilepath = Path.Join(request.Entrypoint, request.InstallItems[0].RelativePath);
                    newFilepath = Path.Join(request.Entrypoint, act.NewName);

                    RenameFile(oldFilepath, newFilepath);

                    return;
                }

                throw new InvalidOperationException("\'Self\' file rename was requested, however more that one file was found in the request.");
            }
            else
            {
                // Else seek for the required file to rename.
                var i = request.InstallItems.FindIndex(x => x.RelativePath == act.OldName);
                if (i < 0)
                {
                    var message = $"Failed to find an item the action should be applied to.\nOld={act.OldName}\nNew={act.NewName}";
                    throw new InvalidOperationException(message);
                }

                oldFilepath = Path.Join(request.Entrypoint, request.InstallItems[i].RelativePath);
                newFilepath = Path.Join(request.Entrypoint, act.NewName);

                RenameFile(oldFilepath, newFilepath);
            }

            // Now check the installation result for an old item and replace it with a new one.
            var j = result.FindIndex(x => x.FilePath == oldFilepath);
            if (j < 0)
            {
                var message = $"Failed to find an item to modify.\nPath = {oldFilepath}";
                throw new InvalidOperationException(message);
            }

            result[j] = new InstallResultFile(newFilepath);
        }

        // Renames the file.
        private void RenameFile(string oldFilepath, string newFilepath)
        {
            if (File.Exists(newFilepath))
            {
                File.Move(oldFilepath, newFilepath, true);
            }
            else
            {
                throw new InvalidOperationException($"The file requested for rename wasn't found:\nOld={oldFilepath}\nNew={newFilepath}");
            }
        }

        #endregion Methods
    }
}
