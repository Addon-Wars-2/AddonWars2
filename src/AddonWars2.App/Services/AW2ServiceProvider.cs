// ==================================================================================================
// <copyright file="AW2ServiceProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services.Interfaces;
    using AddonWars2.App.ViewModels;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.Services.HttpClientWrapper;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using AddonWars2.Services.RssFeedService;
    using AddonWars2.Services.RssFeedService.Interfaces;
    using AddonWars2.Services.RssFeedService.Models;
    using AddonWars2.Services.XmlReadWriteService;
    using AddonWars2.Services.XmlReadWriteService.Interfaces;
    using AddonWars2.Services.XmlSerializerService;
    using AddonWars2.Services.XmlSerializerService.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;

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
            services.AddSingleton<ApplicationConfig>();

            // View models.
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<NewsPageViewModel>();
            services.AddSingleton<LoggingViewModel>();
            services.AddSingleton<InstallAddonsPageViewModel>();

            // Commands.
            services.AddSingleton<CommonCommands>();

            // Services.
            services.AddSingleton<ILogsAggregator, LogsAggregator>();
            services.AddSingleton<AddonsService>();
            services.AddSingleton<DialogService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<IRssFeedService<Gw2RssFeedItem>, Gw2RssFeedService>();
            services.AddSingleton<IXmlReaderService, XmlReaderService>();
            services.AddSingleton<IXmlWriterService, XmlWriterService>();
            services.AddSingleton<IXmlSerializationService, XmlSerializationService>();

            // Http client services.
            services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();

            // Logging.
            services.AddSingleton<SerilogLogsAggregatorSink>();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddSerilog(dispose: true);
            });

            return services.BuildServiceProvider();
        }

        #endregion Methods
    }
}
