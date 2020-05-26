using System;
using System.IO;
using NeuralNetwork;
using DataPreparer;
using System.Collections.Generic;

namespace MovieClassifierClient
{
    class Program
    {
        /// <summary>
        /// Args: 
        /// 0 - image path
        /// 1 (optional, default: model.txt) - explicit model path
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            //Argument load stuff

            if (!File.Exists(args[0]))
                throw new ArgumentException("Provided image path is not valid");

            string imagePath = args[0];
            string modelPath = "model.txt";
            string labelPath = "labels.txt";

            if (args.Length > 1)
            {
                if (File.Exists(args[1]))
                {
                    modelPath = args[1];
                }
                else
                {
                    throw new ArgumentException("Provided model path is not valid");
                }
            }

            if (args.Length > 2)
            {
                if (File.Exists(args[2]))
                {
                    labelPath = args[2];
                }
                else
                {
                    throw new ArgumentException("Provided model path is not valid");
                }
            }



            string[] labels = File.ReadAllLines(labelPath);



            //We assume that we use depth 3 images (RGB)
            Network net = Network.LoadNetworkFromFile(modelPath);
            double[] imageData = LoadImage(imagePath, 50, 25);
            net.PushInputValues(imageData);
            List<double> output = net.GetOutput();
            int predictedIndex = output.MaxAt();

            Console.WriteLine("Predicted movie: " + labels[predictedIndex] + " with " + output[predictedIndex] + "% positiveness");



        }

        private static double[] LoadImage(string path, int targetWidth, int targetHeight)
        {
            double[] data = new double[targetWidth * targetHeight * 3];
            DataPreparer.ImageDataPreparer.LoadImage(path, targetWidth, targetHeight).CopyTo(data, 0);
            return data;
        }



    }
}
