using Fridger.WindowsUniversalApp.Controls;
using Fridger.WindowsUniversalApp.Helpers;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Input;
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
    public sealed partial class FridgeInventoryPage : Page
    {
        public FridgeInventoryPage()
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

        private async void InitAsync()
        {
            var contentViewModel = new ProductsContentViewModel();
            this.ContentViewModel = contentViewModel;
            contentViewModel.Products = await GetProductsList();
            this.ViewModel = new FridgeInventoryViewModel(contentViewModel);
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

        private async Task<IEnumerable<ProductViewModel>> GetProductsList()
        {
            var userData = await this.GetAllProductAsync(false);
            var products = new List<ProductViewModel>();

            foreach (var userItem in userData)
            {
                products.Add(new ProductViewModel(userItem.Name, userItem.ImageSource, userItem.Id.ToString()));
            }

            return products;
        }

        private async Task<List<Product>> GetAllProductAsync(bool? shouldBeBought)
        {
            var connection = this.GetDbConnectionAsync();
            var result = await connection.Table<Product>()
                .Where(p => p.ShouldBeBougth == shouldBeBought)
                .ToListAsync();
            return result;
        }

        public FridgeInventoryViewModel ViewModel
        {
            get { return this.DataContext as FridgeInventoryViewModel; }
            set { this.DataContext = value; }
        }

        public IContentViewModel ContentViewModel { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

        private async void ProductDetails_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var product = sender as ProductDetails;
            string message;

            if (product.Opacity < 1)
            {
                product.Opacity = 1;
                message = string.Format("You added the product {0} back to the fridge inventory!", product.ProductName);
                UpdateProduct(product.ProductId, false);
            }
            else
            {
                product.Opacity = 0.1;
                message = string.Format("You have to buy {0} now!", product.ProductName);
                UpdateProduct(product.ProductId, true);
            }

            Notifier.Notify(message);
        }

        private async void ProductDetails_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == HoldingState.Completed)
            {
                return;
            }    
            var product = sender as ProductDetails;
            string message;
            var commandLabel = "Delete";
            bool result = await Notifier.Ask("Are you sure you want to delete this product?", commandLabel) == commandLabel;
            if (result)
            {
                message = string.Format("You deleted {0}!", product.ProductName);
                RemoveProduct(product.ProductId);
                product.Visibility = Visibility.Collapsed;
                Notifier.Notify(message);
            }
        }
        
        private async void RemoveProduct(string productId)
        {
            var connection = this.GetDbConnectionAsync();
            int id = int.Parse(productId);
            var dbProduct = await connection.Table<Product>()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (dbProduct == null)
            {
                Notifier.Notify("Error Happened");
            }
            else
            {
                int result = await connection.DeleteAsync(dbProduct);
            }
        }


        private async void UpdateProduct(string productId, bool shouldBeBought)
        {
            var connection = this.GetDbConnectionAsync();
            int id = int.Parse(productId);
            var dbProduct = await connection.Table<Product>()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (dbProduct == null)
            {
                Notifier.Notify("Error Happened");
            }
            else
            {
                dbProduct.ShouldBeBougth = shouldBeBought;
                int result = await connection.UpdateAsync(dbProduct);
            }
        }


    }
}
