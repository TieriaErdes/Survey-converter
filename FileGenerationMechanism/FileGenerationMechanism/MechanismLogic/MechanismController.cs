using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DataStruct;

namespace FileGenerationMechanism.MechanismLogic
{
    public class MechanismController
    {
        #region flags of the selected conversion
        private const byte toCSV = 0;
        private const byte toEDF = 1;
        #endregion


        private Channel[] selectedSignals;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="channels"></param>
        public MechanismController(byte control,Channel[] channels, string SaveFolderPath)
        { 
            switch (control)
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

        private void ToCSVConversion()
        {
            CSVMechanismCommands commands = new CSVMechanismCommands(selectedSignals);
            
        }

        private void ToEDFConversion()
        {

        }
    }
}
