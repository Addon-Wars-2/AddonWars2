// ==================================================================================================
// <copyright file="AddonActionApplierFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Factories
{
    using AddonWars2.Core.DTO.Actions;
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Installers.Models;
    using AddonWars2.Installers.Queue;
    using Microsoft.Extensions.Logging;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a factory for action appliers.
    /// </summary>
    public class InstallerCustomActionFactory : IInstallerCustomActionFactory
    {
        #region Fields

        private static ILogger<InstallerCustomActionFactory> _logger;
        private static Dictionary<Type, Type> _registeredTypes = new Dictionary<Type, Type>();

        #endregion Fields

        #region Constructors

        // Static ctor.
        static InstallerCustomActionFactory()
        {
            RegisterAction<RenameFileAddonAction, RenameFileInstallerAction>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerCustomActionFactory"/> class.
        /// </summary>
        /// <param name="logger">A reference to base <see cref="ILogger"/>.</param>
        public InstallerCustomActionFactory(
            ILogger<InstallerCustomActionFactory> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        /// <summary>
        /// Gets a collection of registered action types.
        /// </summary>
        protected static Dictionary<Type, Type> RegisteredTypes => _registeredTypes;

        #endregion Properties

        #region Methods

        /// <summary>
        ///  Registers a new action and its corresponding applier type.
        /// </summary>
        /// <typeparam name="T1">A action type.</typeparam>
        /// <typeparam name="T2">An applier type corresponsing the action.</typeparam>
        public static void RegisterAction<T1, T2>()
        {
            RegisteredTypes.Add(typeof(T1), typeof(T2));
        }

        /// <inheritdoc/>
        public InstallerCustomAction GetInstance<T>(T action, InstallRequest request, ObservableCollection<InstallResultFile> result)
            where T : AddonActionBase
        {
            var t = action.GetType();
            if (!RegisteredTypes.ContainsKey(t))
            {
                var err = $"The type \"{t.FullName}\" is not registered.";
                Logger.LogError(err);
                throw new NotSupportedException(err);
            }

            var typeToCreate = RegisteredTypes[t];
            var instance = Activator.CreateInstance(typeToCreate, request, action, result) as InstallerCustomAction;
            if (instance == null)
            {
                var err = $"Unable to create an action applier instance for the type \"{t.FullName}\".";
                Logger.LogError(err);
                throw new NullReferenceException(err);
            }

            return instance;
        }

        #endregion Methods
    }
}
