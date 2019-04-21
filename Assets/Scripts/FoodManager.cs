using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foodPrefabs;

    [SerializeField]
    private CatGod catGod;

    [SerializeField]
    private GameObject spawnVolume;

    Coroutine spawnFoodCoroutine;

    private GameObject[] foodObjects;
    private Food[] foods;

    void Start()
    {
        foodObjects = foodPrefabs.Select(p => {
            GameObject foodObject = Instantiate(p);
            foodObject.SetActive(false);
            return foodObject;
        }).ToArray();
        foods = foodObjects.Select(o => o.GetComponent<Food>()).ToArray();
    }

    public List<Food> GenerateFoods(int count)
    {
        Food[] shuffledFoods = Utils.Shuffle((Food[])foods.Clone());
        List<Food> selectedFoods = shuffledFoods.Take(count).ToList();
        spawnFoodCoroutine = StartCoroutine(SpawnFood(selectedFoods));
        return selectedFoods;
    }

    public void CleanupFoods(List<Food> cleanup)
    {
        foreach(Food food in cleanup)
        {
            food.gameObject.SetActive(false);
        }
    }

    public int MaxFood
    {
        get { return foodObjects.Length; }
    }

    IEnumerator SpawnFood(List<Food> selectedFoods)
    {
        int spawned = 0;
        Vector2 spawnVolumeOffset = new Vector2(spawnVolume.transform.localScale.x * 0.5f, 0.0f);
        Vector2 spawnStart = (Vector2)spawnVolume.transform.position - spawnVolumeOffset;
        Vector2 spawnEnd = (Vector2)spawnVolume.transform.position + spawnVolumeOffset;
        
        foreach (Food food in selectedFoods)
        {
            food.gameObject.transform.position = Vector2.Lerp(spawnStart, spawnEnd, Mathf.PingPong(spawned, 7) / 7.0f);
            food.gameObject.SetActive(true);
            spawned++;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
