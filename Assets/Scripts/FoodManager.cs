using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foodPrefabs;

    [SerializeField]
    private CatGod catGod;

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
        foreach(Food food in selectedFoods)
        {
            food.gameObject.SetActive(true);
        }
        return selectedFoods;
    }

    public void CleanupFoods(List<Food> foods)
    {
        foreach(Food food in foods)
        {
            food.gameObject.SetActive(false);
        }
    }

    public int MaxFood
    {
        get { return foodObjects.Length; }
    }
}
