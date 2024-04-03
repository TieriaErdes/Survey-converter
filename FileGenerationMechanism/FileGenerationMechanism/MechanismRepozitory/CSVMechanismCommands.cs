using DataStruct;
using FileGenerationMechanism.MechanismLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FileGenerationMechanism.MechanismRepozitory
{
    internal class CSVMechanismCommands : IMechanismCommands
    {
        public CSVMechanismCommands(in DataStruct.Channel[] _selectedChannels,in string _saveFolderPath, in long[] _signalLengths)
        {
            Initialization(_selectedChannels, _saveFolderPath, _signalLengths);
        }

        public void Initialization(in DataStruct.Channel[] _selectedChannels, in string _saveFolderPath, in long[] _signalLengths)
        {
            Debug.WriteLine("To CSV Initialization command request");

            foreach (var channel in _selectedChannels)
            {
                string fullPath = Path.Combine(_saveFolderPath, channel.SignalFileName!, ".csv");

                FileInfo file = new FileInfo(fullPath);

                File.AppendAllText(fullPath, string.Join($"\"{channel.SignalFileName} \", ",
                    $"\"{channel.UnicNumber.ToString()}\", ",
                    $"\"{channel.Type.ToString()}\", ",
                    $"\"{channel.EffectiveFd.ToString()}\", "));
            }
        }

        public void Reset()
        {

        }

        public void AddData()
        {

        }

        public void Finalization()
        {

        }
    }
}
