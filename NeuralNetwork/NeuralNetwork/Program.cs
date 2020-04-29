using System;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Preparing Data
            double[][] data = Data.LoadIrises(@"IrisDatabase.txt");
            double[] numbers = new double[data.Length];
            for (int i = 0; i < data.Length; i++) numbers[i] = i;
            Data.Shuffle(numbers);
            double[][] tmpdata = new double[data.Length][];
            for (int i = 0; i < numbers.Length; i++)
                tmpdata[i] = data[(int)numbers[i]];
            data = tmpdata;

            double[][] expectedvalues = new double[data.Length][];
            double[][] trainingdata = new double[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                trainingdata[i] = new double[4]; expectedvalues[i] = new double[3];
                for (int j = 0; j < 4; j++) trainingdata[i][j] = data[i][j];
                for (int j = 4; j < 7; j++) expectedvalues[i][j - 4] = data[i][j];
            }
            #endregion

            Network network = new Network(4, 3, 15, 3, "weights.txt");
            network.PushExpectedValues(expectedvalues);

            /*
            network.PushInputValues(new double[] { 5.1, 3.5, 1.4, 0.2 }); var outputs = network.GetOutput();
            for (int i = 0; i < outputs.Count; i++) Console.Write(outputs[i] + " "); Console.WriteLine();
            */

            network.Train(trainingdata, 0.00001);

            // TESTING:
            double[][] dataagain = Data.LoadIrises(@"IrisDatabase.txt"); 
            double[][] importantdata = new double[dataagain.Length][];
            for (int i = 0; i < dataagain.Length; i++) 
                importantdata[i] = new double[] { dataagain[i][0], dataagain[i][1], dataagain[i][2], dataagain[i][3] };
            for (int i = 0; i < importantdata.Length; i++) 
                Data.ClassifyIris(importantdata[i], network);

            /*
            network.PushInputValues(new double[] { 5.1, 3.5, 1.4, 0.2 }); outputs = network.GetOutput();
            for (int i = 0; i < outputs.Count; i++) Console.Write(outputs[i] + " "); Console.WriteLine();
            */

            Console.ReadKey();
        }
    }
}