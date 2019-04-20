using UnityEngine;

public class SoftRock : MonoBehaviour
{
    [SerializeField]
    private float minRot;

    [SerializeField]
    private float maxRot;


    // Update is called once per frame
    void Update()
    {
        float mod = (Mathf.Cos(Time.time) + 1) * 0.5f;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(minRot, maxRot, mod));
    }
}
