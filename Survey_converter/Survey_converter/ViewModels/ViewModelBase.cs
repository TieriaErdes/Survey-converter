using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Survey_converter.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string>? _errorMessages;

        protected ViewModelBase()
        {
            ErrorMessages = new ObservableCollection<string>();
        }
    }
}