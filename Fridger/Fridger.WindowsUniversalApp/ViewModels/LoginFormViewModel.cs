using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fridger.WindowsUniversalApp.ViewModels
{
    public class LoginFormViewModel : ViewModelBase
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        
        public LoginFormViewModel(string username,  string password)
        {
            UserName = username;            
            Password = password;
        }
        public LoginFormViewModel()
            :this(string.Empty, string.Empty)
        {
        }

        public LoginFormViewModel(LoginFormViewModel newRegisterForm)
          : this(newRegisterForm.UserName,newRegisterForm.Password)
        {

        }

        public bool Equals(LoginFormViewModel obj)
        {
            return this.UserName == obj.UserName &&
              this.Password == obj.Password;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LoginFormViewModel;

            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

    }
}
