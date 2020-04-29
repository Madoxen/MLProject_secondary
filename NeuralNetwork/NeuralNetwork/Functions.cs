// Kamil Matula gr. D, 26.04.2020, Sztuczne Sieci Neuronowe
using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    class Functions
    {
        public static double Alpha { get; set; } = 0.5;

        public static double CalculateError(List<double> outputs, int row, double[][] ExpectedResult) // funkcja celu: Błąd Średniokwadratowy
        {
            double error = 0;
            for (int i = 0; i < outputs.Count; i++)
                error += Math.Pow(outputs[i] - ExpectedResult[row][i], 2);
            return error;
        }

        public static double InputSumFunction(List<Synapse> Inputs, double bias = 0) 
            // funkcja wejścia: suma iloczynów wag synaps i wyjść neuronów poprzednich + bias (zakłócenia)
        {
            double input = 0;
            foreach (Synapse syn in Inputs) 
                input += syn.GetOutput();
            input += bias;
            return input;
        }

        public static double BipolarLinearFunction(double input) // funkcja aktywacji: liniowa bipolarna
            => (1 - Math.Pow(Math.E, -Alpha * input)) / (1 + Math.Pow(Math.E, -Alpha * input));

        public static double BipolarDifferential(double input) // pochodna funkcji aktywacji: liniowej bipolarnej
            => (2 * Alpha * Math.Pow(Math.E, -Alpha * input)) / (Math.Pow(1 + Math.Pow(Math.E, -Alpha * input), 2));
    }
}