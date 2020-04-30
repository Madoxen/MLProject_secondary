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
            int x = 0, n = data.Length; double y = 0;
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
            Console.Write("\n Input data: ");
            for (int i = 0; i < inputs.Length; i++) 
                Console.Write(inputs[i].ToString("0.0") + " ");

            List<double> outputs = network.GetOutput();
            string[] names = { "Iris-virginica", "Iris-versicolor", "Iris-setosa" };
            Console.WriteLine($"\n Species: {names[outputs.IndexOf(outputs.Max())]}");
        }

        public static double[][][] PrepareIrises(double[][] data)
        {
            // Dividing data into Training Set and Testing Set
            List<double[]> trainingset_list = new List<double[]>();
            List<double[]> testingset_list = new List<double[]>();
            for (int i = 0; i < data.Length; i++)
            {
                if (i % 5 == 0) testingset_list.Add(data[i]);
                else trainingset_list.Add(data[i]);
            }
            double[][] TrainingSet = trainingset_list.ToArray();
            double[][] TestingSet = testingset_list.ToArray();

            // Shuffling Training Set
            double[] numbers = new double[TrainingSet.Length];
            for (int i = 0; i < TrainingSet.Length; i++) 
                numbers[i] = i;
            Shuffle(numbers);
            double[][] tmpdata = new double[TrainingSet.Length][];
            for (int i = 0; i < numbers.Length; i++)
                tmpdata[i] = TrainingSet[(int)numbers[i]];
            TrainingSet = tmpdata;

            // Dividing sets into Inputs and Expected Outputs
            double[][] TrainingDataInput = new double[TrainingSet.Length][];
            double[][] TrainingDataOutput = new double[TrainingSet.Length][];
            for (int i = 0; i < TrainingSet.Length; i++)
            {
                TrainingDataInput[i] = new double[4]; 
                TrainingDataOutput[i] = new double[3];
                for (int j = 0; j < 4; j++) 
                    TrainingDataInput[i][j] = TrainingSet[i][j];
                for (int j = 4; j < 7; j++) 
                    TrainingDataOutput[i][j - 4] = TrainingSet[i][j];
            }
            double[][] TestingDataInput = new double[TestingSet.Length][];
            double[][] TestingDataOutput = new double[TestingSet.Length][];
            for (int i = 0; i < TestingSet.Length; i++)
            {
                TestingDataInput[i] = new double[4];
                TestingDataOutput[i] = new double[3];
                for (int j = 0; j < 4; j++)
                    TestingDataInput[i][j] = TestingSet[i][j];
                for (int j = 4; j < 7; j++)
                    TestingDataOutput[i][j - 4] = TestingSet[i][j];
            }

            return new double[][][] { TrainingDataInput, TrainingDataOutput, TestingDataInput, TestingDataOutput };
        }
    }
}