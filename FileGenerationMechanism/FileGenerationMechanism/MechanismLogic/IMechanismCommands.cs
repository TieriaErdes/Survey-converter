namespace FileGenerationMechanism.MechanismLogic
{
    internal interface IMechanismCommands
    { 
        /// <summary>
        /// получает на вход структуру, описывающую все выбранные сигналы и конечную папку, для сохранения файлов. Создаёт необходимые файлы и открывает их для записи.
        /// </summary>
        public void Initialization(DataStruct.Channel[] _selectedChannels, string _mainPath, string _saveFolderPath, int[] _signalLengths);

        /// <summary>
        /// позволяет добавить массив отсчётов одного сигнала
        /// </summary>
        public void AddData();

        /// <summary>
        /// сохранение информации о всех сигналах и закрытие файлов
        /// </summary>
        public void Finalization();

        /// <summary>
        /// очищает описание сигналов и буферы данных
        /// </summary>
        public void Reset();
    }
}
