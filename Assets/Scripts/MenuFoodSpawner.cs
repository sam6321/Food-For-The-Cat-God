using System.Collections;
using UnityEngine;

public class MenuFoodSpawner : MonoBehaviour
{
    [SerializeField]
    private float delay = 0.2f;

    [SerializeField]
    private GameObject[] prefabs;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        foreach(GameObject prefab in prefabs)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(new Vector2(
                Random.Range(Screen.width * 0.1f, Screen.width * 0.9f),
                Random.Range(Screen.height * 0.1f, Screen.height * 0.9f)
            ));

            Instantiate(prefab, position, Quaternion.identity);

            yield return new WaitForSeconds(delay);
        }
    }
}
