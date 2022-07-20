using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reisebuero.Models;
using Reisebuero.Services;
using Reisebuero.Utilities;

namespace Reisebuero.ViewModels
{
    public class LoginPageViewModel : ObservableObject
    {
        private string _loginID = "";
        private string _loginPassword = "";

        private bool _lockLogin = false;

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
        public RelayCommand SuccessfulLoginCommand { get; set; }

        public LoginPageViewModel()
        {
            LoginCommand = new RelayCommand(
                param => Login(),
                param => !String.IsNullOrWhiteSpace(LoginID)
                         && UInt16.TryParse(LoginID, out ushort r)
                         && !String.IsNullOrEmpty(LoginPassword)
                         && !_lockLogin
            );
        }

        private async void Login()
        {
            _lockLogin = true;
            var employeeService = new AsyncGenericDataService<Employee>(new ReisebueroDbContextFactory());
            var loginService = new AsyncGenericDataService<LoginForm>(new ReisebueroDbContextFactory());

            LoginForm loginForm = new LoginForm(); ;
            loginForm.ID = UInt16.Parse(_loginID);
            loginForm.Password = _loginPassword;

            LoginForm? loginFormResult = await loginService.GetAsync(loginForm.ID);
            if (loginFormResult == null)
            {
                System.Diagnostics.Trace.WriteLine("Account with given ID couldnt found");
                return;
            }
            if(loginFormResult.Password != loginForm.Password)
            {
                System.Diagnostics.Trace.WriteLine("Account with given Password couldnt found");
                return;
            }
            Employee returnedEmployee = (await employeeService.GetAsync(loginForm.ID))!;
            System.Diagnostics.Trace.WriteLine("Account found");
            System.Diagnostics.Trace.WriteLine("ID: " + returnedEmployee.ID + "\nName: " + returnedEmployee.Name);

            SuccessfulLoginCommand?.Execute(returnedEmployee);
            _lockLogin = false;
        }
    }
}
