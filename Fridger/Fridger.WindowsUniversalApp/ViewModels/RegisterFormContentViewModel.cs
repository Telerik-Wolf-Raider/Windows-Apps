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
    public class RegisterFormContentViewModel : ViewModelBase, IContentViewModel
    {
        public ObservableCollection<RegisterFormViewModel> registerForms;
        private ICommand saveCommand;
        private ICommand deleteCommand;

        public IEnumerable<RegisterFormViewModel> RegisterForms
        {
            get
            {
                if (this.registerForms == null)
                {
                    this.registerForms = new ObservableCollection<RegisterFormViewModel>();
                }

                return this.registerForms;
            }
            set
            {
                if (this.registerForms == null)
                {
                    this.registerForms = new ObservableCollection<RegisterFormViewModel>();
                }

                this.registerForms.Clear();
                value.ForEach(this.registerForms.Add);
            }
        }

        public ICommand Save
        {
            get
            {
                if (this.saveCommand == null)
                {
                    this.saveCommand = new DelegateCommand<RegisterFormViewModel>((newSuperhero) =>
                    {
                        this.registerForms.Add(new RegisterFormViewModel(newSuperhero));
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
                    this.deleteCommand = new DelegateCommand<RegisterFormViewModel>((registerForm) =>
                    {
                        this.registerForms.Remove(registerForm);
                    });
                }
                return this.deleteCommand;
            }
        }
    }
}
