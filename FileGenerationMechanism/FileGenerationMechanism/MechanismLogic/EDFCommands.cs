using DataStruct;
using FileGenerationMechanism.MechanismRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileGenerationMechanism.MechanismLogic
{
    public class EDFCommands : IMechanismCommands
    {
        private DataStruct.Channel[]? selectedChannels;
        private DataStruct.SignalParameters? signalParameters;
        private DataStruct.FiltersObject? filtersObject;

        private string? mainPath;
        private string? saveFolderPath;
        private int[]? signalLengths;
        private int numberOfSignals;

        private StreamWriter streamWriter;

        private EDF_FirstDescription fDescription;
        private EDF_SecondDescription[] secondDescriptions;

        public EDFCommands()
        {

        }

        public void Initialization(DataStruct.Channel[] _selectedChannels, DataStruct.SignalParameters _signalParameters, DataStruct.FiltersObject _filtersObject, 
            string _mainPath, string _saveFolderPath, int[] _signalLengths)
        {
            Debug.WriteLine($"To EDF Initialization command request with {_saveFolderPath} path");

            // Initialization local values
            selectedChannels = _selectedChannels;
            signalParameters = _signalParameters;
            filtersObject = _filtersObject;

            mainPath = _mainPath;
            saveFolderPath = _saveFolderPath;
            signalLengths = _signalLengths;

            numberOfSignals = selectedChannels.Length;
            for (int i = 0; i < numberOfSignals; i++)
            {
                signalLengths[i] /= sizeof(double);
            }

            string signalsDataFullPath = Path.Combine(saveFolderPath, "signals.edf");
            streamWriter = File.CreateText(signalsDataFullPath);

            AddDescriptionsInformation();
        }

        private void AddDescriptionsInformation()
        { 
            DateTime _now = DateTime.Now;

            fDescription = new EDF_FirstDescription();

            fDescription.Version = StringsBuilder("0", 8);
            fDescription.PatientIdentification = StringsBuilder("XX_XX_XX_XXXXXX_XXXXXX_ F 10-FEB-1980 XX", 80);
            fDescription.LocalRecordingIdentification = StringsBuilder("Startdate 22-JUL-2017 1.D3WMNSEm_EO EEG tech SN:007840", 80);
            fDescription.StartDateOfRecording = StringsBuilder(_now.ToString("d"), 8);
            fDescription.StartTimeOfRecording = StringsBuilder(_now.ToString("T"), 8);
            fDescription.NumberOfBytesInHeader = StringsBuilder((256 * (selectedChannels!.Length + 1)).ToString(), 8);
            fDescription.NumberOfSignalsData = StringsBuilder(selectedChannels!.Length.ToString(), 4);


            // Second description
            secondDescriptions = new EDF_SecondDescription[selectedChannels.Length];
            for (int i = 0; i < selectedChannels.Length; i++)
            {
                ///
                /// TODO: дописать
                /// 
            }

            // Получение размера структуры
            //int structureSize = Marshal.SizeOf(typeof(EDF_FirstDescription));
            //
            // Преобразование структуры в строку
            //string structureAsString = ConvertStructureToAsciiString(fDescription, structureSize);
            string str = string.Concat(fDescription.Version,
                fDescription.PatientIdentification,
                fDescription.LocalRecordingIdentification,
                fDescription.StartDateOfRecording,
                fDescription.StartTimeOfRecording,
                fDescription.NotUsed,
                fDescription.NumberOfBytesInHeader,
                fDescription.NumberOfDataRecords);

            Debug.WriteLine(str);

            //string _str = fDescription.Version.ToString() + (string)fDescription.PatientIdentification + secondDescriptions.ToString();

            streamWriter.Write(str);

            streamWriter.Close();
        }

        static string StringsBuilder(string _str, int _length)
        {
            return _str.Length < _length ?
                string.Concat(_str, new string(' ', _length - _str.Length))
                : _str;
        }


        public void AddData()
        {

        }

        public void Reset()
        {

        }

        public void Finalization()
        {

        }
    }
}
