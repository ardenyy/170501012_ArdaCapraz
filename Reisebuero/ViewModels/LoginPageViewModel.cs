using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Reisebuero.Models;
using Reisebuero.Utilities;

namespace Reisebuero.ViewModels
{
    public class LoginPageViewModel : ObservableObject
    {
        private string _loginID = "";
        private string _loginPassword = "";

        public string LoginID
        {
            get { return _loginID; }
            set {  SetProperty(ref _loginID, value); }
        }
        public string LoginPassword
        {
            get { return _loginPassword; }
            set { SetProperty(ref _loginPassword, value); }
        }

        public RelayCommand LoginCommand { get; set; }

        public LoginPageViewModel()
        {             
            LoginCommand = new RelayCommand(
                param => Login(),
                param => !String.IsNullOrWhiteSpace(LoginID) 
                         && UInt16.TryParse(LoginID, out ushort r) 
                         && !String.IsNullOrEmpty(LoginPassword)
            );
        }

        private void Login()
        {            
        }
    }
}
