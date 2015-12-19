using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fridger.WindowsUniversalApp.ViewModels
{
        public class ShoppingPageViewModel : ViewModelBase, IPageViewModel
    {
        public ShoppingPageViewModel(IContentViewModel contentViewModel)
        {
            this.ContentViewModel = contentViewModel;
        }

        public string Title
        {
            get
            {
                return "Shopping Mode page";
            }
        }

        public IContentViewModel ContentViewModel { get; set; }
    }
}
