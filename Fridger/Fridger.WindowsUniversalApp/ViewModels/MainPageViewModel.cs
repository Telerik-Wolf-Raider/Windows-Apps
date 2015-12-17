using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fridger.WindowsUniversalApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IPageViewModel
    {
        public MainPageViewModel(IContentViewModel contentViewModel)
        {
            this.ContentViewModel = contentViewModel;
        }

        public string Title
        {
            get
            {
                return "RegisterForms";
            }
        }

        public IContentViewModel ContentViewModel { get; set; }
    }
}
