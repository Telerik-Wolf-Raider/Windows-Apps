using Fridger.WindowsUniversalApp.Helpers;
using Fridger.WindowsUniversalApp.Models;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Fridger.WindowsUniversalApp.Pages
{
    public sealed partial class AddProductsPage : Page
    {
        public AddProductsPage()
        {
            this.InitializeComponent();
            this.InitAsync();
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

        private async void CaptureProductPictureButtonClick(object sender, RoutedEventArgs e)
        {
            var camera = new CameraCaptureUI();

            var photo = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {
                ProductCapture.Source = new BitmapImage(new Uri(photo.Path));
            }
        }

        private async void AddProductPictureButtonClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            string message;
            if (file != null)
            {                
                this.TestingImage.Source = new BitmapImage(new Uri("ms-appx:///Images/" + file.Name));
                message = string.Format("Successful upload of picture!");
            }
            else
            {
                message = string.Format("No file picked!");

            }

            Notifier.Notify(message);
        }

        private async void AddNewProductButtonClick(object sender, RoutedEventArgs e)
        {
            string source = "ms-appx:///Images/Not_available.jpg";
            if (this.TestingImage.Source != null)
            {
                source = (this.TestingImage.Source as BitmapImage).UriSource.OriginalString;
            }

            var name = this.NameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                Notifier.Notify("Please enter a name");
                return;
            }

            var connection = this.GetDbConnectionAsync();
            var product = new Product
            {
                Name = name,
                ImageSource = source,
                ShouldBeBougth = this.shouldAddToToBuyList.IsChecked
            };

            var dbProduct = await connection.Table<Product>()
                .Where(p => p.Name.ToLower() == product.Name.ToLower())
                .FirstOrDefaultAsync();
            if (dbProduct == null)
            {
                await this.InsertProductAsync(product);
                Notifier.Notify("Product added!");
            }
            else
            {
                dbProduct.ShouldBeBougth = product.ShouldBeBougth;
                dbProduct.ImageSource = source;
                await connection.UpdateAsync(dbProduct);
                Notifier.Notify("Product updated!");
            }

            this.TestingImage.Source = null;
            this.NameTextBox.Text = string.Empty;
        }

        //private async void GetAllFromFridgeButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var userData = await this.GetAllProductAsync(false);
        //    var userDataAsString = new StringBuilder();
        //    foreach (var userItem in userData)
        //    {
        //        userDataAsString.AppendLine(userItem.ToString());
        //    }

        //    this.AllItemsFromFridgeTextBlock.Text = userDataAsString.ToString();
        //}

        //private async void GetAllFromToBuyListButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var userData = await this.GetAllProductAsync(true);
        //    var userDataAsString = new StringBuilder();
        //    foreach (var userItem in userData)
        //    {
        //        userDataAsString.AppendLine(userItem.ToString());
        //    }

        //    this.AllItemsFromToBuyListTextBlock.Text = userDataAsString.ToString();
        //}

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

        private async void InitAsync()
        {
            var connection = this.GetDbConnectionAsync();
            await connection.CreateTableAsync<Product>();
        }

        private async Task<int> InsertProductAsync(Product product)
        {
            var connection = this.GetDbConnectionAsync();
            var result = await connection.InsertAsync(product);
            return result;
        }

        private async Task<List<Product>> GetAllProductAsync(bool? shouldBeBought)
        {
            var connection = this.GetDbConnectionAsync();
            var result = await connection.Table<Product>()
                .Where(p => p.ShouldBeBougth == shouldBeBought)
                .ToListAsync();
            return result;
        }
    }
}
