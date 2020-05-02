using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public class MeanErrorTest : ITestStrategy
    {

        private Network network;
        private double minError;
        public double MinError
        {
            get { return minError; }
            set { minError = value; }
        }

        private double recentError;


        public MeanErrorTest(Network network, double minError = 0.001)
        {
            this.network = network;
            this.minError = minError;
        }


        public double Test(double[][] inputs, double[][] expectedOutputs)
        {
            double error = 0;
            List<double> outputs = new List<double>();
            for (int i = 0; i < inputs.Length; i++)
            {
                network.PushInputValues(inputs[i]);
                outputs = network.GetOutput();
                error += Functions.CalculateError(outputs, i, expectedOutputs);
            }
            error /= inputs.Length;
            recentError = error;
            Console.WriteLine($" Average mean square error: {Math.Round(error, 5)}");
            return error;
        }

        public bool CheckHalt()
        {
            return recentError < minError;
        }
    }

}