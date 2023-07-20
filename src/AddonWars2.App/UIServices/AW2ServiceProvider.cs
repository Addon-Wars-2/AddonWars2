// ==================================================================================================
// <copyright file="AW2ServiceProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.Logging;
    using AddonWars2.App.UIServices.Interfaces;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.App.ViewModels.Factories;
    using AddonWars2.Core;
    using AddonWars2.Core.Interfaces;
    using AddonWars2.DependencyResolvers.Factories;
    using AddonWars2.DependencyResolvers.Interfaces;
    using AddonWars2.Downloaders.Factories;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Extractors.Factories;
    using AddonWars2.Extractors.Interfaces;
    using AddonWars2.Providers.Factories;
    using AddonWars2.Providers.Interfaces;
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
    using AddonWars2.SharedData.Interfaces;
    using CommunityToolkit.Mvvm.Messaging;
    using Config.Net;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;
    using MvvmDialogs.DialogTypeLocators;
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
            services.AddSingleton(
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

            // UI Services.
            services.AddSingleton<IDialogTypeLocator, ErrorDialogTypeLocator>();
            services.AddSingleton<IDialogService, DialogService>(x => new DialogService(dialogTypeLocator: x.GetRequiredService<IDialogTypeLocator>()));

            // Other services.
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
            services.AddSingleton<IRssFeedService<Gw2RssFeedItem>, Gw2RssFeedService>();
            services.AddSingleton<IXmlReaderService, XmlReaderService>();
            services.AddSingleton<IXmlWriterService, XmlWriterService>();
            services.AddSingleton<IXmlSerializationService, XmlSerializationService>();
            services.AddSingleton(new GitHubClient(new ProductHeaderValue(defaultProductName, defaultProductVersion)));
            services.AddSingleton(
                builder =>
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("User-Agent", $"{defaultProductName}/{defaultProductVersion} (+{defaultProductComment})");
                    client.Timeout = TimeSpan.FromSeconds(30);
                    return client;
                });
            services.AddSingleton<IGitHubClientWrapper, GitHubClientWrapper>();
            services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
            services.AddSingleton<ILibraryManager, LibraryManager>();

            // Factories.
            services.AddSingleton<IRegistryProviderFactory, RegistryProviderFactory>();
            services.AddSingleton<IAddonDownloaderFactory, AddonDownloaderFactory>();
            services.AddSingleton<IAddonExtractorFactory, AddonExtractorFactory>();
            services.AddSingleton<IDependencyResolverFactory, DependencyResolverFactory>();
            services.AddSingleton<IErrorDialogViewModelFactory, ErrorDialogViewModelFactory>();
            services.AddSingleton<IInstallAddonsDialogFactory, InstallAddonsDialogFactory>();
            services.AddSingleton<IInstallProgressDialogFactory, InstallProgressDialogFactory>();

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
