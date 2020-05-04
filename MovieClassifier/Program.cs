using System;
using System.Collections.Generic;
using System.IO;
using DataPreparer;
using NeuralNetwork;
using System.Linq;

namespace MovieClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tensor dimensions
            int imageWidth = 50;
            int imageHeight = 25;
            int imageDepth = 3; //number of colors

            int outputCount = 3; // we need to know this in advance to avoid back tracking through images

            // args[0] - Learning Rate
            // args[1] - Alpha in Bipolar Linear Function
            // args[2] - Minimum Init Weight
            // args[3] - Maximum Init Weight
            // args[4+] - Hidden Neurons
            int[] hiddenNeurons = new int[args.Length - 4];
            for (int i = 4; i < args.Length; i++) hiddenNeurons[i - 4] = Convert.ToInt32(args[i]);

            Network net = new Network(double.Parse(args[0].Replace(".",",")), double.Parse(args[1].Replace(".",",")), 
                double.Parse(args[2].Replace(".",",")), double.Parse(args[3].Replace(".",",")), 
                imageWidth * imageHeight * imageDepth, hiddenNeurons, outputCount);
            //net.LoadWeights(File.ReadAllLines("weights.txt"));
            net.testStrategy = new MeanErrorTest(net);

            Console.WriteLine(" Loading data...");
            double[][][] finalData = Loader.Load("Resources", outputCount, imageWidth, imageHeight);

            //net.RandomizeWeights();
            ClassifyMovies(finalData, net);
            net.Train(finalData, 100);
            ClassifyMovies(finalData, net);
        }

        public static void ClassifyMovies(double[][][] finalData, Network network)
        {
            List<double> outputs; int correct = 0;
            for (int i = 0; i < finalData[2].Length; i++)
            {
                network.PushInputValues(finalData[2][i]);
                outputs = network.GetOutput();
                if (outputs.IndexOf(outputs.Max()) == finalData[3][i].ToList().IndexOf(1)) correct += 1;
            }
            Console.WriteLine($" Correct ones: {correct}/{finalData[2].Length} ");
        }
    }
}