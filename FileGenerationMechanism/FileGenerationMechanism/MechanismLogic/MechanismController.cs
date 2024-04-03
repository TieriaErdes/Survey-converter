using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DataStruct;
using FileGenerationMechanism.MechanismRepozitory;

namespace FileGenerationMechanism.MechanismLogic
{
    public class MechanismController
    {
        #region flags of the selected conversion
        private const byte toCSV = 0;
        private const byte toEDF = 1;
        #endregion


        private Channel[] selectedSignals;
        private string saveFolderPath;
        private long[] signalLengths;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="channels"></param>
        public MechanismController(byte _control,Channel[] _selectadChannels, string _saveFolderPath, long[] _signalLengths)
        {
            try
            {
                selectedSignals = _selectadChannels;
                saveFolderPath = _saveFolderPath;
                signalLengths = _signalLengths;

                switch (_control)
                {
                    case toCSV:
                        ToCSVConversion();
                        break;
                    case toEDF:
                        ToEDFConversion();
                        break;
                    default:
                        return;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void ToCSVConversion()
        {
            CSVMechanismCommands commands = new CSVMechanismCommands(selectedSignals, saveFolderPath, signalLengths);
        }

        private void ToEDFConversion()
        {

        }
    }
}
