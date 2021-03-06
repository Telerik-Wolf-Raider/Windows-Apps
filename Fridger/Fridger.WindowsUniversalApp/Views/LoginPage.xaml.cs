﻿using Fridger.WindowsUniversalApp.Helpers;
using Fridger.WindowsUniversalApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Fridger.WindowsUniversalApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {              
        private readonly HttpClient httpClient;

        public LoginPage()
        {
            this.InitializeComponent();
            this.httpClient = new HttpClient();
            //var contentViewModel = new RegisterFormContentViewModel();
            //this.DataContext = new MainPageViewModel(contentViewModel);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            AnimatedTransition();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AnimatedTransition();
        }

        public void AnimatedTransition()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }

        private void OnGoToHomePageClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            var reg = (sender as Button).CommandParameter as LoginFormViewModel;
            if (reg == null || string.IsNullOrWhiteSpace(reg.Password) || string.IsNullOrWhiteSpace(reg.UserName) || reg.UserName.Length < 5 )
            {
                Notifier.Notify("Invalid form!");
                return; 
            }

            //this.NotificationTextBlock.Text = string.Empty;
            var url = "http://localhost:57647" + "/token";
            
            var content = new StringContent("grant_type=password&username=" + reg.UserName + "&password=" + reg.Password, Encoding.UTF8, "application/x-www-form-urlencoded");
            //this.NotificationTextBlock.Text = "Loading...";

            var response = await this.httpClient.PostAsync(new Uri(url), content);           
            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TokenKeyValuePair>(resultContent);
            //Newtonsoft.Json.Converters.KeyValuePairConverter()
            //receive json object { "access_token":"TOKEN_STRING_HERE"}
            Notifier.Notify("Logged in successfully!"); 
            Frame.Navigate(typeof(Pages.HomePage));
        }
    }

    [JsonObject]
    public class TokenKeyValuePair {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
