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



            Network net = new Network(imageWidth * imageHeight * imageDepth, 2, 100, outputCount);
            /*data:
            [0] -> Input Data to be evaluated
            [1] -> Expected Output Data
            [2] -> Test Input Data
            [3] -> Test Output Data*/
            double[][][] finalData = new double[4][][];

            List<double[]> inputData = new List<double[]>();
            List<double[]> expectedOutputData = new List<double[]>();
            List<double[]> testInputData = new List<double[]>();
            List<double[]> testOutputData = new List<double[]>();

            { //Ensure that ImageLearningData[] will be disposed after scope exit
                ImageLearningData[] data = ImageDataPreparer.PrepareImages("Resources", imageWidth, imageHeight);
                //Pack data into double Data table

                List<string> uniqueLabels = new List<string>();
                for (int i = 0; i < data.Length; i++)
                {
                    int labelIndex = uniqueLabels.IndexOf(data[i].label);
                    if (labelIndex == -1)
                    {
                        uniqueLabels.Add(data[i].label);
                        labelIndex = uniqueLabels.Count - 1;
                    }


                    //Assign expected output
                    double[] output = new double[outputCount];
                    output[labelIndex] = 1.0;

                    //Assign input values
                    double[] input = data[i].data.ToArray();

                    //Decide between test set and learning set
                    if (i % 5 == 0)
                    {
                        testInputData.Add(input);
                        testOutputData.Add(output);
                    }
                    else
                    {
                        inputData.Add(input);
                        expectedOutputData.Add(output);
                    }
                }
            }

            //Pack everything 
            finalData[0] = inputData.ToArray();
            inputData.Clear();
            finalData[1] = expectedOutputData.ToArray();
            expectedOutputData.Clear();
            finalData[2] = testInputData.ToArray();
            testInputData.Clear();
            finalData[3] = testOutputData.ToArray();
            testOutputData.Clear();

            net.Train(finalData, 10);


            net.PushInputValues(finalData[2][4]);

            //smol test
            List<double> o = net.GetOutput();
            Console.WriteLine("Expected:");
            foreach (double d in finalData[3][4])
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