using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataStruct
{
    [XmlRoot("SignalParameters")]
    public sealed class SignalParameters
    {
        [XmlArray("SignalsList")]
        [XmlArrayItem("Signal", typeof(Signal))]
        public Signal[]? Signals { get; set; }

        public SignalParameters() { }

        public SignalParameters(Signal[] signals)
        {
            Signals = signals;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Signal
    {
        [XmlAttribute(AttributeName = "ID")]
        public int ID { get; init; }
        [XmlAttribute(AttributeName = "PhysicalMAX")]
        public int PhysicalMAX { get; init; }
        [XmlAttribute(AttributeName = "PhysicalMIN")]
        public int PhysicalMIN { get; init; }

        [XmlElement(ElementName = "SignalName")]
        public SignalName SignalName { get; init; }

        [XmlElement(ElementName = "Caption")]
        public Caption Caption { get; init; }

        [XmlArray(ElementName = "Filter")]
        public Filter[] FiltersList { get; init; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SignalName
    {
        [XmlAttribute(AttributeName = "ru")]
        public string ru { get; init; }

        [XmlAttribute(AttributeName = "en")]
        public string en { get; init; }

        [XmlAttribute(AttributeName = "pl")]
        public string pl { get; init; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Caption
    {
        [XmlAttribute(AttributeName = "ru")]
        public string ru { get; init; }

        [XmlAttribute(AttributeName = "en")]
        public string en { get; init; }

        [XmlAttribute(AttributeName = "pl")]
        public string pl { get; init; }
    }

    [XmlRoot("Filters")]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct FiltersList
    {
        [XmlElement(ElementName = "Filter")]
        public int Filter {  get; init; }
    }
}
