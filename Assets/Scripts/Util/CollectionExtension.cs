using System;
using System.Collections.Generic;

namespace Util
{
    public static class CollectionExtension
    {
        private static Random Rand = new Random();

        public static T RandomElement<T>(this T[] items)
        {
            return items[Rand.Next(0, items.Length)];
        }

        public static T RandomElement<T>(this List<T> items)
        {
            return items[Rand.Next(0, items.Count)];
        }   

        public static T[] ArrayShiftForwardByOne<T>(this T[] source)
        {
            var result = new T[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[(i + 1) % source.Length] = source[i];
            }

            return result;
        }   

        public static T[] ArrayShiftBackwardByOne<T>(this T[] source)
        {
            var result = new T[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[ArrayLoopingIndex(i -1, result.Length)] = source[i];
            }

            return result;
        }


        public static int ArrayLoopingIndex(int index, int len)
        {
            if (index < 0) index = len -1;
            if (index > len -1) index = 0;
            return index;
        }
    }
}