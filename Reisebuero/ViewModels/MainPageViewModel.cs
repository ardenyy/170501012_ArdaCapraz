using Reisebuero.Models;
using Reisebuero.Services;
using Reisebuero.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reisebuero.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private Employee _employee;
        private object _editBuffer;
        private readonly ReisebueroDbContextFactory _dbFactory = new ReisebueroDbContextFactory();

        private string _name = "";
        private string _role = "";

        private ObservableCollection<Tour> _tours;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Employee> _employees;

        private ObservableCollection<Tour> _selectedTour = new ObservableCollection<Tour>();
        private ObservableCollection<Customer> _selectedCustomer = new ObservableCollection<Customer>();
        private ObservableCollection<Employee> _selectedEmployee = new ObservableCollection<Employee>();

        private ObservableCollection<Tour> _customerTours = new ObservableCollection<Tour>();
        private ObservableCollection<Customer> _toursCustomers = new ObservableCollection<Customer>();
        private ObservableCollection<TourSale> _employeesTourSales = new ObservableCollection<TourSale>();

        private bool _isReadOnly = true;

        private string _visibilitySelectedTour = Constants.VISIBILITY_HIDDEN;
        private string _visibilitySelectedCustomer = Constants.VISIBILITY_HIDDEN;
        private string _visibilitySelectedEmployee = Constants.VISIBILITY_HIDDEN;
        
        private string _visibilityEditCancel = Constants.VISIBILITY_HIDDEN;

        private string _infoText = "";

        private string _indexAddRemove = "";


        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }
        public ObservableCollection<Tour> Tours
        {
            get => _tours;
            set => SetProperty(ref _tours, value);
        }
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }
        public ObservableCollection<Tour> SelectedTour
        {
            get => _selectedTour;
            set => SetProperty(ref _selectedTour, value);
        }
        public ObservableCollection<Customer> SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }
        public ObservableCollection<Employee> SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }
        public ObservableCollection<Tour> CustomersTours
        {
            get => _customerTours;
            set => SetProperty(ref _customerTours, value);
        }
        public ObservableCollection<Customer> ToursCustomers
        {
            get => _toursCustomers;
            set => SetProperty(ref _toursCustomers, value);
        }
        public ObservableCollection<TourSale> EmployeesTourSales
        {
            get => _employeesTourSales;
            set => SetProperty(ref _employeesTourSales, value);
        }
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetProperty(ref _isReadOnly, value);
        }
        public string VisibilitySelectedTour
        {
            get => _visibilitySelectedTour;
            set => SetProperty(ref _visibilitySelectedTour, value);
        }
        public string VisibilitySelectedCustomer
        {
            get => _visibilitySelectedCustomer;
            set => SetProperty(ref _visibilitySelectedCustomer, value);
        }
        public string VisibilitySelectedEmployee
        {
            get => _visibilitySelectedEmployee;
            set => SetProperty(ref _visibilitySelectedEmployee, value);
        }
        public string VisibilityEditCancel
        {
            get => _visibilityEditCancel;
            set => SetProperty(ref _visibilityEditCancel, value);
        }
        public string InfoText
        {
            get => _infoText;
            set => SetProperty(ref _infoText, value);
        }
        public string IndexAddRemove
        {
            get => _indexAddRemove;
            set => SetProperty(ref _indexAddRemove, value);
        }

        public RelayCommand DoubleClickCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        public MainPageViewModel(Employee employee)
        {
            DoubleClickCommand = new RelayCommand(
                param => GetInfo(param as BaseModel)
            );
            CreateCommand = new RelayCommand(
                param => CreateModel(param)
            );
            EditCommand = new RelayCommand(
               param => EditModel(param),
               param => param != null
            );
            ConfirmCommand = new RelayCommand(
                async param =>
                {
                    if (await ConfirmEdit(param)) GetInfo(_employee);
                }
            );
            DeleteCommand = new RelayCommand(
                async param => {
                    if (await DeleteModel(param)) GetInfo(_employee);
                },
                param =>
                {
                    return (param != null
                            && (param is not Employee
                                || (param as Employee)?.ID != _employee.ID
                    ));
                }
            );
            AddCommand = new RelayCommand(
                async param =>
                {
                    if (await AddToList(param)) GetInfo(_employee);
                },
                param =>
                {
                    int index = -1;
                    return int.TryParse(IndexAddRemove, out index);
                }
            );
            RemoveCommand = new RelayCommand(
                async param =>
                {
                    if (await RemoveFromList(param)) GetInfo(_employee);
                },
                param =>
                {
                    int index = -1;
                    return int.TryParse(IndexAddRemove, out index);
                }
            );
            GetInfo(employee);
        }

        private async void GetInfo(BaseModel? model)
        {
            if (model != null && model is Employee)
            {
                var employee = model as Employee;
                _employee = employee;
                Name = employee.Name + " " + employee.Surname;
                Role = employee.Role.ToString();
                if (employee.Role == Constants.Role.Administrator) { }
            }
            /*
            var tourService = new AsyncGenericDataService<Tour>(_dbFactory);
            var customerService = new AsyncGenericDataService<Customer>(_dbFactory);
            var employeeService = new AsyncGenericDataService<Employee>(_dbFactory);

            Tours = new ObservableCollection<Tour>(await tourService.GetAllAsync());
            Customers = new ObservableCollection<Customer>(await customerService.GetAllAsync());
            Employees = new ObservableCollection<Employee>(await employeeService.GetAllAsync());

            System.Diagnostics.Trace.WriteLine(Employees[0].TourSales.Count);
            */
            var factory = _dbFactory;
            using var context = factory.CreateDbContext();
            List<TourSale> entities = context.TourSales.ToList();
            context.AttachRange(entities);
            Tours = new ObservableCollection<Tour>(context.Tours.ToList());
            Customers = new ObservableCollection<Customer>(context.Customers.ToList());
            Employees = new ObservableCollection<Employee>(context.Employees.ToList());
            _employee = Employees.Where(e => e.ID == _employee.ID).Single();

            //System.Diagnostics.Trace.WriteLine(Employees[0].TourSales.Count);
            if (model is Employee)
            {
                SelectItem(_employee);
            }
            else
            {
                SelectItem(model);
            }
        }

        private async Task<bool> AddToList(object param)
        {
            int index = -1;
            if (!int.TryParse(IndexAddRemove, out index))
            {
                InfoText = "ID Should Be Numeric";
                return false;
            }
            if ((string)param == "Tour")
            {
                var tour = SelectedTour[0];
                if (tour.TourSales.Find(x => x.Customer.ID == index) != null)
                {
                    InfoText = "ID already exist";
                    return false;
                }
                if (tour.Capacity <= tour.TourSales.Count)
                {
                    InfoText = "Full Capacity" + tour.TourSales[0].Customer.ID;
                    return false;
                }
                var customerService = new AsyncGenericDataService<Customer>(_dbFactory);
                var tourSaleService = new AsyncGenericDataService<TourSale>(_dbFactory);
                var tourService = new AsyncGenericDataService<Tour>(_dbFactory);
                var customer = await customerService.GetAsync(index);
                if (customer == null)
                {
                    InfoText = "Error";
                    return false;
                }
                var sales = await tourSaleService.GetAllAsync();
                var salesList = sales.ToList();
                TourSale sale = salesList.Find(x => x.TourID == tour.ID && x.CustomerID == index);
                if (sale != null)
                {
                    InfoText = "Error: Already Created";
                    return false;
                }
                TourSale newSale = new TourSale()
                {
                    Tour = tour,
                    Customer = customer,
                    Employee = _employee,
                    Date = DateTime.Now
                };
                System.Diagnostics.Trace.WriteLine(newSale);
                await tourSaleService.CreateAsync(newSale);
                InfoText = "Succesfuly Added";
                return true;
            }
            else if ((string)param == "Customer")
            {
                var customer = SelectedCustomer[0];
                if (customer.TourSales.Find(x => x.Tour.ID == index) != null)
                {
                    InfoText = "ID already exist";
                    return false;
                }
                var tourService = new AsyncGenericDataService<Tour>(_dbFactory);
                var tourSaleService = new AsyncGenericDataService<TourSale>(_dbFactory);
                var tour = await tourService.GetAsync(index);
                if (tour == null)
                {
                    InfoText = "Error";
                    return false;
                }
                if (tour.Capacity <= tour.TourSales.Count)
                {
                    InfoText = "Full Capacity";
                    return false;

                }
                var sales = await tourSaleService.GetAllAsync();
                var salesList = sales.ToList();
                TourSale sale = salesList.Find(x => x.TourID == tour.ID && x.CustomerID == index);
                if (sale != null)
                {
                    InfoText = "Error: Already Created";
                    return false;
                }
                TourSale newSale = new TourSale()
                {
                    Tour = tour,
                    Customer = customer,
                    Employee = _employee,
                    Date = DateTime.Now
                };
                await tourSaleService.CreateAsync(newSale);
                InfoText = "Succesfuly Added";
                return true;
            }
            InfoText = "Error: Type Error: " + param;
            return false;
        }

        private async Task<bool> RemoveFromList(object param)
        {
            int index = -1;
            if (!int.TryParse(IndexAddRemove, out index))
            {
                InfoText = "ID Should Be Numeric";
                return false;
            }
            if ((string)param == "Tour")
            {
                var tour = SelectedTour[0];
                if (tour.TourSales.Find(x => x.Customer.ID == index) == null)
                {
                    InfoText = "ID does not exist";
                    return false;
                }
                var tourSaleService = new AsyncGenericDataService<TourSale>(_dbFactory);
                var sales = await tourSaleService.GetAllAsync();
                var salesList = sales.ToList();
                TourSale? sale = salesList.Find(x => x.TourID == tour.ID && x.CustomerID == index);
                if (sale == null)
                {
                    InfoText = "Error: Sale does not Exist";
                    return false;
                }
                await tourSaleService.DeleteAsync(sale.ID);
                InfoText = "Succesfuly Removed";
                return true;
            }
            else if ((string)param == "Customer")
            {
                var customer = SelectedCustomer[0];
                if (customer.TourSales.Find(x => x.Tour.ID == index) == null)
                {
                    InfoText = "ID does not exist";
                    return false;
                }
                var tourSaleService = new AsyncGenericDataService<TourSale>(_dbFactory);
                var sales = await tourSaleService.GetAllAsync();
                var salesList = sales.ToList();
                TourSale? sale = salesList.Find(x => x.TourID == index && x.CustomerID == customer.ID);
                if (sale == null)
                {
                    InfoText = "Error: Sale does not Exist";
                    return false;
                }
                await tourSaleService.DeleteAsync(sale.ID);
                InfoText = "Succesfuly Removed";
                return true;
            }
            InfoText = "Error: Type Error: " + param;
            return false;
        }

        private void CreateModel(object param)
        {
            System.Diagnostics.Trace.WriteLine("create " + param);

            IsReadOnly = false;
            VisibilityEditCancel = Constants.VISIBILITY_VISIBLE;
            _editBuffer = null;
            if ((string)param == "Tour")
            {
                VisibilitySelectedTour = Constants.VISIBILITY_VISIBLE;
                VisibilitySelectedCustomer = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedEmployee = Constants.VISIBILITY_HIDDEN;
                SelectedTour.Clear();
                SelectedTour.Add(new Tour());
            }
            else if ((string)param == "Customer")
            {
                VisibilitySelectedTour = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedCustomer = Constants.VISIBILITY_VISIBLE;
                VisibilitySelectedEmployee = Constants.VISIBILITY_HIDDEN;
                SelectedCustomer.Clear();
                SelectedCustomer.Add(new Customer());
            }
            else if ((string)param == "Employee")
            {
                VisibilitySelectedTour = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedCustomer = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedEmployee = Constants.VISIBILITY_VISIBLE;
                SelectedEmployee.Clear();
                SelectedEmployee.Add(new Employee());
            }
        }

        private async Task<bool> DeleteModel(object param)
        {
            if (param is not BaseModel) return false;
            var model = param as BaseModel;
            if (param is Tour)
            {
                var service = new AsyncGenericDataService<Tour>(_dbFactory);
                var result = await service.GetAsync(model.ID);
                if (result != null)
                    await service.DeleteAsync(model.ID);
            }
            else if (param is Customer)
            {
                var service = new AsyncGenericDataService<Customer>(_dbFactory);
                var result = await service.GetAsync(model.ID);
                if (result != null)
                    await service.DeleteAsync(model.ID);
            }
            else if (param is Employee)
            {
                var service = new AsyncGenericDataService<Employee>(_dbFactory);
                var result = await service.GetAsync(model.ID);
                if (result != null)
                    await service.DeleteAsync(model.ID);
            }
            _editBuffer = null;
            SelectedCustomer.Clear();
            SelectedEmployee.Clear();
            SelectedTour.Clear();
            return true;
        }

        private void EditModel(object param)
        {
            _editBuffer = Functions.CopyObject((BaseModel)param);
            System.Diagnostics.Trace.WriteLine("_editbuffer null: " + (_editBuffer == null).ToString());
            IsReadOnly = false;
            VisibilityEditCancel = Constants.VISIBILITY_VISIBLE;

        }

        private async Task<bool> ConfirmEdit(object param)
        {
            bool status = false;
            var msg = param as string;
            if (msg != null && msg == "Cancel")
            {
                IsReadOnly = true;
                VisibilityEditCancel = Constants.VISIBILITY_HIDDEN;
                return status;
            }
            var buffer = Functions.CopyObject((BaseModel)_editBuffer);
            if (param is not BaseModel || param == null)
                return status;
            bool idChanged = false;
            if (buffer == null || !(buffer).ID.Equals((param as BaseModel)?.ID))
                idChanged = true;
            if ((param as BaseModel).ID < 0)
            {
                InfoText = "ID cant be smaller than 0";
                return status;
            }
            if (param is Tour)
            {
                var tour = param as Tour;
                if (tour == buffer as Tour)
                {
                    return status;
                }
                var service = new AsyncGenericDataService<Tour>(_dbFactory);
                if (!tour.CheckObjectData())
                {
                    InfoText = "Error with Infos";
                    IsReadOnly = true;
                    SelectedTour.Clear();
                    if (buffer != null)
                        SelectedTour.Add(buffer as Tour);
                    return status;
                }
                if (idChanged)
                {
                    Tour? a = await service.GetAsync(tour.ID);
                    if (a != null)
                    {
                        InfoText = "ID is alread used";
                        IsReadOnly = true;
                        SelectedTour.Clear();
                        if (buffer != null)
                            SelectedTour.Add(buffer as Tour);
                        return status;
                    }
                    IsReadOnly = true;
                    System.Diagnostics.Trace.WriteLine("buffer ID: " + buffer?.ID);
                    if (buffer != null)
                        await service.DeleteAsync((buffer as Tour).ID);
                    await service.CreateAsync(tour);
                    VisibilityEditCancel = Constants.VISIBILITY_HIDDEN;
                    status = true;
                    return status;
                }
                IsReadOnly = true;
                await service.UpdateAsync(tour.ID, tour);
                VisibilityEditCancel = Constants.VISIBILITY_HIDDEN;
                status = true;
            }
            else if (param is Customer)
            {
                var customer = param as Customer;
                if (customer == buffer as Customer)
                {
                    return status;
                }
                var service = new AsyncGenericDataService<Customer>(_dbFactory);
                if (!customer.CheckObjectData())
                {
                    InfoText = "Error with Infos";
                    IsReadOnly = true;
                    SelectedCustomer.Clear();
                    if (buffer != null)
                        SelectedCustomer.Add(buffer as Customer);
                    return status;
                }
                if (idChanged)
                {
                    Customer? a = await service.GetAsync(customer.ID);
                    if (a != null)
                    {
                        InfoText = "ID is alread used";
                        IsReadOnly = true;
                        SelectedCustomer.Clear();
                        if (buffer != null)
                            SelectedCustomer.Add(buffer as Customer);
                        return status;
                    }
                    IsReadOnly = true;
                    if (buffer != null)
                        await service.DeleteAsync((buffer as Customer).ID);
                    await service.CreateAsync(customer);
                    VisibilityEditCancel = Constants.VISIBILITY_HIDDEN;
                    status = true;
                    return status;
                }
                IsReadOnly = true;
                await service.UpdateAsync(customer.ID, customer);
                VisibilityEditCancel = Constants.VISIBILITY_HIDDEN;
                status = true;
            }
            else if (param is Employee)
            {
                var employee = param as Employee;
                if (employee == buffer as Employee)
                {
                    return status;
                }
                var service = new AsyncGenericDataService<Employee>(_dbFactory);
                if (!employee.CheckObjectData())
                {
                    InfoText = "Error with Infos";
                    IsReadOnly = true;
                    SelectedEmployee.Clear();
                    if (buffer != null)
                        SelectedEmployee.Add(buffer as Employee);
                    return status;
                }
                if (idChanged)
                {
                    Employee? a = await service.GetAsync(employee.ID);
                    if (a != null)
                    {
                        InfoText = "ID is alread used";
                        IsReadOnly = true;
                        SelectedEmployee.Clear();
                        if (buffer != null)
                            SelectedEmployee.Add(buffer as Employee);
                        return status;
                    }
                    IsReadOnly = true;
                    if (buffer != null)
                        await service.DeleteAsync((buffer as Employee).ID);
                    await service.CreateAsync(employee);
                    VisibilityEditCancel = Constants.VISIBILITY_HIDDEN;
                    status = true;
                    return status;
                }
                IsReadOnly = true;
                await service.UpdateAsync(employee.ID, employee);
                VisibilityEditCancel = Constants.VISIBILITY_HIDDEN;
                status = true;
            }
            return status;
        }

        private void SelectItem(object param)
        {
            System.Diagnostics.Trace.WriteLine("select " + param);
            if (param is not BaseModel || param == null) return;
            if (param is Tour)
            {
                SelectedTour.Clear();
                SelectedTour.Add(param as Tour);
                ToursCustomers = new ObservableCollection<Customer>((param as Tour).TourSales.Select(x => x.Customer));
                VisibilitySelectedTour = Constants.VISIBILITY_VISIBLE;
                VisibilitySelectedCustomer = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedEmployee = Constants.VISIBILITY_HIDDEN;
                System.Diagnostics.Trace.WriteLine("Count: " + ToursCustomers.Count);
            }
            else if (param is Customer)
            {
                SelectedCustomer.Clear();
                SelectedCustomer.Add(param as Customer);
                CustomersTours = new ObservableCollection<Tour>((param as Customer).TourSales.Select(x => x.Tour));
                VisibilitySelectedTour = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedCustomer = Constants.VISIBILITY_VISIBLE;
                VisibilitySelectedEmployee = Constants.VISIBILITY_HIDDEN;
                System.Diagnostics.Trace.WriteLine("Count: " + CustomersTours.Count);
            }
            else if (param is Employee)
            {
                SelectedEmployee.Clear();
                SelectedEmployee.Add(param as Employee);
                EmployeesTourSales = new ObservableCollection<TourSale>((param as Employee).TourSales);
                VisibilitySelectedTour = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedCustomer = Constants.VISIBILITY_HIDDEN;
                VisibilitySelectedEmployee = Constants.VISIBILITY_VISIBLE;
                System.Diagnostics.Trace.WriteLine("selected empl");
                System.Diagnostics.Trace.WriteLine("Count: " + EmployeesTourSales.Count);
            }
        }
    }
}
