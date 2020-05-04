using System;

namespace NeuralNetwork
{
    class Synapse
    {
        static Random tmp = new Random();
        internal Neuron FromNeuron, ToNeuron;
        public double Weight { get; set; }
        public double PushedData { get; set; }
        public static int SynapsesCount { get; set; } = 0;
        public static double WeightDivider { get; set; }

        public Synapse(Neuron fromneuron, Neuron toneuron) // standard synapse
        {
            FromNeuron = fromneuron; ToNeuron = toneuron;
            Weight = (tmp.NextDouble() - 0.5) / WeightDivider;
            SynapsesCount += 1;
        }

        public Synapse(Neuron toneuron, double data) // input synapse for first layer
        {
            ToNeuron = toneuron; PushedData = data; 
            Weight = 1;
            SynapsesCount += 1;
        }

        public double GetOutput()
        {
            if (FromNeuron == null) return PushedData; // if it is first layer
            return FromNeuron.OutputValue * Weight;
        }
    }
}