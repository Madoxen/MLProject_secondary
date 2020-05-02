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
            int imageWidth = 100;
            int imageHeight = 50;
            int imageDepth = 3; //number of colors

            int outputCount = 3; // we need to know this in advance to avoid back tracking through images


            Network net = new Network(imageWidth * imageHeight * imageDepth, 2, 50, outputCount);
         //   net.LoadWeights(File.ReadAllLines("weights.txt"));
            net.testStrategy = new HighestHitTest(net);

            Console.WriteLine("Loading data...");
            double[][][] finalData = Loader.Load("Resources", outputCount, imageWidth, imageHeight);


            //net.RandomizeWeights();
            net.Train(finalData, 100);
 
            net.PushInputValues(finalData[2][1]);

            //smol test
            List<double> o = net.GetOutput();
            Console.WriteLine("Expected:");
            foreach (double d in finalData[3][1])
            {
                Console.Write(d + " | ");
            }
            Console.WriteLine("Got:");
            foreach (double d in o)
            {
                Console.Write(d + " | ");
            }

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
            Console.WriteLine($" Correct ones: {correct}/{finalData[0].Length + finalData[2].Length}");
        }
    }
}