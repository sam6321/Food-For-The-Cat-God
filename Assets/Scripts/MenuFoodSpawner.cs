using System.Collections;
using UnityEngine;

public class MenuFoodSpawner : MonoBehaviour
{
    [SerializeField]
    private float delay = 0.2f;

    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    private Transform spawnVolume;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        int spawned = 0;
        foreach(GameObject prefab in prefabs)
        {
            Vector2 spawnVolumeOffset = new Vector2(spawnVolume.transform.localScale.x * 0.5f, 0.0f);
            Vector2 spawnStart = (Vector2)spawnVolume.transform.position - spawnVolumeOffset;
            Vector2 spawnEnd = (Vector2)spawnVolume.transform.position + spawnVolumeOffset;

            Vector2 position = Vector2.Lerp(spawnStart, spawnEnd, Mathf.PingPong(spawned, 6) / 6.0f);

            Instantiate(prefab, position, Quaternion.identity);
            spawned++;

            yield return new WaitForSeconds(delay);
        }
    }
}
