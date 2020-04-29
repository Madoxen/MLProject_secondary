using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    class Data
    {
        public static void Shuffle(double[] data)
        {
            Random rnd = new Random();
            int x = 0; int n = data.Length; double y = 0;
            for (int i = 0; i < n - 1; i++)
            {
                x = i + rnd.Next(n - i); y = data[x];
                data[x] = data[i]; data[i] = y;
            }
        }

        public static double[][] LoadIrises(string path)
        {
            string[] lines = File.ReadAllLines(path);
            double[][] data = new double[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');
                data[i] = new double[tmp.Length + 2];
                for (int j = 0; j < tmp.Length - 1; j++)
                    data[i][j] = Convert.ToDouble(tmp[j].Replace('.', ','));
                if (tmp[4] == "Iris-setosa") data[i][6] = 1;
                else if (tmp[4] == "Iris-versicolor") data[i][5] = 1;
                else if (tmp[4] == "Iris-virginica") data[i][4] = 1;
            }
            return data;
        }

        public static void ClassifyIris(double[] inputs, Network network)
        {
            network.PushInputValues(inputs);
            Console.Write("\n Wprowadzone dane: ");
            for (int i = 0; i < inputs.Length; i++) 
                Console.Write(inputs[i].ToString("0.0") + " ");

            List<double> outputs = network.GetOutput();
            Console.Write("\n Gatunek: ");
            double sum = 0; foreach (double i in outputs) sum += Math.Abs(i);
            if (outputs.Max() < 0.75 || sum > 1.5)
                Console.Write("nieokreślony");
            else
            {
                string[] names = { "Iris-virginica", "Iris-versicolor", "Iris-setosa" };
                Console.Write(names[outputs.IndexOf(outputs.Max())]);
            }
            Console.WriteLine();
        }
    }
}
