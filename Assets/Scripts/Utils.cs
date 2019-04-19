using UnityEngine;
using System.Collections.Generic;

public class Utils
{
    public static T[] Shuffle<T>(T[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            T tmp = array[i];
            int index = Random.Range(0, array.Length);
            array[i] = array[index];
            array[index] = tmp;
        }

        return array;
    }

    public static T RandomElement<T>(IList<T> collection) where T : new()
    {
        if (collection.Count == 0)
        {
            return new T();
        }
        else
        {
            return collection[Random.Range(0, collection.Count)];
        }
    }
}
