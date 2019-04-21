using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> allAudio;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D ()
    {
        Utils.PlayRandomSound(audioSource, allAudio);
    }
}
