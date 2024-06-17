using System.Runtime.InteropServices;

namespace FileGenerationMechanism.MechanismRepository
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 256)]
    internal struct EDF_FirstDescription
    {
        /// <summary>
        /// Версия формата данных (0 для обычного EDF). Длинна 8 байт
        /// </summary>
        public string Version;

        /// <summary>
        /// Идентификация пациента. Длинна 80 байт
        /// </summary>
        public string PatientIdentification;

        /// <summary>
        /// Идентификация локальной записи. Длинна 80 байт
        /// </summary>
        public string LocalRecordingIdentification;

        /// <summary>
        /// Дата старта записи. Длинна 8 байт
        /// </summary>
        public string StartDateOfRecording;

        /// <summary>
        /// Время начала записи. Длинна 8 байт
        /// </summary>
        public string StartTimeOfRecording;

        /// <summary>
        /// Количество байт в заголовке (основной + дополнительный). Длинна 8 байт
        /// </summary>
        public string NumberOfBytesInHeader;

        /// <summary>
        /// Не используется в стандартной спецификации EDF. Длинна 44 байта
        /// </summary>
        public string NotUsed;

        /// <summary>
        /// Количество записей сигналов. Длинна 8 байт.
        /// </summary>
        public long NumberOfDataRecords;


        /// <summary>
        /// Длительность каждого сигнала (в секундах). Длинна 8 байт
        /// </summary>
        public string DurationOfADataRecord;

        /// <summary>
        /// Количество записываемых сигналов. Длинна 4 байт
        /// </summary>
        public string NumberOfSignalsData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 256)]
    internal struct EDF_SecondDescription
    {
        /// <summary>
        /// Метка для сигнала. Длинна 16 байт
        /// </summary>
        public string LabelForTheSignal;

        /// <summary>
        /// Тип преобразователя. Длинна 80 байт
        /// </summary>
        public string TransducerType;
        /// <summary>
        /// Единица физического измерения. Длинна 8 байт.
        /// </summary>
        public string Units;

        /// <summary>
        /// Минимальное физическое значение в единице измерения. Длинна 8 байт
        /// </summary>
        public string MinPossibleValue;

        /// <summary>
        /// Максимальное физическое значение в единице измерения. Длинна 8 байт
        /// </summary>
        public string MaxPossibleValue;

        /// <summary>
        /// Цифровой минимум. Длинна 8 байт
        /// </summary>
        public string MinValueNumerically;

        /// <summary>
        /// Цифровой максимум. Длинна 8 байт
        /// </summary>
        public string MaxValueNumerically;

        /// <summary>
        /// Предварительная фильтрация. Длинна 80 байт
        /// </summary>
        public string Prefiltering;

        /// <summary>
        /// Количество выборок в каждой записи. Длинна 8 байт
        /// </summary>
        public long SamplesInDataRecord;

        /// <summary>
        /// Зарезервировано. Длинна 32 байта
        /// </summary>
        public string reserved;
    }
}
