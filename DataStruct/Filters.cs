using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataStruct
{
    [XmlRoot("Filters")]
    public sealed class FiltersObject
    {
        [XmlArray(ElementName = "Filter")]
        public Filter[] Filters { get; init; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Filter
    {
        [XmlAttribute(AttributeName = "Id")]
        public int Id { get; init; }

        [XmlAttribute(AttributeName ="Enabled")]
        public bool Enabled { get; init; }

        [XmlAttribute(AttributeName = "Type")]
        public int Type { get; init; }

        [XmlAttribute(AttributeName = "PassType")]
        public int PassType { get; init; }

        [XmlAttribute(AttributeName = "Fd")]
        public int Fd { get; init; }

        [XmlAttribute(AttributeName = "Freq_mHz")]
        public int Freq_mHz { get; init; }

        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; init; }

        [XmlElement(ElementName = "Caption")]
        public Caption Caption { get; init; }

        [XmlElement(ElementName = "Description")]
        public Description Description { get; init; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Description
    {
        [XmlAttribute(AttributeName = "ru")]
        public string ru { get; init; }

        [XmlAttribute(AttributeName = "en")]
        public string en { get; init; }

        [XmlAttribute(AttributeName = "pl")]
        public string pl { get; init; }
    }
}
