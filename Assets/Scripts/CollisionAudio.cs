using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> allAudio;

     void OnCollisionEnter2D ()
     {
        GetComponent<AudioSource>().clip = allAudio[UnityEngine.Random.Range(0, allAudio.Count)];
        GetComponent<AudioSource>().Play();
     }
}
