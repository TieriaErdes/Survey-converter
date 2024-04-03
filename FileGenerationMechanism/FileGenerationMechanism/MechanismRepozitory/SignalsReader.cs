using DataStruct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey_converter.Models
{
    public sealed class SignalsReader
    {
        public static async Task<double[]> ReadSomeDataFromSingleFile(int firstIndex, int lengthOfInterval, string signalName)
        {
            try
            {
                //var stopWatch = Stopwatch.StartNew();

                double[] currentPartOfSignal = new double[lengthOfInterval];
                await Task.Run(() =>
                {
                    using (FileStream signalFile = new FileStream(signalName, FileMode.Open))
                    {
                        byte[] bytes = new byte[lengthOfInterval * sizeof(double)];
                        signalFile.Read(bytes, firstIndex, lengthOfInterval * sizeof(double));

                        for (int i = 0; i < lengthOfInterval; i++)
                            currentPartOfSignal[i] = BitConverter.ToDouble(bytes, i * sizeof(double));
                    }
                }
                );
                //Debug.WriteLine($"{signalName} {stopWatch.Elapsed}");
                return currentPartOfSignal;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new double[0];
            }
        }

        public static async ValueTask<double[]> ReadSomeDataFromFiles(int firstIndex, int lengthOfInterval, string? mainPath, Channel[] signals)
        {
            if (mainPath == null && signals == null)
                return new double[] { -1 };

            Task<double[]>[]? tasks = new Task<double[]>[signals.Length];
            for (int i = 0; i < signals.Length; i++) 
            {
                tasks[i] = ReadSomeDataFromSingleFile(firstIndex, lengthOfInterval, Path.Combine(mainPath!, signals[i].SignalFileName!));
            }

            double[] results = new double[signals.Length];
            await Task.WhenAll(tasks);

            for (int i = 0;i < tasks.Length; i++)
            {
                results = tasks[i].Result;
            }

            //foreach (var result in results)
            //    Debug.WriteLine(result.FirstOrDefault());

            return results;
        }

        public static long[] SignalsLengthCalculator(string mainPath, Channel[] signals)
        {
            string[] signalNames = new string[signals.Length];
            for (int i = 0; i  < signals.Length; i++)
                signalNames[i] = (Path.Combine(mainPath, signals[i].SignalFileName!));

            try
            {
                long[] results = new long[signals.Length];
                for (int i = 0; i < signals.Length; i++)
                    using (FileStream signalFile = new FileStream(signalNames[i], FileMode.Open))
                    {
                            results[i] = signalFile.Length;
                    }

                return results;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return new long[] { -1 };
            }
        }
    }
}
