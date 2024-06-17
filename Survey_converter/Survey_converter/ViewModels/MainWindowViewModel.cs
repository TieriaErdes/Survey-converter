using CommunityToolkit.Mvvm.Input;
using Survey_converter.Services;
using System.Collections.ObjectModel;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Survey_converter.Models;
using Avalonia.Platform.Storage;
using FileGenerationMechanism.MechanismLogic;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using DataStruct;

namespace Survey_converter.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        private ObservableCollection<string>? _ChannelNames;
        /// <summary>
        /// Реализует в себе динамическое хранение списка строк названий файлов сигналов.
        /// Значения строк получают Item в ListOfSignals (MainWindow)
        /// </summary>
        public ObservableCollection<string>? ChannelNames
        {
            get { return _ChannelNames; }
            set { SetProperty(ref _ChannelNames, value); }
        }

        /// <summary>
        /// Его дети хранят в себе информацию о массиве сигналов (их конфигурации)
        /// </summary>
        private SerializedObject<DataStruct.BOSMeth>? _Channels;

        /// <summary>
        /// Его дети хранят в себе информацию о фильтрах
        /// </summary>
        private SerializedObject<DataStruct.SignalParameters>? _SignalsParameters;

        /// <summary>
        /// Его дети хранят в себе подробную информацию о фильтрах
        /// </summary>
        private SerializedObject<DataStruct.FiltersObject>? _FilterDescription;

        /// <summary>
        /// Структура данных, хранящая в себе информацию о выбранных сигналах
        /// </summary>
        private DataStruct.Channel[]? selectedSignals;


        IStorageFolder? folder;
        string? folderPath;
        private IStorageFolder? saveFolder;
        [ObservableProperty]
        private string _SaveFolderPath = string.Empty;

        /// <summary>
        /// Класс библиотеки для конвертации в формат CSV
        /// </summary>
        CSVCommands csv;
        EDFCommands edf;


        public MainWindowViewModel()
        {
            ErrorMessages?.Clear();

            csv = new CSVCommands();
            edf = new EDFCommands();
        }

        [RelayCommand]
        public async Task OpenFileFolderAsync()
        {
            try
            {
                var _filesService = App.Current?.Services?.GetService<IFilesService>()
                    ?? throw new NullReferenceException(Languages.Resources.Missing_FS_instance_Exeption);

                folder = await _filesService.GetFolderAsync() 
                    ?? throw new NullReferenceException(Languages.Resources.get_FolderWithSignals_Exeption);


                folderPath = folder.TryGetLocalPath();
                // класс десериализации MethDescroption.xml. Хранит в себе же все данные о сигналах 
                _Channels = new SerializedObject<DataStruct.BOSMeth>(folderPath!, @"MethDescription.xml")
                    ?? throw new NullReferenceException(Languages.Resources.process_DesiarializationFailed_Exeption);

                /// 
                /// TODO: Локализацию для исключений при сериализации других объектов
                ///
                _SignalsParameters = new SerializedObject<DataStruct.SignalParameters>(folderPath!, @"SignalDescriptions.xml")
                    ?? throw new NullReferenceException(); /// ЛОКАЛИЗОВАТЬ

                _FilterDescription = new SerializedObject<DataStruct.FiltersObject>(folderPath!, @"FilterDescriptions.xml")
                    ?? throw new NullReferenceException(); /// ЛОКАЛИЗОВАТЬ

                ChannelNames = new ObservableCollection<string>();
                foreach (var channel in _Channels.Type!.Channels!)
                {
                    ChannelNames.Add(channel.SignalFileName!);
                }
            }
            catch (Exception ex)
            {
                ErrorMessages!.Add(ex.Message);
            }
        }


        [RelayCommand]
        public async Task SelectSaveFolderAsync()
        {
            try
            {
                var _filesService = App.Current?.Services?.GetService<IFilesService>()
                    ?? throw new NullReferenceException(Languages.Resources.Missing_FS_instance_Exeption);

                saveFolder = await _filesService.GetFolderAsync();
                if (saveFolder is null) return;

                SaveFolderPath = saveFolder.TryGetLocalPath()!;
            }
            catch (Exception ex)
            {
                ErrorMessages?.Add(ex.Message);
            }
        }


        public void Update_selectedSignalsNames(System.Collections.IList selectedItems, int itemsCount)
        {
            selectedSignals = new DataStruct.Channel[itemsCount];

            foreach (var channel in _Channels!.Type!.Channels)
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    if (channel.SignalFileName!.Equals(selectedItems[i]!.ToString()))
                        selectedSignals[i] = channel;
                }
            }
        }


        #region Convertion flags
        public byte CurrentConvertingFlag;

        private const byte ToCSV = 0;
        private const byte ToEDF = 1;
        #endregion


        #region Conversion commands (old)

        [RelayCommand]
        public void Initialization_Command()
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (CurrentConvertingFlag == ToCSV)
                csv.Initialization(selectedSignals, _SignalsParameters!.Type!, _FilterDescription!.Type!, folderPath!, SaveFolderPath,
                    SignalsLengthCalculator.Calculation(folderPath!, selectedSignals));
            else if (CurrentConvertingFlag == ToEDF)
            {
                edf.Initialization(selectedSignals, _SignalsParameters!.Type!, _FilterDescription!.Type!, folderPath!, SaveFolderPath, 
                    SignalsLengthCalculator.Calculation(folderPath!, selectedSignals));
            }
        }

        [RelayCommand]
        public void AddData_Command()
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;
            
            if (CurrentConvertingFlag == ToCSV)
                csv.AddData();
        }

        [RelayCommand]
        public void Finalization_Command()
        {
            if(selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (CurrentConvertingFlag == ToCSV)
                csv.Finalization();
        }

        [RelayCommand]
        public void Reset_Command()
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (CurrentConvertingFlag == ToCSV)
                csv.Reset();
        }

        #endregion


        #region Conversion commands (new)

        public void Initialization_Command(int _a)
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (CurrentConvertingFlag == ToCSV)
                csv.Initialization(selectedSignals, _SignalsParameters!.Type!, _FilterDescription!.Type!, folderPath!, SaveFolderPath,
                    SignalsLengthCalculator.Calculation(folderPath!, selectedSignals));
            else
            {
                // Здесь будет код конвертации в EDF
            }
        }

        public void AddData_Command(int _a)
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (CurrentConvertingFlag == ToCSV)
            {
                for (int j = 0, iteration = 0; j < csv.SignalLengths!.Average(); j += CSVCommands.ReadingInterval, iteration++)
                {
                    //if ()
                    csv.AddData(SignalsReader.ReadSomeDataFromFiles(j, CSVCommands.ReadingInterval, folderPath, selectedSignals).Result, iteration);
                }
            }
        }

        #endregion
    }
}