using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace DataStruct
{
    [XmlRoot(ElementName = "BOSMeth", IsNullable = false)]
    public sealed class BOSMeth
    {
        [XmlElement(ElementName = "TemplateGUID")]
        public Guid TemplateGUID { get; init; }

        [XmlArray(ElementName = "Channels", IsNullable = false)]
        public Channel[] Channels;

        public BOSMeth() { }
        public BOSMeth(Guid templateGuid, Channel[] channels)
        {
            TemplateGUID = templateGuid;
            Channels = channels;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Channel
    {
        [XmlAttribute(AttributeName = "UnicNumber")]
        public int UnicNumber { get; init; }
        [XmlAttribute(AttributeName = "SignalFileName")]
        public string? SignalFileName { get; init; }
        [XmlAttribute(AttributeName = "Type")]
        public int Type { get; init; }
        [XmlAttribute(AttributeName = "EffectiveFd")]
        public int EffectiveFd { get; init; }

        //public Channel() { }
        //public Channel(int unicNumber, string signalFileName, int type, int effectiveFd)
        //{
        //    UnicNumber = unicNumber;
        //    SignalFileName = signalFileName;
        //    Type = type;
        //    EffectiveFd = effectiveFd;
        //}
    }
}