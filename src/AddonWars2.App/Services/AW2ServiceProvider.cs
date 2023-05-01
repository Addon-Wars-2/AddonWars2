﻿// ==================================================================================================
// <copyright file="AW2ServiceProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System;
    using AddonWars2.App.Commands;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services.Interfaces;
    using AddonWars2.App.ViewModels;
    using AddonWars2.Services.RssFeedService;
    using AddonWars2.Services.RssFeedService.Interfaces;
    using AddonWars2.Services.RssFeedService.Models;
    using AddonWars2.Services.WebClientService;
    using AddonWars2.Services.WebClientService.Interfaces;
    using AddonWars2.Services.XmlReadWriteService;
    using AddonWars2.Services.XmlReadWriteService.Interfaces;
    using AddonWars2.Services.XmlSerializerService;
    using AddonWars2.Services.XmlSerializerService.Interfaces;
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
            services.AddSingleton<NLogLogsAggregatorTarget>();
            services.AddSingleton<ApplicationConfig>();

            // View models.
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<NewsPageViewModel>();
            services.AddSingleton<LoggingViewModel>();

            // Commands.
            services.AddSingleton<CommonCommands>();

            // Services.
            services.AddSingleton<ILogsAggregator, LogsAggregator>();
            services.AddSingleton<AddonsService>();
            services.AddSingleton<DialogService>();
            services.AddSingleton<MessageBoxService>();
            services.AddSingleton<Gw2RssFeedService>();
            services.AddSingleton<IWebClientService, GenericWebClientService>();
            services.AddSingleton<IXmlReaderService, XmlReaderService>();
            services.AddSingleton<IXmlWriterService, XmlWriterService>();
            services.AddSingleton<IXmlSerializationService, XmlSerializationService>();

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
