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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // No self collision sounds pls
        if (!collision.collider.transform.IsChildOf(transform))
        {
            Utils.PlayRandomSound(audioSource, allAudio);
        }
    }
}
