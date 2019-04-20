using UnityEngine;

public class SoftRotate : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, rotateSpeed * Time.time);
    }
}
