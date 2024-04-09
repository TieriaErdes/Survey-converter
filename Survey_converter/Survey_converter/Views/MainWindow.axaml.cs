using Avalonia.Controls;
using Survey_converter.ViewModels;
using System.Globalization;

namespace Survey_converter.Views
{
    public partial class MainWindow : Window
    {
        private bool vmDataContext_flag;

        private bool selectedSignals_in_not_zero;
        private bool converterFormat_isSelected;
        private bool savePath_isInputted;

        public MainWindow()
        {
            InitializeComponent();

            ConvertButton.IsEnabled = false;

            selectedSignals_in_not_zero = false;
            converterFormat_isSelected = false;
            savePath_isInputted = false;
        }



        private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (ListOfSignals.SelectedItems == null || ListOfSignals.SelectedItems.Count == 0)
            {
                selectedSignals_in_not_zero = false;
                return;
            }

            Reset.IsEnabled = false;
            Finalization.IsEnabled = false;
            AddData.IsEnabled = false;

            selectedSignals_in_not_zero = true;

            ((MainWindowViewModel)DataContext!).Update_selectedSignalsNames(ListOfSignals.SelectedItems, ListOfSignals.SelectedItems.Count);

            if (selectedSignals_in_not_zero && converterFormat_isSelected && savePath_isInputted)
                ConvertButton.IsEnabled = true;
        }

        private void RuItem_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Languages.Resources.Culture = new CultureInfo("ru-RU");

            InitializeComponent();
        }

        private void EngItem_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Languages.Resources.Culture = new CultureInfo("en-US");

            InitializeComponent();
        }



        #region Conversion flags
        private const byte ToCSV = 0;
        private const byte ToEDF = 1;
        #endregion

        private void ToggleButton_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (toEDFButton.IsChecked == true) 
                toEDFButton.IsChecked = false;

            ((MainWindowViewModel)DataContext!).ActiveConvertingFlag = ToCSV;

            converterFormat_isSelected = true;

            if (selectedSignals_in_not_zero && converterFormat_isSelected && savePath_isInputted)
                ConvertButton.IsEnabled = true;
        }

        private void ToggleButton_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (toCSVButton.IsChecked == true)
                toCSVButton.IsChecked = false;

            ((MainWindowViewModel)DataContext!).ActiveConvertingFlag = ToCSV;

            converterFormat_isSelected = true;

            if (selectedSignals_in_not_zero && converterFormat_isSelected && savePath_isInputted)
                ConvertButton.IsEnabled = true;
        }

        private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            savePath_isInputted = true;

            if (selectedSignals_in_not_zero && converterFormat_isSelected && savePath_isInputted)
                ConvertButton.IsEnabled = true;
        }

        private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (TextBox.Text == string.Empty || ConvertButton.IsEnabled) return;

            savePath_isInputted = true;

            if (selectedSignals_in_not_zero && converterFormat_isSelected && savePath_isInputted)
                ConvertButton.IsEnabled = true;
        }

        private void Initialization_Button(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Reset.IsEnabled = true;
            Finalization.IsEnabled = true;
            AddData.IsEnabled = true;
        }

        private void Reset_Button(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Reset.IsEnabled = false;
            Finalization.IsEnabled = false;
            AddData.IsEnabled = false;

            ConvertButton.Flyout!.Hide();
        }
    }
}