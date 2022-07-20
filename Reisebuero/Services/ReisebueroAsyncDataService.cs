using Reisebuero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reisebuero.Services
{
    public class ReisebueroAsyncDataService
    {
        private AsyncGenericDataService<Customer> _customerDataService;
        private AsyncGenericDataService<Employee> _employeeDataService;
        private AsyncGenericDataService<LoginForm> _loginFormDataService;
        private AsyncGenericDataService<Tour> _tourDataService;
        private AsyncGenericDataService<TourSale> _tourSaleDataService;

        public ReisebueroAsyncDataService(ReisebueroDbContextFactory factory)
        {
            _customerDataService = new AsyncGenericDataService<Customer>(factory);
            _employeeDataService = new AsyncGenericDataService<Employee>(factory);
            _loginFormDataService = new AsyncGenericDataService<LoginForm>(factory);
            _tourDataService = new AsyncGenericDataService<Tour>(factory);
            _tourSaleDataService = new AsyncGenericDataService<TourSale>(factory);
        }

        public async Task<bool> CreateEmployee(Employee employee)
        {
            if(employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            Employee? searchResult = await _employeeDataService.GetAsync(employee.ID);
            if (searchResult != null)
            {
                return false;
            }
            await _employeeDataService.CreateAsync(employee);
            return true;
        }
    }
}
