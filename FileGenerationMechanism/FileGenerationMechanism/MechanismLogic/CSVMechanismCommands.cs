using FileGenerationMechanism.MechanismRepository;
using System.Diagnostics;

namespace FileGenerationMechanism.MechanismLogic
{
    public sealed class CSVMechanismCommands : IMechanismCommands
    {
        private const int readingInterval = 1000;

        private DataStruct.Channel[]? selectedChannels;
        private string? mainPath;
        private string? saveFolderPath;
        private int[]? signalLengths;
        private int numberOfSignals;

        private StreamWriter[]? streamWriter;

        public CSVMechanismCommands() { }
        public CSVMechanismCommands(in DataStruct.Channel[] _selectedChannels, in string _mainPath, in string _saveFolderPath, in int[] _signalLengths)
        {
            Initialization(_selectedChannels, _mainPath, _saveFolderPath, _signalLengths);
        }

        #region Initialization command
        public void Initialization(DataStruct.Channel[] _selectedChannels, string _mainPath, string _saveFolderPath, int[] _signalLengths)
        {
            Debug.WriteLine($"To CSV Initialization command request with {_saveFolderPath} path");

            selectedChannels = _selectedChannels;
            mainPath = _mainPath;
            saveFolderPath = _saveFolderPath;
            signalLengths = _signalLengths;

            numberOfSignals = selectedChannels.Length;
            for (int i = 0; i < numberOfSignals; i++)
            {
                signalLengths[i] /= sizeof(double);
            }

            streamWriter = new StreamWriter[numberOfSignals];
            for (int i = 0; i < numberOfSignals; i++)
            {
                string signalsDataFullPath = Path.Combine(saveFolderPath, string.Join(string.Empty, selectedChannels[i].SignalFileName!, ".csv"));
                streamWriter[i] = File.CreateText(signalsDataFullPath);
            }
        }

        #endregion


        #region Add data command

        public async void AddData()
        {
            Task[] Tasks = new Task[numberOfSignals];
            for (int i = 0; i < selectedChannels!.Length; i++)
            {
                Tasks[i] = WriteSignalsValuesAsync(i);
            }

            await Task.WhenAll(Tasks);
        }

        private async Task WriteSignalsValuesAsync(int i)
        {
            await Task.Run(async () =>
            {
                //var stopWatch = Stopwatch.StartNew();
                try
                {

                    streamWriter![i].WriteLine(string.Join("; ",
                                                    // TODO: Исправить проблему кодировок (текст на русском превращается в иероглифы)
                                                    //$"\"{Localization.Resources.TimeText}\"",
                                                    //$"\"{Localization.Resources.ValueText}\"\n"));
                                                    $"\"Time\"", $"\"Value\""));

                    int index = 0;
                    // время отсчёта
                    float time = 0;
                    for (index = 0; index < signalLengths![i]; index += readingInterval)
                    {
                        if (index + readingInterval < signalLengths[i])
                        {
                            double[] result = SignalsReader.ReadSomeDataFromSingleFileAsync(index / readingInterval, readingInterval, Path.Combine(mainPath!, selectedChannels[i].SignalFileName!)).Result;

                            for (int j = 0; j < result.Length; j++, time++)
                                streamWriter[i].WriteLine(string.Join("; ", time / selectedChannels[i].EffectiveFd, result[j]));
                        }
                        else
                        {
                            double[] result = SignalsReader.ReadSomeDataFromSingleFileAsync(index / readingInterval, signalLengths[i] - index, Path.Combine(mainPath!, selectedChannels[i].SignalFileName!)).Result;

                            for (int j = 0; j < result.Length; j++, time++)
                                streamWriter[i].WriteLine(string.Join("; ", time / selectedChannels[i].EffectiveFd, result[j]));
                        }


                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                //Debug.WriteLine($"{selectedChannels![i].SignalFileName} {stopWatch.Elapsed}");
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

            for (int i = 0; i < selectedChannels!.Length; i++)
            {
                streamWriter![i].Close();
            }

            await Task.WhenAll(tasks);
        }

        private async Task WriteSignalsCharacteristicsAsync(int i)
        {
            await Task.Run(() =>
            {
                string signalsStructFullPath = Path.Combine(saveFolderPath!, string.Join(string.Empty, selectedChannels![i].SignalFileName!, ".txt"));
                File.AppendAllText(signalsStructFullPath, string.Join(", ",
                    $"{Localization.Resources.FileNameText}",
                    $"{Localization.Resources.UnicNumberText}",
                    $"{Localization.Resources.TypeText}",
                    $"{Localization.Resources.EffectiveFdText}",
                    $"\n{selectedChannels[i].SignalFileName}",
                    $"\t{selectedChannels[i].UnicNumber.ToString()}",
                    $"\t{selectedChannels[i].Type.ToString()}",
                    $"\t{selectedChannels[i].EffectiveFd.ToString()}\n"));
            });
        }

        #endregion


        public void Reset()
        {
            selectedChannels = default;
            mainPath = default;
            saveFolderPath = default;
            signalLengths = default;
            numberOfSignals = default;

            streamWriter = null;
        }
    }
}
