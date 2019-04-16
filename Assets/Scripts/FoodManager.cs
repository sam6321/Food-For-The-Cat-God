using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foodPrefabs;

    private GameObject[] foodObjects;
    private Food[] foods;

    public static Food.IFoodCheck[] GenerateRandomFoodCheck(Food[] foods, uint count)
    {
        Food[] shuffledFoods = Utils.Shuffle((Food[])foods.Clone());

        List<Food.IFoodCheck> foodChecks = new List<Food.IFoodCheck>();
        for (uint i = 0; i < count; i++)
        {
            Food food = shuffledFoods[i];
            if(food.ShouldGenerateFoodCheck())
            {
                foodChecks.Add(food.GetRandomFoodCheck());
            }
        }
        return foodChecks.ToArray();
    }

    void Start()
    {
        foodObjects = foodPrefabs.Select(p => Instantiate(p)).ToArray();
        foods = foodObjects.Select(o => o.GetComponent<Food>()).ToArray();
        Food.IFoodCheck[] checks = GenerateRandomFoodCheck(foods, 5);
        foreach(Food.IFoodCheck c in checks)
        {
            Debug.Log(c.GetCheckString());
        }
    }
}
