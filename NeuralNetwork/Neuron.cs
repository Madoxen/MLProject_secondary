using System.Collections.Generic;

namespace NeuralNetwork
{
    class Neuron
    {
        public List<Synapse> Inputs { get; set; }
        public List<Synapse> Outputs { get; set; }
        public double InputValue { get; set; }
        public double OutputValue { get; set; }

        public Neuron()
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
        }

        public void AddOutputNeuron(Neuron outputneuron)
        {
            Synapse synapse = new Synapse(this, outputneuron);
            Outputs.Add(synapse); outputneuron.Inputs.Add(synapse);
        }

        public void AddInputSynapse(double input)
        {
            Synapse syn = new Synapse(this, input);
            Inputs.Add(syn);
        }

        public void CalculateOutput()
        {
            InputValue = Functions.InputSumFunction(Inputs);
            OutputValue = Functions.BipolarLinearFunction(InputValue);
        }

        public void PushValueOnInput(double input) 
        { 
            Inputs[0].OutputValue = input; 
        }
    }
}