using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fridger.WindowsUniversalApp.ViewModels
{
   public  class FridgeInventoryViewModel : ViewModelBase, IPageViewModel
    {
        public FridgeInventoryViewModel(IContentViewModel contentViewModel)
        {
            this.ContentViewModel = contentViewModel;
        }

        public string Title
        {
            get
            {
                return "Fridge inventory page";
            }
        }

        public IContentViewModel ContentViewModel { get; set; }
    }
}
