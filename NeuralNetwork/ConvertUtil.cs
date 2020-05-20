using System;
using System.Globalization;

namespace NeuralNetwork
{
    public static class ConvertUtil
    {
        public static double ConvertArg(string d)
        {
            return Double.Parse(d.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }

}