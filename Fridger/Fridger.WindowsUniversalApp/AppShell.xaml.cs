using Fridger.WindowsUniversalApp.Pages;
using Fridger.WindowsUniversalApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Fridger.WindowsUniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        public AppShell()
        {
            this.InitializeComponent();
        }

        public Frame AppFrame
        {
            get
            {
                return this.theFrame;
            }
        }

        private void OnHomeAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            this.AppFrame.Navigate(typeof(Pages.HomePage));
        }

        private async void OnAddAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //this.AppFrame.Navigate(typeof(AddSuperheroPage));
            var message = string.Format("You just clicked the button {0}", (sender as Button).Content);
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }
        private void OnAddProductAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            this.AppFrame.Navigate(typeof(AddProductsPage));
        }

        private void OnLoginAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            this.AppFrame.Navigate(typeof(LoginPage));
        }

        private void OnShoppingModeAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            this.AppFrame.Navigate(typeof(ShoppingModePage));
        }

        private void OnSettingsAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            this.AppFrame.Navigate(typeof(SettingsPage));
        }

        private void OnDatabaseAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            this.AppFrame.Navigate(typeof(GetProductsFromDatabase));
        }
    }
}
