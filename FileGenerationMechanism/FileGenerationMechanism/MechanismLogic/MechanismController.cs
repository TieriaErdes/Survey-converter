using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileGenerationMechanism.MechanismLogic
{
    public class MechanismController
    {
        #region flags of the selected conversion
        private const byte toCSV = 0;
        private const byte toEDF = 1;
        #endregion

        #region Data structure
        [StructLayout(LayoutKind.Sequential)]
        public class Channel
        {
            [XmlAttribute(AttributeName = "UnicNumber")]
            public int UnicNumber { get; init; }
            [XmlAttribute(AttributeName = "SignalFileName")]
            public string? SignalFileName { get; init; }
            [XmlAttribute(AttributeName = "Type")]
            public int Type { get; init; }
            [XmlAttribute(AttributeName = "EffectiveFd")]
            public int EffectiveFd { get; init; }

            public Channel(int unicNumber, string signalFileName, int type, int effectiveFd)
            {
                UnicNumber = unicNumber;
                SignalFileName = signalFileName;
                Type = type;
                EffectiveFd = effectiveFd;
            }
        }
        #endregion

        private Channel[] selectedSignals;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="channels"></param>
        public MechanismController(byte control,object channels) 
        {
            //selectedSignals = (Channel[])channels;
            var res = channels.GetType;
            var undf = channels as Channel[];

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
            //Debug.WriteLine($"{selectedSignals[0].SignalFileName}, {selectedSignals[0].EffectiveFd}");
        }

        private void ToEDFConversion()
        {

        }
    }
}
