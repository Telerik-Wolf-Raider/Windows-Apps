using Fridger.WindowsUniversalApp.Extensions;
using Fridger.WindowsUniversalApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fridger.WindowsUniversalApp.ViewModels
{    
    public class ProductsContentViewModel : ViewModelBase, IContentViewModel
    {
        public ObservableCollection<ProductViewModel> products;
        private ICommand saveCommand;
        private ICommand deleteCommand;

        public IEnumerable<ProductViewModel> Products
        {
            get
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<ProductViewModel>();
                }

                return this.products;
            }
            set
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<ProductViewModel>();
                }

                this.products.Clear();
                value.ForEach(this.products.Add);
            }
        }

        public ICommand Save
        {
            get
            {
                if (this.saveCommand == null)
                {
                    this.saveCommand = new DelegateCommand<ProductsContentViewModel>((newProduct) =>
                    {
                        this.products.Add(new ProductViewModel(newProduct));
                    });
                }
                return this.saveCommand;
            }
        }

        public ICommand Delete
        {
            get
            {
                if (this.deleteCommand == null)
                {
                    this.deleteCommand = new DelegateCommand<ProductViewModel>((newProduct) =>
                    {
                        this.products.Remove(newProduct);
                    });
                }
                return this.deleteCommand;
            }
        }
    }
}
