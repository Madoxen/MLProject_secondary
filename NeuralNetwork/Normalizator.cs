using System;

namespace ML.Lib
{
    public class Normalizator
    {
        static double Max(double[] input)
        {
            double result = double.MinValue;
            for (int i = 0; i < input.Length; i++)
            {
                if (result < input[i])
                    result = input[i];
            }
            return result;
        }

        static double Min(double[] input)
        {
            double result = double.MaxValue;
            for (int i = 0; i < input.Length; i++)
            {
                if (result > input[i])
                    result = input[i];
            }
            return result;
        }

        public static double[] Normalize(double[] input, double nmin, double nmax)
        {
            double[] result = new double[input.Length];
            double min = Min(input);
            double max = Max(input);

            for (int i = 0; i < input.Length; i++)
            {
                result[i] = ((input[i] - min) / (max - min)) * (nmax - nmin) + nmin;
            }
            return result;
        }

     



    }
}
