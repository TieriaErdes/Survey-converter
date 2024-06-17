using DataStruct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Survey_converter.Models
{
    internal sealed class SerializedObject<SerializedType>
    {
        /// <summary>
        /// Текущий сериализуемый тип
        /// </summary>
        public SerializedType? Type;

        /// <summary>
        /// полный путь к обрабатываемым файлам
        /// </summary>
        public string? DirectoryPath;

        private string fileName;

        /// <summary>
        /// Constructor for deserialize data from xml file
        /// </summary>
        public SerializedObject(string path, string _fileName)
        {
            fileName = _fileName;

            DirectoryPath = Path.Join(path, Path.DirectorySeparatorChar.ToString());
            DeserializeData();
        }

        // Десериализатор 
        private void DeserializeData()
        {
            try
            {
                XmlSerializer _formatter = new XmlSerializer(typeof(SerializedType));

                using (FileStream fileStream = new FileStream(Path.Combine(DirectoryPath!, fileName), FileMode.Open))
                {
                    Type = (SerializedType?)(_formatter.Deserialize(fileStream) ?? throw new Exception("Deserializing xml file is failed"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Deserializing failed. Cause: {e.Message}");
            }
        }

    }
}
