using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public class HighestHitTest : ITestStrategy
    {
        private Network network;
        private double maximumPercentageHalt;
        public double MinDelta
        {
            get { return maximumPercentageHalt; }
            set { maximumPercentageHalt = value; }
        }

        private double recentPercentage;
        public double CurrentRecord { get; private set; }


        public HighestHitTest(Network network, double minDelta = 0.001)
        {
            this.network = network;
            this.maximumPercentageHalt = minDelta;
        }

        public double Test(double[][] inputs, double[][] expectedOutputs)
        {
            double hitPercentage = 0;
            int hits = 0;
            List<double> outputs = new List<double>();
            for (int i = 0; i < inputs.Length; i++)
            {
                network.PushInputValues(inputs[i]);
                outputs = network.GetOutput();
                if (outputs.MaxAt() == expectedOutputs[i].MaxAt())
                    hits++;
            }
            hitPercentage = (double)hits / (double)inputs.Length;
            recentPercentage = hitPercentage;
            Console.WriteLine($" Hit percentage : {Math.Round(hitPercentage * 100.0, 3)}%");
            return hitPercentage;
        }

        public bool CheckHalt()
        {
            return recentPercentage >= maximumPercentageHalt;
        }

        public bool CheckRecord()
        {
            if (CurrentRecord < recentPercentage)
            {
                CurrentRecord = recentPercentage;
                return true;
            }
            return false;
        }
    }

}