using UnityEngine;
using System;
using System.Collections.Generic;

public class Utils
{
    public static T[] Shuffle<T>(T[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            T tmp = array[i];
            int index = UnityEngine.Random.Range(0, array.Length);
            array[i] = array[index];
            array[index] = tmp;
        }

        return array;
    }

    public static T RandomElement<T>(IList<T> collection)
    {
        if (collection.Count == 0)
        {
            throw new InvalidOperationException("Picking random from empty list");
        }
        else
        {
            return collection[UnityEngine.Random.Range(0, collection.Count)];
        }
    }

    public static void PlayRandomSound(AudioSource source, IList<AudioClip> clips)
    {
        AudioClip clip = RandomElement(clips);
        source.PlayOneShot(clip);
    }
}
