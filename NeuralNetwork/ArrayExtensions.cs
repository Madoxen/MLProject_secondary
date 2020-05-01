using System;

namespace ML.Lib
{
    public static class ArrayExtensions
    {

        //Gets "columnIndex" column from given set
        //This function expects that the set is an rectangular matrix
        public static T[] GetColumn<T>(this T[][] set, int columnIndex)
        {
            T[] result = new T[set.Length];
            for (int i = 0; i < set.Length; i++)
            {
                result[i] = set[i][columnIndex];
            }
            return result;
        }


        //Sets "columnIndex"-column in set with given "column"
        //This method expects that the set is an rectangular matrix
        //And that the given data does not exceed any 
        public static void SetColumn<T>(this T[][] set, T[] column, int columnIndex)
        {
            for (int i = 0; i < set.Length; i++)
            {
                set[i][columnIndex] = column[i];
            }
        }

        //returns index of maximum element
        public static int MaxAt<T>(this T[] set) where T : IComparable<T>
        {
            T maxValue = set[0];
            int index = 0;
            for (int i = 0; i < set.Length; i++)
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