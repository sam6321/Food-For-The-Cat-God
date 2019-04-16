using UnityEngine;

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
}
