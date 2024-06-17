using DataStruct;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Survey_converter.Models
{
    public sealed class SignalsReader
    {
        /// <summary>
        /// Функция чтения массива double значений из одного файла
        /// </summary>
        public static async Task<double[]> ReadSomeDataFromSingleFileAsync(int iteration, int lengthOfInterval, string signalName)
        {
            try
            {
                //var stopWatch = Stopwatch.StartNew();

                double[] currentPartOfSignal = new double[lengthOfInterval];

                await Task.Run(() =>
                {
                    using (FileStream signalFile = new FileStream(signalName, FileMode.Open))
                    {
                        //Debug.WriteLine($"{signalFile.Length} {iteration } {lengthOfInterval * sizeof(double)}");

                        byte[] bytes = new byte[lengthOfInterval * sizeof(double)];

                        /// можно заменить это с помощью поля Position
                        /// signalFile.Position = iteration * sizeof(double) * lengthOfInterval; НЕ РАБОТАЕТ ПРИ ПОСЛЕДНЕЙ ИТЕРАЦИИ
                        // Пролистываю файл до нужного отрезка
                        for (int i = 0; i < iteration; i++)
                            signalFile.Read(bytes, 0, bytes.Length);

                        signalFile.Read(bytes, 0, bytes.Length);

                        for (int i = 0; i < lengthOfInterval; i++)
                            currentPartOfSignal[i] = BitConverter.ToDouble(bytes, i * sizeof(double));
                    }
                });
                //Debug.WriteLine($"{signalName} {stopWatch.Elapsed}");
                return currentPartOfSignal;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new double[0];
            }
        }

        public static async ValueTask<double[][]> ReadSomeDataFromFiles(int firstIndex, int lengthOfInterval, string? mainPath, Channel[] signals)
        {
            if (mainPath == null && signals == null)
                return new double[0][];

            int signalsLength = signals.Length;

            Task<double[]>[]? tasks = new Task<double[]>[signalsLength];
            for (int i = 0; i < signalsLength; i++) 
            {
                tasks[i] = ReadSomeDataFromSingleFileAsync(firstIndex, lengthOfInterval, Path.Combine(mainPath!, signals[i].SignalFileName!));
            }

            double[][] results = new double[signalsLength][];
            await Task.WhenAll(tasks);

            for (int i = 0;i < tasks.Length; i++)
            {
                results[i] = tasks[i].Result;
            }

            return results;
        }
    }
}
