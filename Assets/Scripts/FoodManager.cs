﻿using UnityEngine;
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

    public List<Food> GenerateFoods(uint count)
    {
        Food[] shuffledFoods = Utils.Shuffle((Food[])foods.Clone());
        return shuffledFoods.Take((int)count).ToList();
    }
}
