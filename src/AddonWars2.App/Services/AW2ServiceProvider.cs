// ==================================================================================================
// <copyright file="AW2ServiceProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using AddonWars2.Addons.Downloaders;
    using AddonWars2.Addons.Downloaders.Interfaces;
    using AddonWars2.Addons.RegistryProvider;
    using AddonWars2.Addons.RegistryProvider.Interfaces;
    using AddonWars2.App.Models.Configuration;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services.Interfaces;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.Services.GitHubClientWrapper;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using AddonWars2.Services.HttpClientWrapper;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using AddonWars2.Services.RssFeedService;
    using AddonWars2.Services.RssFeedService.Interfaces;
    using AddonWars2.Services.RssFeedService.Models;
    using AddonWars2.Services.XmlReadWriteService;
    using AddonWars2.Services.XmlReadWriteService.Interfaces;
    using AddonWars2.Services.XmlSerializerService;
    using AddonWars2.Services.XmlSerializerService.Interfaces;
    using AddonWars2.SharedData;
    using Config.Net;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Octokit;
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

            var defaultProductName = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title.Replace(" ", "-") ?? string.Empty;
            var defaultProductVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? string.Empty;
            var defaultProductComment = $"https://github.com/Addon-Wars-2/AddonWars2";  // TODO: retrieve from static data

            // Config.
            services.AddSingleton<IApplicationConfig>(
                builder =>
                {
                    var settings = new ConfigurationBuilder<IApplicationConfig>()
                        .UseJsonFile(Path.Join(IOHelper.GenerateApplicationDataDirectory(), "config.json"))
                        .Build();
                    return settings;
                });

            // Static data.
            services.AddSingleton<IAppSharedData, AppSharedData>();
            services.AddSingleton<IWebSharedData, WebSharedData>();
            services.AddSingleton<IGameSharedData, GameSharedData>();

            // View models.
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<NewsPageViewModel>();
            services.AddSingleton<LoggingViewModel>();
            services.AddSingleton<ManageAddonsPageViewModel>();
            services.AddSingleton<SettingsPageViewModel>();
            services.AddSingleton<SettingsGeneralPageViewModel>();
            services.AddSingleton<SettingsApiPageViewModel>();


            // Commands.
            services.AddSingleton<CommonCommands>();

            // Services.
            services.AddSingleton<DialogService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<IRssFeedService<Gw2RssFeedItem>, Gw2RssFeedService>();
            services.AddSingleton<IXmlReaderService, XmlReaderService>();
            services.AddSingleton<IXmlWriterService, XmlWriterService>();
            services.AddSingleton<IXmlSerializationService, XmlSerializationService>();
            services.AddSingleton<IRegistryProviderFactory, RegistryProviderFactory>();
            services.AddSingleton<IAddonDownloaderFactory, AddonDownloaderFactory>();
            services.AddSingleton<IGitHubClientWrapper, GitHubClientWrapper>();

            services.AddSingleton<GitHubClient>(
                new GitHubClient(new ProductHeaderValue(defaultProductName, defaultProductVersion)));

            services.AddSingleton<HttpClient>(
                builder =>
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("User-Agent", $"{defaultProductName}/{defaultProductVersion} (+{defaultProductComment})");
                    return client;
                });
            services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();

            // Logging.
            services.AddSingleton<ILogsAggregator, LogsAggregator>();
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
