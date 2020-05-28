using System;
using System.IO;
using System.Linq;
using NeuralNetwork;
using DataPreparer;
using System.Collections.Generic;

namespace LabelGuesser
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

            string resourcesPath = "../MovieClassifierLearner/MLSecondaryDataSet";
            string modelPath = "model.txt";


            //Load network from file
            Network net = Network.LoadNetworkFromFile(modelPath);
            //Load data set from dir
            Console.WriteLine("Loading images...");
            ImageLearningData[] data = ImageDataPreparer.PrepareImages(resourcesPath, 50, 25);
            List<LabelID> ids = new List<LabelID>();
            Console.WriteLine("Calculating averages...");
            int a = 0;
            foreach (ImageLearningData d in data)
            {
                Console.Write("\r" + a + "/" + data.Count());
                net.PushInputValues(d.data.ToArray());
                List<double> currentOutput = net.GetOutput();
                if (ids.Exists(x => x.label == d.label))
                {
                    LabelID i = ids.Find(x => x.label == d.label);
                    for (int j = 0; j < i.expectedOutput.Length; j++)
                    {
                        i.expectedOutput[j] += currentOutput[j];
                    }
                    i.hits++;
                }
                else
                {
                    LabelID i = new LabelID();
                    i.label = d.label;
                    i.expectedOutput = currentOutput.ToArray();
                    i.hits = 1;
                    ids.Add(i);
                }
                a++;
            }

            foreach(LabelID i in ids)
            {
                Console.WriteLine(i.label + " is " + i.expectedOutput.MaxAt());
                for(int j = 0; j < i.expectedOutput.Length; j++)
                {
                    Console.Write((i.expectedOutput[j] / i.hits) + " ");
                }
                Console.WriteLine();   
            }

            ids = ids.OrderBy(x => x.expectedOutput.MaxAt()).ToList();
            Console.WriteLine("So the label.txt should look like this");
            foreach(LabelID i in ids)
            {
                Console.WriteLine(i.label);
            }


        }
    }
}
