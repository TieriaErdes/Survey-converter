using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileGenerationMechanism.MechanismLogic
{
    internal interface IMechanismCommands
    {
        /// <summary>
        /// очищает описание сигналов и буферы данных
        /// </summary>
        public void Reset();

        /// <summary>
        /// получает на вход структуру, описывающую все выбранные сигналы и конечную папку, для сохранения файлов. Создаёт необходимые файлы и открывает их для записи.
        /// </summary>
        public void Initialization();

        /// <summary>
        /// позволяет добавить массив отсчётов одного сигнала
        /// </summary>
        public void AddData();

        /// <summary>
        /// сохранение информации о всех сигналах и закрытие файлов
        /// </summary>
        public void Finalization();
    }
}
