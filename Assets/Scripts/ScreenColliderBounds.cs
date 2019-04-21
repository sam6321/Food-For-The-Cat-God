using UnityEngine;

public class ScreenColliderBounds : MonoBehaviour
{
    [SerializeField]
    private GameObject colliderPrefab;

    [SerializeField]
    private Transform spawnerArea;

    private GameObject left;
    private GameObject top;
    private GameObject right;
    private GameObject bottom;

    const float halfWidth = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        left = Instantiate(colliderPrefab, transform);
        top = Instantiate(colliderPrefab, transform);
        right = Instantiate(colliderPrefab, transform);
        bottom = Instantiate(colliderPrefab, transform);
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        Vector2 position = camera.transform.position;

        Vector2 horizontalOffset = new Vector2(camera.orthographicSize * camera.aspect + halfWidth, 0);
        Vector2 horizontalVerticalOffset = new Vector2(0, (camera.orthographicSize + halfWidth) * 0.5f);
        Vector2 verticalOffset = new Vector2(0, camera.orthographicSize + halfWidth);

        Vector2 horizontalScale = new Vector3(camera.orthographicSize * camera.aspect * 2, 1, 1);
        Vector2 verticalScale = new Vector3(1, (camera.orthographicSize + halfWidth) * 3.0f, 1);

        left.transform.position = position - horizontalOffset + horizontalVerticalOffset;
        left.transform.localScale = verticalScale;

        top.transform.position = position + verticalOffset * 2.0f;
        top.transform.localScale = horizontalScale;

        right.transform.position = position + horizontalOffset + horizontalVerticalOffset;
        right.transform.localScale = verticalScale; 

        bottom.transform.position = position - verticalOffset;
        bottom.transform.localScale = horizontalScale;

        spawnerArea.position = (Vector2)top.transform.position - verticalOffset * 0.25f;
        spawnerArea.localScale = new Vector3(horizontalScale.x * 0.95f, 1, 1);
    }
}
