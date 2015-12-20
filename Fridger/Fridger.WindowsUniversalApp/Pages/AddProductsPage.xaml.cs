using Fridger.WindowsUniversalApp.Models;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Fridger.WindowsUniversalApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddProductsPage : Page
    {
        public AddProductsPage()
        {
            this.InitializeComponent();
            this.InitAsync();
        }

        private async void AddNewProductButtonClick(object sender, RoutedEventArgs e)
        {
            var connection = this.GetDbConnectionAsync();
            var product = new Product
            {
                Name = this.NameTextBox.Text,
                ShouldBeBougth = this.shouldAddToToBuyList.IsChecked
            };
            var dbProduct = await connection.Table<Product>()
                .Where(p=> p.Name.ToLower() == product.Name.ToLower())
                .FirstOrDefaultAsync();
            if (dbProduct==null)
            {
                await this.InsertProductAsync(product);
                // TODO: Notify user
            }
            else
            {
                dbProduct.ShouldBeBougth = product.ShouldBeBougth;
                await connection.UpdateAsync(dbProduct);
            }           
        }

        private async void GetAllFromFridgeButtonClick(object sender, RoutedEventArgs e)
        {
            var userData = await this.GetAllProductAsync(false);
            var userDataAsString = new StringBuilder();
            foreach (var userItem in userData)
            {
                userDataAsString.AppendLine(userItem.ToString());
            }

            this.AllItemsFromFridgeTextBlock.Text = userDataAsString.ToString();
        }

        private async void GetAllFromToBuyListButtonClick(object sender, RoutedEventArgs e)
        {
            var userData = await this.GetAllProductAsync(true);
            var userDataAsString = new StringBuilder();
            foreach (var userItem in userData)
            {
                userDataAsString.AppendLine(userItem.ToString());
            }

            this.AllItemsFromToBuyListTextBlock.Text = userDataAsString.ToString();
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
                .Where(p=>p.ShouldBeBougth == shouldBeBought)
                .ToListAsync();
            return result;
        }
    }
}
