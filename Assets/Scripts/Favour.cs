﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Favour : MonoBehaviour
{
    const float MaxFavour = 100.0f;

    [SerializeField]
    private float favourDecreasePerSecond = 3;

    [SerializeField]
    private float favourTickingThreshold = 20;

    [SerializeField]
    private UnityEvent onFavourHitZero;

    private float favour = MaxFavour;
    private float displayFavour = MaxFavour;

    private SpriteMask mask;
    private AudioSource audioSource;

    void Start()
    {
        mask = GetComponentInChildren<SpriteMask>();
        audioSource = GetComponent<AudioSource>();
        Enabled = false;
    }

    // Favour begins at 100 and slowly decreases at a fixed rate. When it hits 0, you lose.
    // Favour is boosted up by correct foods, and decreased by wrong ones
    void Update()
    {
        if (favour > 0.0f && Enabled)
        {
            FavourPunch(-favourDecreasePerSecond * Time.deltaTime);
        }

        float moveAmount = Mathf.Abs(displayFavour - favour) * 0.1f + 5.0f * Time.deltaTime;
        displayFavour = Mathf.MoveTowards(displayFavour, favour, moveAmount);
        mask.alphaCutoff = 1.0f - displayFavour / MaxFavour;
    }

    private bool _enabled;
    public bool Enabled
    { 
        set
        {
            _enabled = value;
            SetFavourTickingSound();
        }
        
        get
        {
            return _enabled;
        }
    }

    public float DecreasePerSecond
    {
        set { favourDecreasePerSecond = value; }
        get { return favourDecreasePerSecond; }
    }

    public void ResetFavour()
    {
        favour = MaxFavour;
    }

    public void FavourPunch(float amount)
    {
        favour = Mathf.Clamp(favour + amount, 0, MaxFavour);
        if (favour == 0.0f)
        {
            onFavourHitZero.Invoke();
        }
        SetFavourTickingSound();
    }

    private void SetFavourTickingSound()
    {
        bool desired = favour <= favourTickingThreshold && favour > 0 && Enabled;
        if(audioSource.isPlaying != desired)
        {
            if(desired)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}
