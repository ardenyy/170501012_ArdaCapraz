using Reisebuero.Models;
using Reisebuero.Services;
using System.Windows;

namespace Reisebuero
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CreateAdminIfNotExist();
            base.OnStartup(e);
        }

        /// <summary>
        /// Creates admin account on first run of application with id = 0, password = admin.
        /// </summary>
        /// <returns></returns>
        private async void CreateAdminIfNotExist()
        {
            GenericDataService<Employee> employeeService = new GenericDataService<Employee>(new ReisebueroDbContextFactory());
            GenericDataService<LoginForm> loginService = new GenericDataService<LoginForm>(new ReisebueroDbContextFactory());
            Employee? returnedEmployee = await employeeService.Get(0);
            if (returnedEmployee == null)
            {
                Employee employee = new Employee();
                employee.ID = 0;
                employee.Name = "admin";
                employee.Surname = "admin";
                employee.Role = Utilities.Constants.Role.Administrator;
                LoginForm loginForm = new LoginForm();
                loginForm.ID = employee.ID;
                loginForm.Password = "admin";
                _ = await employeeService.Create(employee);
                _ = await loginService.Create(loginForm);
            }
        }
    }
}
