using Fridger.WindowsUniversalApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace Fridger.WindowsUniversalApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingModePage : Page
    {
        public ShoppingModePage()
        {
            this.InitializeComponent();
            this.Products = new List<ProductViewModel>() {

                new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 1" },
                new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 2" },
                new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 3" },
                new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName="Product name 4" },
            };
            this.ProductTest = new ProductViewModel { ImgSource = "https://i.ytimg.com/vi/UIrEM_9qvZU/maxresdefault.jpg", ProductName = "Product name 1" };

            this.DataContext = Products;
        }

        public IEnumerable<ProductViewModel> Products { get; set; }

        public ProductViewModel ProductTest { get; set; }
    }
}
