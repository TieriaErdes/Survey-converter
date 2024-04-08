using CommunityToolkit.Mvvm.Input;
using Survey_converter.Services;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Survey_converter.Models;
using Avalonia.Platform.Storage;
using FileGenerationMechanism.MechanismLogic;
using FileGenerationMechanism;
using DataStruct;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Diagnostics;
using FileGenerationMechanism.MechanismRepository;

namespace Survey_converter.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";


        private ObservableCollection<string>? _ChannelNames;
        /// <summary>
        /// Реализует в себе динамическое хранение списка строк названий файлов сигналов.
        /// Значения строк получают Item в SignalsPopupListBox (MainWindow)
        /// </summary>
        public ObservableCollection<string>? ChannelNames
        {
            get { return _ChannelNames; }
            set { SetProperty(ref _ChannelNames, value); }
        }

        /// <summary>
        /// Его дети хранят в себе информацию о массиве сигналов (их конфигурации)
        /// </summary>
        private SerializedChannel? _Channels;

        /// <summary>
        /// Структура данных, хранящая в себе информацию о выбранных сигналах
        /// </summary>
        private DataStruct.Channel[] selectedSignals;


        IStorageFolder folder;
        string? folderPath;


        private IStorageFolder? saveFolder;

        CSVMechanismCommands csv;


        [ObservableProperty]
        private string _SaveFolderPath = string.Empty;

        public MainWindowViewModel()
        {
            ErrorMessages?.Clear();

            csv = new CSVMechanismCommands();
        }

        [RelayCommand]
        public async Task OpenFileFolderAsync()
        {
            try
            {
                var _filesService = App.Current?.Services?.GetService<IFilesService>()
                    ?? throw new NullReferenceException("Missing File Service instance.");

                folder = await _filesService.GetFolderAsync();
                if (folder is null) return;


                folderPath = folder.TryGetLocalPath();
                // класс десериализации MethDescroption.xml. Хранит в себе же все данные о сигналах 
                _Channels = new SerializedChannel(folderPath!);
                if (_Channels.bosMeth is null) return;

                ChannelNames = new ObservableCollection<string>();
                foreach (var channel in _Channels.bosMeth!.Channels!)
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
                    ?? throw new NullReferenceException("Missing File Service instance.");

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
            //Debug.WriteLine("Call successful");

            selectedSignals = new DataStruct.Channel[itemsCount];

            foreach (var channel in _Channels!.bosMeth!.Channels)
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    if (channel.SignalFileName!.Equals(selectedItems[i]!.ToString()))
                        selectedSignals[i] = channel;
                }
            }
        }


        #region Convertion flags
        public byte ActiveConvertingFlag;

        private const byte ToCSV = 0;
        private const byte ToEDF = 1;
        #endregion


        #region Conversion commands

        [RelayCommand]
        public void Initialization_Command()
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (ActiveConvertingFlag == ToCSV)
                csv.Initialization(selectedSignals, folderPath!, SaveFolderPath,
                    SignalsLengthCalculator.Calculation(folderPath!, selectedSignals));
            else
            {
                // Здесь будет код конвертации в EDF
            }
        }

        [RelayCommand]
        public void AddData_Command()
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;
            
            if (ActiveConvertingFlag == ToCSV)
                csv.AddData();
        }

        [RelayCommand]
        public void Finalization_Command()
        {
            if(selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (ActiveConvertingFlag == ToCSV)
                csv.Finalization();
        }

        [RelayCommand]
        public void Reset_Command()
        {
            if (selectedSignals == null || selectedSignals.Length == 0)
                return;

            if (ActiveConvertingFlag == ToCSV)
                csv.Reset();
        }

        #endregion
    }
}