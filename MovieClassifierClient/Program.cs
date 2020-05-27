using System;
using System.IO;
using NeuralNetwork;
using DataPreparer;
using System.Linq;
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
            List<Record> records = new List<Record>();
            for(int i = 0; i < output.Count; i++)
            {
                Record r = new Record();
                r.label = labels[i];
                r.val = output[i];
                records.Add(r);
            }



            records = records.OrderByDescending(x=>x.val).ToList();
            for(int i = 0; i < 2; i++)
            {
                Console.WriteLine(" Predicted movie: " + records[i].label + " with " + Math.Round(records[i].val * 100, 5) + "% positiveness");
            }
            
        }

        private static double[] LoadImage(string path, int targetWidth, int targetHeight)
        {
            double[] data = new double[targetWidth * targetHeight * 3];
            byte[] raw = DataPreparer.ImageDataPreparer.LoadImageRaw(path, targetWidth, targetHeight);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = ((double)raw[i] / 255.0);
            }

            return data;
        }

        private class Record
        {
            public string label;
            public double val;
        }
    }
}
