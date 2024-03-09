using Avalonia;
using CommunityToolkit.Mvvm.Input;
using Survey_converter.Services;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Survey_converter.Models;
using Avalonia.Platform.Storage;

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



        public MainWindowViewModel()
        {
            ErrorMessages?.Clear();
        }

        [RelayCommand]
        public async Task OpenFileFolderAsync()
        {
            try
            {
                var _filesService = App.Current?.Services?.GetService<IFilesService>()
                    ?? throw new NullReferenceException("Missing File Service instance.");

                var folder = await _filesService.GetFolderAsync();
                if (folder is null) return;

                // класс десериализации MethDescroption.xml. Хранит в себе же все данные о сигналах 
                _Channels = new SerializedChannel(folder.TryGetLocalPath());
                if (_Channels is null) return;

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
    }
}