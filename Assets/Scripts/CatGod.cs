using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CatGod : MonoBehaviour
{
    public enum Mood
    {
        Normal = 0,
        Happy = 1,
        Annoyed = 2
    }

    FoodManager foodManager;
    List<Food> allFood;
    List<Food> remainingFood;
    Food.FoodCheck currentCheck;
    int correct;
    int wrong;

    void Start()
    {
        foodManager = GameObject.Find("GameManager").GetComponent<FoodManager>();
    }

    public void OnIntroductionTextComplete()
    {
        allFood = foodManager.GenerateFoods(6);
        correct = 0;
        wrong = 0;
        foreach (Food food in allFood)
        {
            food.gameObject.SetActive(true);
        }
        // Only generate food checks from foods that should generate food checks
        remainingFood = allFood.Where(food => food.ShouldGenerateFoodCheck()).ToList();
        NextCheck();
    }

    void NextCheck()
    {
        if(remainingFood.Count == 0)
        {
            currentCheck = null;
            Debug.Log("All Done: " + correct + " correct and " + wrong + " wrong");
            // Clean up any remaining foods that weren't used (cat and dog food do not generate food checks and are not mandatory)
            foreach (Food food in allFood)
            {
                food.gameObject.SetActive(false);
            }
        }
        else
        {
            // Pick a random food from the list of remaining food and use it to generate a food check.
            int index = Random.Range(0, remainingFood.Count - 1);
            Food food = remainingFood[index];
            currentCheck = food.GetRandomFoodCheck();

            Debug.Log(currentCheck.GetCheckString());
        }
    }

    void OnDrop(GameObject droppedObject)
    {
        Food food = droppedObject.GetComponent<Food>();
        if(currentCheck != null && food)
        {
            Debug.Assert(remainingFood.Contains(food), "Non spawned food object somehow dropped on cat god?");

            if(currentCheck.Check(food))
            {
                Debug.Log("Yes, that's what I want!");
                food.gameObject.SetActive(false);
                remainingFood.Remove(food);
                NextCheck();
                correct++;
            }
            else
            {
                Debug.Log("No, ew!!");
                wrong++;
            }
        }
    }
}
