using System;

namespace NeuralNetwork
{
    class Synapse
    {
        static Random tmp = new Random();
        internal Neuron FromNeuron, ToNeuron;
        public double Weight { get; set; }
        public double OutputValue { get; set; }
        static public int SynapsesCount { get; set; } = 0;

        public Synapse(Neuron fromneuron, Neuron toneuron)
        {
            FromNeuron = fromneuron; ToNeuron = toneuron;
            Weight = tmp.NextDouble() - 0.5;
            SynapsesCount += 1;
        }

        public Synapse(Neuron toneuron, double output) //InputSynapse
        {
            ToNeuron = toneuron; OutputValue = output; 
            Weight = 1;
            SynapsesCount += 1;
        }

        public double GetOutput()
        {
            if (FromNeuron == null) return OutputValue;
            return FromNeuron.OutputValue * Weight;
        }
    }
}