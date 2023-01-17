// ==================================================================================================
// <copyright file="NewsPage.xaml.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Views
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Utils.Helpers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Web.WebView2.Core;
    using Microsoft.Web.WebView2.Wpf;

    /// <summary>
    /// Interaction logic for NewsPage.xaml.
    /// </summary>
    public partial class NewsPage : Page
    {
        #region Constructs

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsPage"/> class.
        /// </summary>
        public NewsPage()
        {
            InitializeComponent();
            InitializeWebView2Async();

            AW2Application.Current.MainWindowInstance.Closed += MainWindowInstance_Closed;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initializes <see cref="WebView2"/> control.
        /// </summary>
        public async void InitializeWebView2Async()
        {
            // TODO: Is there a better way to inject this?
            var appConfig = AW2Application.Current.Services.GetRequiredService<ApplicationConfig>();
            var appDataDir = appConfig.AppDataDir;
            ////var cssFilePath = Path.Combine(appDataDir, appConfig.RssFeedDirName, "rssfeed.css");
            ////var css = Assembly.GetExecutingAssembly().GetManifestResourceStream("AddonWars2.App.Resources.rssfeed.css");

            ////using (var stream = File.OpenWrite(cssFilePath))
            ////{
            ////    await css.CopyToAsync(stream);
            ////}

            // HACK: https://github.com/MicrosoftEdge/WebView2Feedback/issues/299#issuecomment-648812482
            var options = new CoreWebView2EnvironmentOptions("--disk-cache-size=1");

            var task = await CoreWebView2Environment.CreateAsync(null, appDataDir, options);

            var webView2 = (WebView2)FindName("WebView2Control");
            if (webView2 != null)
            {
                await webView2.EnsureCoreWebView2Async(task);
                webView2.CoreWebView2.Settings.IsStatusBarEnabled = true;
                webView2.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                webView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                webView2.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
                webView2.CoreWebView2.Settings.IsScriptEnabled = true;
                webView2.AllowExternalDrop = false;
            }
        }

        // Cleanup WebView data on main window close event.
        private void MainWindowInstance_Closed(object sender, EventArgs e)
        {
            var webView2 = (WebView2)FindName("WebView2Control");
            if (webView2 != null)
            {
                var profile = webView2.CoreWebView2?.Profile;
                profile?.ClearBrowsingDataAsync();
            }
        }

        #endregion Methods
    }
}
