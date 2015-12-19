namespace Fridger.WindowsUniversalApp.ViewModels
{
    public class ProductViewModel
    {
        private ProductsContentViewModel newProduct;

        public ProductViewModel(ProductsContentViewModel newProduct)
        {
            this.newProduct = newProduct;
        }
        public ProductViewModel()
                : this(string.Empty, string.Empty, string.Empty)
        {
        }


        public ProductViewModel(ProductViewModel newProduct)
                 : this(newProduct.ProductName, newProduct.ImgSource, newProduct.Value)
        {

        }

        public ProductViewModel(string productName, string imgSrc, string value)
        {
            this.ProductName = productName;
            this.ImgSource = imgSrc;
            this.Value = value;
        }

        public string ProductName { get; set; }
        public string ImgSource { get; set; }
        public string Value { get; internal set; }
    }
}
