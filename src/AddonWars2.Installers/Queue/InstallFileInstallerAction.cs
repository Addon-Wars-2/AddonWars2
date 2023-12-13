// ==================================================================================================
// <copyright file="InstallFileInstallerAction.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Queue
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using AddonWars2.Installers.Models;

    /// <summary>
    /// Represents an action that installs a single file by writing its data to a file.
    /// </summary>
    public class InstallFileInstallerAction : InstallerActionBase
    {
        #region Fields

        private readonly Action _action;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallFileInstallerAction"/> class.
        /// </summary>
        /// <param name="request">The installation request.</param>
        /// <param name="file">A file to install.</param>
        /// <param name="result">A collection of installed items.</param>
        public InstallFileInstallerAction(InstallRequest request, InstallRequestFile file, ObservableCollection<InstallResultFile> result)
        {
            var path = Path.Join(request.Entrypoint, file.RelativePath);

            //_action = new Action(() =>
            //{
            //    Directory.CreateDirectory(Path.GetDirectoryName(path) !);  // okay to throw an exception if path = null or empty
            //    File.WriteAllBytes(path, file.Content);
            //    result.InstalledFiles.Add(new InstallResultFile(path));
            //});

            _action = new Action(() =>
            {
                Debug.WriteLine($"InstallFileAction | file=\'{file.Filename}\', path=\'{path}\'");
                result.Add(new InstallResultFile(path));
            });
        }

        #endregion Constructors

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

        #endregion Methods
    }
}
