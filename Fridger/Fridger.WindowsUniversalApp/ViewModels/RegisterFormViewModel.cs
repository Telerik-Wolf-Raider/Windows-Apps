using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fridger.WindowsUniversalApp.ViewModels
{
    public class RegisterFormViewModel: ViewModelBase
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public RegisterFormViewModel(string username, string email, string password, string confirmPassword)
        {
            UserName = username;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
        public RegisterFormViewModel()
            :this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public RegisterFormViewModel(RegisterFormViewModel newRegisterForm)
          : this(newRegisterForm.UserName, newRegisterForm.Email, newRegisterForm.Password, newRegisterForm.ConfirmPassword)
        {

        }

        public bool Equals(RegisterFormViewModel obj)
        {
            return this.UserName == obj.UserName &&
              this.Email == obj.Email &&
              this.Password == obj.Password &&
              this.ConfirmPassword == obj.ConfirmPassword;
        }

        public override bool Equals(object obj)
        {
            var other = obj as RegisterFormViewModel;

            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

    }
}
