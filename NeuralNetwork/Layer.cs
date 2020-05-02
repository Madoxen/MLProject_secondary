using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    class Layer
    {

        private static Random r = new Random();
        public List<Neuron> Neurons;

        public Layer()
        {
            Neurons = new List<Neuron>();
        }

        public Layer(int numberofneurons)
        {
            Neurons = new List<Neuron>();
            for (int i = 0; i < numberofneurons; i++)
                Neurons.Add(new Neuron());
        }

        public void ConnectLayers(Layer outputlayer)
        {
            foreach (Neuron thisneuron in Neurons)
                foreach (Neuron thatneuron in outputlayer.Neurons)
                    thisneuron.AddOutputNeuron(thatneuron);
        }

        public void CalculateOutputOnLayer()
        {
            foreach (Neuron neuron in Neurons)
                neuron.CalculateOutput();
        }


        /// <summary>
        /// Randomizes weights using Box Muller distribution algorithm
        /// Based on: http://neuralnetworksanddeeplearning.com/chap3.html#weight_initialization
        /// </summary>
        public void RandomizeWeights()
        {
            foreach (Neuron n in Neurons)
            {
                foreach (Synapse s in n.Inputs)
                {
                    s.Weight = r.NextDouble() * Math.Sqrt(2.0 / (Neurons.Count + n.Inputs.Count));
                }
            }
        }
    }
}