using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Favour : MonoBehaviour
{
    const float MaxFavour = 100.0f;

    [SerializeField]
    private float favourDecreasePerSecond = 3;

    [SerializeField]
    private UnityEvent onFavourHitZero;

    private float favour = MaxFavour;
    private float displayFavour = MaxFavour;

    private SpriteMask mask;

    void Start()
    {
        mask = GetComponentInChildren<SpriteMask>();
        Enabled = false;
    }

    // Favour begins at 100 and slowly decreases at a fixed rate. When it hits 0, you lose.
    // Favour is boosted up by correct foods, and decreased by wrong ones
    void Update()
    {
        if (favour > 0.0f && Enabled)
        {
            favour = Mathf.Max(0.0f, favour - favourDecreasePerSecond * Time.deltaTime);
            if (favour == 0.0f)
            {
                onFavourHitZero.Invoke();
            }
        }

        float moveAmount = Mathf.Abs(displayFavour - favour) * 0.1f + 5.0f * Time.deltaTime;
        displayFavour = Mathf.MoveTowards(displayFavour, favour, moveAmount);
        mask.alphaCutoff = 1.0f - displayFavour / MaxFavour;
    }

    public bool Enabled { set; get; }

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
        favour = Mathf.Min(favour + amount, MaxFavour);
    }
}
