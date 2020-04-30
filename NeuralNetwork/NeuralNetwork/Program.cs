using System;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            double[][] data = Data.LoadIrises(@"IrisDatabase.txt");
            Network network = new Network(4, 2, 10, 3, "weights.txt");
            network.Train(data, 5000);

            //RESULTS OF TRAINING:
            for (int i = 0; i < data.Length; i++)
                Data.ClassifyIris(new double[] { data[i][0], data[i][1], data[i][2], data[i][3] }, network);

            Console.ReadKey();
        }
    }
}