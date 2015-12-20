using Fridger.WindowsUniversalApp.Controls;
using Fridger.WindowsUniversalApp.Models;
using Fridger.WindowsUniversalApp.ViewModels;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Fridger.WindowsUniversalApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingModePage
    {
        private string pageTitle;
        private Geolocator geolocator;
        public ShoppingModePage()
        {
            this.InitializeComponent();
            this.geolocator = new Geolocator();
            this.InitAsync();

            var contentViewModel = new ProductsContentViewModel();

            this.ContentViewModel = contentViewModel;
            contentViewModel.Products = GetProductsList();
            this.ViewModel = new ShoppingPageViewModel(contentViewModel);
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

        private async void InitAsync()
        {
            var connection = this.GetDbConnectionAsync();
            await connection.CreateTableAsync<GeoPositionPoint>();
        }
        private SQLiteAsyncConnection GetDbConnectionAsync()
        {
            var dbFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");

            var connectionFactory =
                new Func<SQLiteConnectionWithLock>(
                    () =>
                    new SQLiteConnectionWithLock(
                        new SQLitePlatformWinRT(),
                        new SQLiteConnectionString(dbFilePath, storeDateTimeAsTicks: false)));

            var asyncConnection = new SQLiteAsyncConnection(connectionFactory);

            return asyncConnection;
        }

        private IEnumerable<ProductViewModel> GetProductsList()
        {
            // here you can get the products from the server 

            return new List<ProductViewModel>()
                      {
                            new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 1" },
                            new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 2" },
                            new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 3" },
                            new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 4" },
                      };
        }

        public ShoppingPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as ShoppingPageViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public IContentViewModel ContentViewModel { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

        public ProductViewModel ProductTest { get; set; }

        public string Title
        {
            get
            {
                return this.pageTitle;
            }
        }

        private async void ProductDetails_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            //this.AppFrame.Navigate(typeof(AddSuperheroPage));
            var product = sender as ProductDetails;
            var message = string.Format("You just double tapped the product {0}", product.ProductName);
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
            if (product.Opacity < 1)
            {
                product.Opacity = 1;
            }
            else
            {
                product.Opacity = 0.1;
            }
        }

        private async void OnShowSavedLocationsClick(object sender, RoutedEventArgs e)
        {
            var connection = this.GetDbConnectionAsync();
            var userData = await connection.Table<Geoposition>()
                .ToListAsync();
           
            var userDataAsString = new StringBuilder();
            foreach (var userItem in userData)
            {
                userDataAsString.AppendLine(string.Format("Latitude:{0}, Longitude:{1}", userItem.Coordinate.Latitude, userItem.Coordinate.Longitude));
            }

            this.SavedLocations.Text = userDataAsString.ToString();
        }

        private async void OnSaveCurrentLocationClick(object sender, RoutedEventArgs e)
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                // Make the request for the current position
                Geoposition pos = await this.geolocator.GetGeopositionAsync().AsTask();
                var lat = pos.Coordinate.Latitude;
                var lon = pos.Coordinate.Longitude;

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                 {
                     var connection = this.GetDbConnectionAsync();
                     var position = new GeoPositionPoint(lat, lon);
                     var result = await connection.InsertAsync(position);

                     //var message = string.Format("Your current location is:  Latitude:{0}, Longitude: {1}", lat, lon);
                     //var dialog = new MessageDialog(message);
                     //dialog.Commands.Add(new UICommand("OK"));
                     //await dialog.ShowAsync();
                 });
            }
            else
            {
                var dialog = new MessageDialog("You denied geolocation or you don't have a gps enabled. Current location isnt saved!");
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
    }
}
