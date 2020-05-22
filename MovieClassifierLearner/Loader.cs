using System.Collections.Generic;
using System.Linq;
using System.IO;
using DataPreparer;
using NeuralNetwork;

namespace MovieClassifierLearner
{

    public static class Loader
    {
        public static double[][][] Load(string path, int outputCount, int imageWidth, int imageHeight)
        {
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
            List<string> uniqueLabels = new List<string>();

            { //Ensure that ImageLearningData[] will be disposed after scope exit
                ImageLearningData[] data = ImageDataPreparer.PrepareImages("Resources", imageWidth, imageHeight);
                //Pack data into double Data table


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
                    if (i % 10 == 0)
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

            //Shuffling
            int[] numbers = new int[inputData.Count];
            for (int i = 0; i < numbers.Length; i++) numbers[i] = i;
            Shuffler.Shuffle(numbers);
            List<double[]> tmpInputData = new List<double[]>();
            List<double[]> tmpOutputData = new List<double[]>();
            for (int i = 0; i < numbers.Length; i++)
            {
                tmpInputData.Add(inputData[numbers[i]]);
                tmpOutputData.Add(expectedOutputData[numbers[i]]);
            }
            inputData = tmpInputData;
            expectedOutputData = tmpOutputData;

            //Pack everything 
            finalData[0] = inputData.ToArray();
            inputData.Clear();
            finalData[1] = expectedOutputData.ToArray();
            expectedOutputData.Clear();
            finalData[2] = testInputData.ToArray();
            testInputData.Clear();
            finalData[3] = testOutputData.ToArray();
            testOutputData.Clear();

            //Output labels
            File.WriteAllLines("labels.txt", uniqueLabels);

            return finalData;
        }

    }
}