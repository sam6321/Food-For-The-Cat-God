using UnityEngine;

public class CatGod : MonoBehaviour
{
    public enum Mood
    {
        Normal = 0,
        Happy = 1,
        Annoyed = 2
    }

    FoodManager foodManager;

    void Start()
    {
        foodManager = GameObject.Find("GameManager").GetComponent<FoodManager>();
    }


    public void OnIntroductionTextComplete()
    {
        FoodCheck[] checks = foodManager.GenerateFoodChecks(6);
        foreach(FoodCheck check in checks)
        {
            check.Food.gameObject.SetActive(true);
        }
    }

    void OnDrop(GameObject droppedObject)
    {
        Food food = droppedObject.GetComponent<Food>();
        Debug.Log(food.Name);
    }
}
