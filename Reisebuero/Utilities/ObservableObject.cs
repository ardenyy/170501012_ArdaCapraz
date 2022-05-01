using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Reisebuero.Utilities
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            field = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected T SetProperty<T>(T newValue, T? oldValue = default(T), [CallerMemberName] string? propertyName = null)
        {
            if (oldValue != null)
            {
                if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                {
                    return oldValue;
                }
            }
            OnPropertyChanged(propertyName);
            return newValue;
        }
    }
}
