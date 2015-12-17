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

        private void OnGoToHomePageClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            // should use system.web http client
            var reg = (sender as Button).CommandParameter as LoginFormViewModel;
            if (reg == null)
            {
                this.NotificationTextBlock.Text = "Invalid form!";
            }
            this.NotificationTextBlock.Text = string.Empty;
            var url = "http://localhost:57647" + "/token";
            
            var content = new StringContent("grant_type=password&username=" + reg.UserName + "&password=" + reg.Password, Encoding.UTF8, "application/x-www-form-urlencoded");
            this.NotificationTextBlock.Text = "Loading...";
            var response = await this.httpClient.PostAsync(new Uri(url), content);           
            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TokenKeyValuePair>(resultContent);
            //Newtonsoft.Json.Converters.KeyValuePairConverter()
            //receive json object { "access_token":"TOKEN_STRING_HERE"}
            this.NotificationTextBlock.Text = result.AccessToken;
        }
    }

    [JsonObject]
    public class TokenKeyValuePair {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
