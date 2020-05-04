﻿using System;
using System.Collections.Generic;
using System.IO;

namespace NeuralNetwork
{
    public class Network
    {
        static double LearningRate { get; set; }
        internal List<Layer> Layers;
        internal double[][] ExpectedResult;
        double[][] ErrorFunctionChanges;

        public ITestStrategy testStrategy;
        public bool TestHaltEnabled { get; set; }

        public bool TestingEnabled { get; set; }

        public Network(double learningrate, double alpha, double mininitweight, double maxinitweight, int numInputNeurons, 
        int[] hiddenLayerSizes, int numOutputNeurons, bool testHaltEnabled = false, bool testingEnabled = true)
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


            Layers = new List<Layer>();
            AddFirstLayer(numInputNeurons);
            for (int i = 0; i < hiddenLayerSizes.Length; i++)
                AddNextLayer(new Layer(hiddenLayerSizes[i]));
            AddNextLayer(new Layer(numOutputNeurons));

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
            SaveWeights(@"startingweights.txt");
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
                }
            }
            SaveWeights(@"endingweights.txt");
        }

        public void RandomizeWeights()
        {
            foreach (Layer l in Layers)
            {
                l.RandomizeWeights();
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

        public void SaveWeights(string path)
        {
            List<string> tmp = new List<string>();
            foreach (Layer layer in Layers)
                foreach (Neuron neuron in layer.Neurons)
                    foreach (Synapse synapse in neuron.Inputs)
                        tmp.Add(synapse.Weight.ToString());
            File.WriteAllLines(path, tmp);
        }

        public void LoadWeights(string[] lines)
        {
            Console.WriteLine(" Loading weights...");
            if (lines.Length != Synapse.SynapsesCount)
                Console.WriteLine(" Incorrect input file.");
            else
            {
                try
                {
                    int i = 0;
                    foreach (Layer layer in Layers)
                        foreach (Neuron neuron in layer.Neurons)
                            foreach (Synapse synapse in neuron.Inputs)
                                synapse.Weight = Double.Parse(lines[i++]);
                }
                catch (Exception) { Console.WriteLine(" Incorrect input file."); }
            }
        }
    }
}