using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Fridger.WindowsUniversalApp.ViewModels;
using Fridger.WindowsUniversalApp.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Fridger.WindowsUniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly HttpClient httpClient;

        public MainPage()
        {
            this.InitializeComponent();
            this.httpClient = new HttpClient();
            var contentViewModel = new RegisterFormContentViewModel();
            contentViewModel.RegisterForms = new List<RegisterFormViewModel>()
      {
        new RegisterFormViewModel("Batman", "https://littlemissobsessivesanatomy.files.wordpress.com/2012/07/happy-batman2.jpg", "Everything and cool suit", "dasda"),
        new RegisterFormViewModel("Ironman", "https://s-media-cache-ak0.pinimg.com/originals/c1/91/6c/c1916cd2bbbd45e67d043096d9c55fb1.jpg", "","Smart, Millionare, has a cool suit"),
      };

            this.DataContext = new MainPageViewModel(contentViewModel);
        }

        private async void RunButtonClick(object sender, RoutedEventArgs e)
        {
            this.ResultTextBlock.Text = string.Empty;
            var url = "http://localhost:57647/" + this.tbRoute.Text;
            //var url = "http://www.addic7ed.com/ajax_getEpisodes.php?showID=3005&season=4";        
            var response = await this.httpClient.GetAsync(new Uri(url));
            var result = await response.Content.ReadAsStringAsync();

            this.ResultTextBlock.Text = result;

        }

        private async void PostButtonClick(object sender, RoutedEventArgs e)
        {
            var reg = (sender as Button).CommandParameter;
            this.ResultTextBlock.Text = string.Empty;
            var url = "http://localhost:57647/" + this.tbRoute.Text;           
            var content = new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await this.httpClient.PostAsync(new Uri(url), content);
            var result = await response.Content.ReadAsStringAsync();

            this.ResultTextBlock.Text = result;

        }

        private void OnGoToRegisterPageClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }

        private void OnGoToLoginPageClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        //private async void RunButtonClick(object sender, RoutedEventArgs e)
        //{
        //    this.ResultTextBlock.Text = string.Empty;
        //    var url = "http://localhost:57647/" + this.tbRoute.Text;
        //    //var url = "http://www.addic7ed.com/ajax_getEpisodes.php?showID=3005&season=4";
        //    var request = WebRequest.CreateHttp(url);
        //    request.ContentType = "application/json";
        //    request.Method = "POST";


        //    var response = await request.GetResponseAsync();

        //    var reader = new StreamReader(response.GetResponseStream());
        //    var json = await reader.ReadToEndAsync();

        //    this.ResultTextBlock.Text = json;

        //}

        //private async Task<string> PostAsString(string url)
        //{
        //    var response = await this.httpClient.PostAsync(new Uri(url),);
        //    var result = await response.Content.ReadAsStringAsync();
        //    return result;
        //}

    }
    //public class RegisterForm
    //{
    //    public RegisterForm(string username, string email, string password, string confirmPassword)
    //    {
    //        UserName = username;
    //        Email = email;
    //        Password = password;
    //        ConfirmPassword = confirmPassword;
    //    }

    //    public string UserName { get; set; }

    //    public string Email { get; set; }

    //    public string Password { get; set; }

    //    public string ConfirmPassword { get; set; }
    //}
}
