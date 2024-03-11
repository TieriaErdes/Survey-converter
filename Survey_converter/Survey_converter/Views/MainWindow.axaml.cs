using Avalonia.Controls;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Survey_converter.ViewModels;

namespace Survey_converter.Views
{
    public partial class MainWindow : Window
    {
        private bool vmDataContext_flag;

        public MainWindow()
        {
            InitializeComponent();

            vmDataContext_flag = (MainWindowViewModel)DataContext == null ? true : false;
        }

        

        private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (ListOfSignals.SelectedItems == null || ListOfSignals.SelectedItems.Count == 0) 
                return;

            if (vmDataContext_flag)
                ((MainWindowViewModel)DataContext!).Update_selectedSignalsNames(ListOfSignals.SelectedItems, ListOfSignals.SelectedItems.Count);
        }
    }
}