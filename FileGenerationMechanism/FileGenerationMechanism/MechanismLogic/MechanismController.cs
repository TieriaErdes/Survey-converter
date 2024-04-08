using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DataStruct;
using FileGenerationMechanism.MechanismRepository;

namespace FileGenerationMechanism.MechanismLogic
{
    public class MechanismController
    {
        #region flags of the selected conversion
        private const byte toCSV = 0;
        private const byte toEDF = 1;
        #endregion

        /// <summary>
        ///  Конструктор конвертера сигналов, распределяющий какой вид конвертации выбран пользователем
        /// </summary>
        public MechanismController(byte _control,Channel[] _selectedChannels, string _mainPath, string _saveFolderPath, int[] _signalLengths)
        {
            try
            {
                switch (_control)
                {
                    case toCSV: // == 0
                        CSVMechanismCommands commands = new CSVMechanismCommands(_selectedChannels, _mainPath, _saveFolderPath, _signalLengths);
                        break;
                    case toEDF: // == 1
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

        private void ToEDFConversion()
        {

        }
    }
}
