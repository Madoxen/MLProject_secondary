using System.Collections.Generic;

namespace NeuralNetwork
{
    class Layer
    {
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
    }
}