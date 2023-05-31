// ==================================================================================================
// <copyright file="WindowLocator.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using AddonWars2.App.UIServices.Interfaces;

    /// <summary>
    /// Represents a class which can be used to locate and get <see cref="Window"/> instances
    /// based on a provided view model name.
    /// </summary>
    public class WindowLocator : IWindowLocator
    {
        #region Methods

        /// <inheritdoc/>
        public Window FindWindow<T>()
            where T : Window
        {
            // Search through all types defined in this assembly to find matching view (window) type, which:
            // a) Has the same name as provided in parameter or defined by naming convention.
            // b) Can be assigned to a type this methods return (some base Window class).

            var windowTypeName = typeof(T).Name;
            var windowType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                t => t.Name == windowTypeName && typeof(Window).IsAssignableFrom(t))
                ?? throw new ArgumentOutOfRangeException($"Unable to locate window type for the view model: {typeof(T)}");

            var windowTypeFullName = windowType.FullName ?? string.Empty;
            var windowInstance = Assembly.GetExecutingAssembly()?.CreateInstance(windowTypeFullName) as Window;

            return windowInstance ?? throw new NullReferenceException($"Unable to create window instance for typeName={windowTypeFullName}.");
        }

        #endregion Methods
    }
}
