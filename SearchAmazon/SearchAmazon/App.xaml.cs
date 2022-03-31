﻿// ······································································//
// <copyright file="App.xaml.cs" company="Jay Bautista Mendoza">         //
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.          //
// </copyright>                                                          //
// ······································································//

namespace SearchAmazon
{
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows;
    using SearchAmazon.Views;
    using JayWpf.Windows;

    /// <summary>Interaction logic for App XAML.</summary>
    public partial class App : Application
    {
        /// <summary>Instance of WPF Window to create.</summary>
        private WpfWindow mainWindow;

        /// <summary>Startup event of the main App.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">StartupEventArgs 'e'.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            this.mainWindow = new WpfWindow("Views/MainPage.xaml");
            this.mainWindow.Title = "Search Amazon";
            this.mainWindow.IconText = "🔍ᵃ";
            this.mainWindow.IconTextSize = 18;
            this.mainWindow.Show();
        }
    }
}
