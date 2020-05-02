using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public static class ListExtensions
    {

        public static int MaxAt<T>(this IList<T> set) where T : IComparable<T>
        {
            T maxValue = set[0];
            int index = 0;
            for (int i = 0; i < set.Count; i++)
            {
                if (maxValue.CompareTo(set[i]) < 0)
                {
                    index = i;
                    maxValue = set[i];
                }
            }
            return index;
        }

    }

}