using System;
using System.Collections.Generic;


namespace NeuralNetwork
{
    public class Shuffler
    {
        public static void Shuffle<T>(T[] input)
        {
            Random rand = new Random();
            for (int i = 0; i < input.Length; i++)
            {
                Swap<T>(input, i, rand.Next(0, input.Length - 1));
            }
        }

        static void Swap<T>(T[] input, int a, int b)
        {
            T buff = input[a];
            input[a] = input[b];
            input[b] = buff;
        }

    }
}
