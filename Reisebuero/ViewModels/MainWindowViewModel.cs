using Reisebuero.Models;
using Reisebuero.Utilities;

namespace Reisebuero.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private LoginPageViewModel _loginPageViewModel;
        private MainPageViewModel _mainPageViewModel;

        public MainWindowViewModel()
        {
            _loginPageViewModel = new LoginPageViewModel
            {
                SuccessfulLoginCommand = new RelayCommand(
                    param => SuccessfulLogin(param))
            };
            CurrentViewModel = _loginPageViewModel;
        }

        private void SuccessfulLogin(object data)
        {
            var employee = data as Employee;
            if (employee == null) return;
            _mainPageViewModel = new MainPageViewModel(employee);
            CurrentViewModel = _mainPageViewModel;
        }
    }
}
