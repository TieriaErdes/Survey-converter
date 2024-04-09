﻿using System;
using System.Diagnostics;
using System.IO;

namespace Survey_converter.Models
{
    internal sealed class SignalsLengthCalculator
    {
        public static int[] Calculation(string mainPath, DataStruct.Channel[] signals)
        {
            int signalsLength = signals.Length;
            string[] signalNames = new string[signalsLength];
            for (int i = 0; i < signalsLength; i++)
                signalNames[i] = (Path.Combine(mainPath, signals[i].SignalFileName!));

            try
            {
                int[] results = new int[signalsLength];
                for (int i = 0; i < signalsLength; i++)
                {
                    FileInfo file = new FileInfo(signalNames[i]);
                    results[i] = (int)file.Length;
                }

                return results;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return new int[] { -1 };
            }
        }
    }
}
