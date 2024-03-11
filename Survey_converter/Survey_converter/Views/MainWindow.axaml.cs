using Avalonia.Controls;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Survey_converter.ViewModels;
using System.Globalization;

namespace Survey_converter.Views
{
    public partial class MainWindow : Window
    {
        private bool vmDataContext_flag;

        public MainWindow()
        {
            InitializeComponent();

            ConvertButton.IsEnabled = false;

            vmDataContext_flag = (MainWindowViewModel)DataContext == null ? true : false;
        }

        

        private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (ListOfSignals.SelectedItems == null || ListOfSignals.SelectedItems.Count == 0) 
                return;

            if (vmDataContext_flag)
            {
                ConvertButton.IsEnabled = true;
                ((MainWindowViewModel)DataContext!).Update_selectedSignalsNames(ListOfSignals.SelectedItems, ListOfSignals.SelectedItems.Count);
            }
        }

        private void RuItem_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Languages.Resources.Culture = new CultureInfo("ru-RU");
        }

        private void EngItem_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Languages.Resources.Culture = new CultureInfo("en-US");
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (toCSVButton.IsChecked == true)
            {
                ((MainWindowViewModel)DataContext).ToCSVCommand();
            }
            else if (toEDFButton.IsChecked == false)
            {
                ((MainWindowViewModel)DataContext).ToEDFCommand();
            }
        }
    }
}