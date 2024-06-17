using FileGenerationMechanism.MechanismRepository;
using System.Diagnostics;

namespace FileGenerationMechanism.MechanismLogic
{
    public sealed class CSVCommands : IMechanismCommands
    {
        public const int ReadingInterval = 1000;

        public DataStruct.Channel[]? SelectedChannels;
        private DataStruct.SignalParameters? signalParameters;
        private DataStruct.FiltersObject? filtersObject;

        private string? mainPath;
        private string? saveFolderPath;
        public int[]? SignalLengths;
        private int numberOfSignals;

        private StreamWriter[]? streamWriter;

        public CSVCommands() { }
        public CSVCommands(DataStruct.Channel[] _selectedChannels, DataStruct.SignalParameters _signalParameters, DataStruct.FiltersObject _filtersObject, 
            string _mainPath, string _saveFolderPath, int[] _signalLengths)
        {
            Initialization(_selectedChannels, _signalParameters, _filtersObject, _mainPath, _saveFolderPath, _signalLengths);
        }

        #region Initialization command
        public void Initialization(DataStruct.Channel[] _selectedChannels, DataStruct.SignalParameters _signalParameters, DataStruct.FiltersObject _filtersObject, 
            string _mainPath, string _saveFolderPath, int[] _signalLengths)
        {
            Debug.WriteLine($"To CSV Initialization command request with {_saveFolderPath} path");

            SelectedChannels = _selectedChannels;
            mainPath = _mainPath;
            saveFolderPath = _saveFolderPath;
            SignalLengths = _signalLengths;

            numberOfSignals = SelectedChannels.Length;
            for (int i = 0; i < numberOfSignals; i++)
            {
                SignalLengths[i] /= sizeof(double);
            }

            streamWriter = new StreamWriter[numberOfSignals];
            for (int i = 0; i < numberOfSignals; i++)
            {
                string signalsDataFullPath = Path.Combine(saveFolderPath, string.Join(string.Empty, SelectedChannels[i].SignalFileName!, ".csv"));
                streamWriter[i] = File.CreateText(signalsDataFullPath);
            }
        }

        #endregion


        #region Add data command

        public async void AddData()
        {
            Task[] Tasks = new Task[numberOfSignals];
            for (int i = 0; i < SelectedChannels!.Length; i++)
            {
                Tasks[i] = WriteSignalsValuesAsync(i);
            }

            await Task.WhenAll(Tasks);
        }

        private async Task WriteSignalsValuesAsync(int _i)
        {
            await Task.Run(() =>
            {
                //var stopWatch = Stopwatch.StartNew();
                try
                {
                    streamWriter![_i].WriteLine(string.Join("; ",
                                                    // TODO: Исправить проблему кодировок (текст на русском превращается в иероглифы)
                                                    //$"\"{Localization.Resources.TimeText}\"",
                                                    //$"\"{Localization.Resources.ValueText}\"\n"));
                                                    $"\"Time\"", $"\"Value\""));

                    int index = 0;
                    // время отсчёта
                    float time = 0;

                    /// ЛУЧШЕ СДЕЛАТЬ ЭТО АСИНХРОННО, А НЕ ДЁРГАТЬ ТАСКУ МИЛЛИОН РАЗ
                    for (index = 0; index < SignalLengths![_i]; index += ReadingInterval)
                    {
                        if (index + ReadingInterval < SignalLengths[_i])
                        {
                            double[] result = SignalsReader.ReadSomeDataFromSingleFileAsync(index / ReadingInterval, ReadingInterval, Path.Combine(mainPath!, SelectedChannels[_i].SignalFileName!)).Result;

                            for (int j = 0; j < result.Length; j++, time++)
                                streamWriter[_i].WriteLine(string.Join("; ", time / SelectedChannels[_i].EffectiveFd, result[j]));
                        }
                        else
                        {
                            double[] result = SignalsReader.ReadSomeDataFromSingleFileAsync(index / ReadingInterval, SignalLengths[_i] - index, Path.Combine(mainPath!, SelectedChannels[_i].SignalFileName!)).Result;

                            for (int j = 0; j < result.Length; j++, time++)
                                streamWriter[_i].WriteLine(string.Join("; ", time / SelectedChannels[_i].EffectiveFd, result[j]));
                        }


                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                //Debug.WriteLine($"{selectedChannels![_i].SignalFileName} {stopWatch.Elapsed}");
            });
        }

        public float ConversionProgressMarker = 0;

        public async void AddData(double[][] _signalsValues, int _iteration)
        {
            if (_signalsValues.Length == SelectedChannels!.Length)
            {
                Debug.WriteLine("The dimension of the received data array is not equal to the number of selected signals");
                return;
            }

            Task[] Tasks = new Task[numberOfSignals];
            for (int i = 0; i < SelectedChannels!.Length; i++)
            {
                streamWriter![i].WriteLine(string.Join("; ",
                                                        // TODO: Исправить проблему кодировок (текст на русском превращается в иероглифы)
                                                        //$"\"{Localization.Resources.TimeText}\"",
                                                        //$"\"{Localization.Resources.ValueText}\"\n"));
                                                        $"\"Time\"", $"\"Value\""));
                Tasks[i] = WriteSignalsValuesAsync(i, _signalsValues[i], _iteration);
            }

            await Task.WhenAll(Tasks);

            ConversionProgressMarker += ReadingInterval / (float)SignalLengths!.Average();
        }

        private async Task WriteSignalsValuesAsync(int _i, double[] _signalValues, int _iteration)
        {
            await Task.Run(() =>
            {
                try
                {
                    int time = 0;
                    for (int j = 0; j < _signalValues.Length; j++, time++)
                    {
                        streamWriter![_i].WriteLine(string.Join("; ", time / SelectedChannels![_i].EffectiveFd, _signalValues[j]));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            });
        }

        #endregion


        #region Finalization command

        public async void Finalization()
        {
            Task[] tasks = new Task[numberOfSignals];
            for (int i = 0; i < numberOfSignals; i++)
            {
                tasks[i] = WriteSignalsCharacteristicsAsync(i);
            }

            for (int i = 0; i < SelectedChannels!.Length; i++)
            {
                streamWriter![i].Close();
            }

            await Task.WhenAll(tasks);
        }

        private async Task WriteSignalsCharacteristicsAsync(int i)
        {
            await Task.Run(() =>
            {
                string signalsStructFullPath = Path.Combine(saveFolderPath!, string.Join(string.Empty, SelectedChannels![i].SignalFileName!, ".txt"));
                File.AppendAllText(signalsStructFullPath, string.Join(", ",
                    $"{Localization.Resources.FileNameText}",
                    $"{Localization.Resources.UnicNumberText}",
                    $"{Localization.Resources.TypeText}",
                    $"{Localization.Resources.EffectiveFdText}",
                    $"\n{SelectedChannels[i].SignalFileName}",
                    $"\t{SelectedChannels[i].UnicNumber.ToString()}",
                    $"\t{SelectedChannels[i].Type.ToString()}",
                    $"\t{SelectedChannels[i].EffectiveFd.ToString()}\n"));
            });
        }

        #endregion


        public void Reset()
        {
            SelectedChannels = default;
            mainPath = default;
            saveFolderPath = default;
            SignalLengths = default;
            numberOfSignals = default;

            streamWriter = null;
        }
    }
}
