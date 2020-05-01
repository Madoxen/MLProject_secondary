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



            Network net = new Network(imageWidth * imageHeight * imageDepth, 2, 1000, outputCount);
            double[][][] finalData =  Loader.Load("Resources",outputCount,imageWidth,imageHeight);

            net.Train(finalData, 10);
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
        }
    }
}