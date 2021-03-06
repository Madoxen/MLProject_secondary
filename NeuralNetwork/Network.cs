﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace NeuralNetwork
{
    public class Network
    {
        static double LearningRate { get; set; }
        static double SynapsesCount { get; set; }
        internal List<Layer> Layers;
        internal double[][] ExpectedResult;
        double[][] ErrorFunctionChanges;

        public ITestStrategy testStrategy;
        public bool TestHaltEnabled { get; set; }

        public bool TestingEnabled { get; set; }

        public bool RecordSaveEnabled { get; set; }

        public Network(double learningrate, double alpha, double mininitweight, double maxinitweight, int numInputNeurons,
        int[] hiddenLayerSizes, int numOutputNeurons, bool testHaltEnabled = false, bool testingEnabled = true, bool recordSaveEnabled = true)
        {
            Console.WriteLine("\n Building neural network...");
            if (numInputNeurons < 1 || hiddenLayerSizes.Length < 1 || numOutputNeurons < 1)
                throw new Exception("Incorrect Network Parameters");

            Functions.Alpha = alpha;
            Synapse.MinInitWeight = mininitweight;
            Synapse.MaxInitWeight = maxinitweight;
            LearningRate = learningrate;
            this.testStrategy = new MeanErrorTest(this);
            this.TestHaltEnabled = testHaltEnabled;
            this.TestingEnabled = testingEnabled;
            this.RecordSaveEnabled = recordSaveEnabled;

            Layers = new List<Layer>();
            AddFirstLayer(numInputNeurons);
            for (int i = 0; i < hiddenLayerSizes.Length; i++)
                AddNextLayer(new Layer(hiddenLayerSizes[i]));
            AddNextLayer(new Layer(numOutputNeurons));

            SynapsesCount = Synapse.SynapsesCount;

            ErrorFunctionChanges = new double[Layers.Count][];
            for (int i = 1; i < Layers.Count; i++)
                ErrorFunctionChanges[i] = new double[Layers[i].Neurons.Count];
        }

        private void AddFirstLayer(int inputneuronscount)
        {
            Layer inputlayer = new Layer(inputneuronscount);
            foreach (Neuron neuron in inputlayer.Neurons)
                neuron.AddInputSynapse(0);
            Layers.Add(inputlayer);
        }

        private void AddNextLayer(Layer newlayer)
        {
            Layer lastlayer = Layers[Layers.Count - 1];
            lastlayer.ConnectLayers(newlayer);
            Layers.Add(newlayer);
        }

        public void PushInputValues(double[] inputs)
        {
            if (inputs.Length != Layers[0].Neurons.Count)
                throw new Exception("Incorrect Input Size");

            for (int i = 0; i < inputs.Length; i++)
                Layers[0].Neurons[i].PushValueOnInput(inputs[i]);
        }

        public void PushExpectedValues(double[][] expectedvalues)
        {
            if (expectedvalues[0].Length != Layers[Layers.Count - 1].Neurons.Count)
                throw new Exception("Incorrect Expected Output Size");

            ExpectedResult = expectedvalues;
        }

        public List<double> GetOutput()
        {
            List<double> output = new List<double>();
            for (int i = 0; i < Layers.Count; i++)
                Layers[i].CalculateOutputOnLayer();
            foreach (Neuron neuron in Layers[Layers.Count - 1].Neurons)
                output.Add(neuron.OutputValue);
            return output;
        }


        /// <summary>
        /// Trains network with given data
        /// </summary>
        /// <param name="data">
        /// [0] -> Input Data to be evaluated
        /// [1] -> Expected Output Data
        /// [2] -> Test Input Data
        /// [3] -> Test Output Data</param>
        /// <param name="epochCount"></param>
        public void Train(double[][][] data, int epochCount)
        {
            double[][] inputs = data[0], expectedOutputs = data[1];
            double[][] testInputs = data[2], testOutputs = data[3];

            PushExpectedValues(expectedOutputs);

            Console.WriteLine(" Training neural network...");
            for (int i = 0; i < epochCount; i++)
            {
                List<double> outputs = new List<double>();
                for (int j = 0; j < inputs.Length; j++)
                {
                    PushInputValues(inputs[j]);
                    outputs = GetOutput();
                    ChangeWeights(outputs, j);
                }

                if (TestingEnabled == true)
                {
                    testStrategy.Test(testInputs, testOutputs);
                    if (testStrategy.CheckHalt() && TestHaltEnabled == true)
                        break;
                    if (testStrategy.CheckRecord() && RecordSaveEnabled == true)
                        SaveNetworkToFile(@"record_weights" + "_" + testStrategy.GetType().Name.ToString() + "_" + Math.Round(testStrategy.CurrentRecord, 2).ToString() + ".txt");
                }
            }
        }

        public void RandomizeWeights()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                Layers[i].RandomizeWeights();
            }
        }

        private void CalculateErrorFunctionChanges(List<double> outputs, int row)
        {
            for (int i = 0; i < Layers[Layers.Count - 1].Neurons.Count; i++)
                ErrorFunctionChanges[Layers.Count - 1][i] = (ExpectedResult[row][i] - outputs[i])
                    * Functions.BipolarDifferential(Layers[Layers.Count - 1].Neurons[i].InputValue);
            for (int k = Layers.Count - 2; k > 0; k--)
                for (int i = 0; i < Layers[k].Neurons.Count; i++)
                {
                    ErrorFunctionChanges[k][i] = 0;
                    for (int j = 0; j < Layers[k + 1].Neurons.Count; j++)
                        ErrorFunctionChanges[k][i] += ErrorFunctionChanges[k + 1][j] * Layers[k + 1].Neurons[j].Inputs[i].Weight;
                    ErrorFunctionChanges[k][i] *= Functions.BipolarDifferential(Layers[k].Neurons[i].InputValue);
                }
        }

        private void ChangeWeights(List<double> outputs, int row)
        {
            CalculateErrorFunctionChanges(outputs, row);
            for (int k = Layers.Count - 1; k > 0; k--)
                for (int i = 0; i < Layers[k].Neurons.Count; i++)
                    for (int j = 0; j < Layers[k - 1].Neurons.Count; j++)
                        Layers[k].Neurons[i].Inputs[j].Weight +=
                            LearningRate * 2 * ErrorFunctionChanges[k][i] * Layers[k - 1].Neurons[j].OutputValue;
        }

        public void SaveNetworkToFile(string path)
        {
            List<string> tmp = new List<string>();
            for (int i = 1; i < Layers.Count; i++)
                foreach (Neuron neuron in Layers[i].Neurons)
                    foreach (Synapse synapse in neuron.Inputs)
                        tmp.Add(synapse.Weight.ToString(CultureInfo.InvariantCulture));
            
            string build = $"{LearningRate.ToString()} {Functions.Alpha.ToString()} {Synapse.MinInitWeight} {Synapse.MaxInitWeight}";
            foreach (Layer layer in Layers) build += " " + layer.Neurons.Count.ToString();
            tmp.Insert(0, build);
            File.WriteAllLines(path, tmp);
        }


        // loading from .txt file where in first line there are: learning rate, alpha, minimum init weight, 
        // maximum init weight and sizes of all layers - all separated by spaces; other lines are synapse weights 
        // (one per line)
        public static Network LoadNetworkFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string[] firstLine = lines[0].Split();
            List<int> hiddenLayerSizes = new List<int>();
            for (int i = 5; i < firstLine.Length - 1; i++) 
                hiddenLayerSizes.Add(Convert.ToInt32(firstLine[i]));
            
            Network net = new Network(ConvertUtil.ConvertArg(firstLine[0]), ConvertUtil.ConvertArg(firstLine[1]), 
                ConvertUtil.ConvertArg(firstLine[2]), ConvertUtil.ConvertArg(firstLine[3]), Convert.ToInt32(firstLine[4]), 
                hiddenLayerSizes.ToArray(), Convert.ToInt32(firstLine[firstLine.Length - 1]));

            Console.WriteLine(" Loading weights...");
            if (lines.Length - 1 != SynapsesCount)
                Console.WriteLine(" Incorrect input file.");
            else
            {
                try
                {
                    int i = 1;
                    for (int j = 1; j < net.Layers.Count; j++)
                        foreach (Neuron neuron in net.Layers[j].Neurons)
                            foreach (Synapse synapse in neuron.Inputs)
                                synapse.Weight = ConvertUtil.ConvertArg(lines[i++]);
                }
                catch (Exception) { Console.WriteLine(" Incorrect input file."); }
            }
            return net;
        }


        public int GetLayerSize(int layerIndex)
        {
            return Layers[layerIndex].Neurons.Count;
        }
    }
}