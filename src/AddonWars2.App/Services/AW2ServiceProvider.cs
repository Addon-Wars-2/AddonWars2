// ==================================================================================================
// <copyright file="AW2ServiceProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System;
    using AddonWars2.App.Commands;
    using AddonWars2.App.Controllers;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;

    /// <summary>
    /// Application service collection for Dependency Injection purposes.
    /// </summary>
    public static class AW2ServiceProvider
    {
        #region Methods

        /// <summary>
        /// Returns the application service collection.
        /// </summary>
        /// <returns><see cref="IServiceCollection"/> instance.</returns>
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Models.
            // TODO: factories?
            services.AddSingleton<NLogLoggingManagerTarget>();
            services.AddSingleton<ApplicationConfig>();

            // View models.
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<NewsViewModel>();
            services.AddSingleton<LoggingViewModel>();

            // Commands.
            services.AddSingleton<CommonCommands>();

            // Services.
            services.AddSingleton<LoggingManager>();
            services.AddSingleton<DialogService>();
            services.AddSingleton<MessageBoxService>();

            // Configure logger here as per NLog GitHub guide.
            var cfg = IOHelper.GetLoggerConfigurationNLog();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddNLog(cfg);
            });

            return services.BuildServiceProvider();
        }

        #endregion Methods
    }
}
