using System.Collections.Generic;

namespace Rei.Utilities
{
    public static class CollectionTool
    {
        public static List<T> Copy<T>(this List<T> listToCopy)
        {
            List<T> output = new List<T>(listToCopy.Count);

            foreach (var item in listToCopy)
            {
                output.Add(item);
            }

            return output;
        }
        
        public static T[] Copy<T>(this T[] arrayToCopy)
        {
            T[] output = new T[arrayToCopy.Length];

            for (int i = 0; i < arrayToCopy.Length; i++)
            {
                output[i] = arrayToCopy[i];
            }

            return output;
        }
    }
}