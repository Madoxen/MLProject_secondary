using System.IO;
using System;
using System.Linq;
using NeuralNetwork;
using System.Collections.Generic;

namespace NeuralNetwork.Tests
{

    public class Loader
    {
        /// <summary>
        /// Test loader, loads data.csv file
        /// Use only for simple tests
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static double[][][] Load(string path)
        {
            /*data:
            [0] -> Input Data to be evaluated
            [1] -> Expected Output Data
            [2] -> Test Input Data
            [3] -> Test Output Data*/
            double[][][] finalData = new double[4][][];

            List<double[]> learningInputData = new List<double[]>();
            List<double[]> learningOutputData = new List<double[]>();
            List<double[]> testInputData = new List<double[]>();
            List<double[]> testOutputData = new List<double[]>();

            string[] lines = File.ReadAllLines(path).Skip(1).ToArray();   //Start from second line
            Shuffler.Shuffle(lines); //randomize data order

            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(",");
                double[] data = new double[4];
                double[] output = new double[2];


                //Load data
                for (int j = 0; j < 4; j++)
                {
                    data[j] = Convert.ToDouble(tokens[j]);
                }

                //Load class
                if (tokens[4] == "0")
                {
                    output = new double[] { 1.0, 0.0 };
                }
                else if (tokens[4] == "1")
                {
                    output = new double[] { 0.0, 1.0 };
                }
                else
                {
                    throw new Exception("Error while reading data file: Unrecognized object class");
                }


                if (i % 3 == 0) //take 30% of data as test data
                {
                    testInputData.Add(data);
                    testOutputData.Add(output);
                }
                else //take 70% as learning data
                {
                    learningInputData.Add(data);
                    learningOutputData.Add(output);
                }
            }


            //Pack everything 
            finalData[0] = learningInputData.ToArray();
            learningInputData.Clear();
            finalData[1] = learningOutputData.ToArray();
            learningOutputData.Clear();
            finalData[2] = testInputData.ToArray();
            testInputData.Clear();
            finalData[3] = testOutputData.ToArray();
            testOutputData.Clear();

            //Normalize data arrays (not output arrays as those are already normalized)
            for (int i = 0; i < 4; i++)
            {
             //   finalData[0].SetColumn(Normalizator.Normalize(finalData[0].GetColumn(i), 0.0, 1.0), i);
               // finalData[2].SetColumn(Normalizator.Normalize(finalData[2].GetColumn(i), 0.0, 1.0), i);
            }




            return finalData;
        }
    }
}