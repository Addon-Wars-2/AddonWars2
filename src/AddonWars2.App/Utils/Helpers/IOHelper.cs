// ==================================================================================================
// <copyright file="IOHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Win32;

    /// <summary>
    /// Provides various methods to manage IO flow.
    /// </summary>
    public static class IOHelper
    {
        #region Methods

        /// <summary>
        /// Determines if a user has write access to a given directory.
        /// </summary>
        /// <param name="directory">Directory path.</param>
        /// <returns><see langword="true"/> if has access, otherwise - <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="directory"/> is <see langword="null"/> or empty.</exception>
        public static bool HasWriteAccessToDirectory(string directory)
        {
            ArgumentException.ThrowIfNullOrEmpty(directory, nameof(directory));

            try
            {
                var stream = File.Create(Path.Combine(directory, Path.GetRandomFileName()), bufferSize: 1, FileOptions.DeleteOnClose);
                using (stream)
                {
                    // Do nothing.
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Builds and returns the application data directory for this application.
        /// The directory will be placed into OS appdata directory.
        /// </summary>
        /// <returns>Application data directory path.</returns>
        public static string BuildApplicationDataDirectory()
        {
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = Path.GetFileNameWithoutExtension(Environment.ProcessPath);
            var appDir = Path.Join(appDataDir, appName);

            return appDir;
        }

        /// <summary>
        /// Performs a registry search for a given <see cref="name"/> within the specified
        /// <paramref name="keyname"/> using the local machine data.
        /// </summary>
        /// <remarks>
        /// A returned value must be checked for <see langword="null"/>.
        /// </remarks>
        /// <param name="keyname">Key (directory) to search in.</param>
        /// <param name="name">Value name to search for.</param>
        /// <returns>A found value boxed inside an <see cref="object"/> if found. Otherwise - <see langword="null"/>.</returns>
        public static object? SearchRegistryKey(string keyname, string name)
        {
            if (keyname == null || name == null)
            {
                return null;
            }

            RegistryKey? parent = Registry.LocalMachine.OpenSubKey(keyname);

            try
            {
                var foundValue = parent?.GetValue(name);
                return foundValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Writes a given resource locally.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <param name="path">File path used to write a resource.</param>
        /// <param name="fileMode"><see cref="FileMode"/> used for the writing operation.</param>
        /// <returns><see cref="Task"/> object.</returns>
        /// <exception cref="NullReferenceException">Is thrown if a given resource is not found.</exception>
        /// <exception cref="NullReferenceException">Is thrown if <paramref name="path"/> directory is a root directory or <paramref name="path"/> is <see langword="null"/>.</exception>
        public static async Task ResourceCopyToAsync(string resourceName, string path, FileMode fileMode = FileMode.Create)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var resource = assembly.GetManifestResourceStream(resourceName))
            {
                if (resource == null)
                {
                    throw new NullReferenceException($"Resource not found: {resourceName}");
                }

                var dirName = Path.GetDirectoryName(path);
                if (dirName == null)
                {
                    throw new NullReferenceException(nameof(dirName));
                }

                Directory.CreateDirectory(dirName);

                using (FileStream file = new FileStream(path, fileMode, FileAccess.Write))
                {
                    await resource.CopyToAsync(file);
                }
            }
        }

        #endregion Methods
    }
}
