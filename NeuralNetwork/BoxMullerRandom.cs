using System;

namespace NeuralNetwork
{
    public static class BoxMullerRandom
    {
        public static double NextGaussianDouble(this Random r, double mean, double standardDeviation)
        {
            double u1 = 1.0 - r.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - r.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + standardDeviation * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }


    }

}