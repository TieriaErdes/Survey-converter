using DataStruct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileGenerationMechanism.MechanismLogic
{
    internal class CSVMechanismCommands : IMechanismCommands
    {
        public CSVMechanismCommands(Channel[] selectedChannels)
        {
            Initialization(selectedChannels);
        }

        public void Initialization(in Channel[] selectedChannels)
        {
            Debug.WriteLine("Initialization");
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
