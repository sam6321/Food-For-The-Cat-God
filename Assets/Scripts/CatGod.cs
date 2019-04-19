using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CatGod : MonoBehaviour
{
    public enum Mood
    {
        Normal = 0,
        Happy = 1,
        Annoyed = 2
    }

    [SerializeField]
    GameObject speechPanel;

    [SerializeField]
    Vector2[] speechPanelOffsets;

    [SerializeField]
    Favour favour;

    [SerializeField]
    float favourPunchPositive = 20.0f;

    [SerializeField]
    float favourPunchNegative = 30.0f;

    Coroutine speechCoroutine;

    Animator animator;

    FoodManager foodManager;
    List<Food> allFood;
    List<Food> remainingFood;
    Food.FoodCheck currentCheck;
    int correct;
    int wrong;
    bool disableDrop = false;

    void Start()
    {
        foodManager = GameObject.Find("GameManager").GetComponent<FoodManager>();
        animator = GetComponent<Animator>();
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
        favour.ResetFavour();
        NextCheck();
    }

    void NextCheck()
    {
        if(remainingFood.Count == 0)
        {
            currentCheck = null;
            disableDrop = true;
            favour.Enabled = false;
            Say(Mood.Normal, "All Done: " + correct + " correct and " + wrong + " wrong");
            // Clean up any remaining foods that weren't used (cat and dog food do not generate food checks and are not mandatory)
            foreach (Food food in allFood)
            {
                food.gameObject.SetActive(false);
            }
        }
        else
        {
            // Pick a random food from the list of remaining food and use it to generate a food check.
            Food food = Utils.RandomElement(remainingFood);
            currentCheck = food.GetRandomFoodCheck();

            Say(Mood.Normal, currentCheck.GetCheckString(), 0.0f, () => {
                disableDrop = false;
                favour.Enabled = true;
            });
        }
    }

    IEnumerator SpeechCoroutine(string text, float endDelay=0.0f, Action onComplete=null)
    {
        Vector2 worldPosition = (Vector2)transform.position + Utils.RandomElement(speechPanelOffsets);
        speechPanel.transform.position = Camera.main.WorldToScreenPoint(worldPosition);

        Text speechText = speechPanel.GetComponentInChildren<Text>();
        speechPanel.SetActive(true);

        animator.SetBool("talking", true);
        speechText.text = "";
        foreach (char character in text)
        {
            speechText.text += character;
            yield return new WaitForSeconds(0.01f);
        }
        animator.SetBool("talking", false);

        if (endDelay > 0.0f)
        {
            yield return new WaitForSeconds(endDelay);
        }

        if(onComplete != null){
            onComplete.Invoke();
        }
        speechCoroutine = null;
    }

    void Say(Mood mood, string text, float endDelay=0.0f, Action onComplete=null)
    {
        // Clear current text appearing (if any)
        if(speechCoroutine != null)
        {
            StopCoroutine(speechCoroutine);
        }

        animator.SetInteger("mood", (int)mood);
        speechCoroutine = StartCoroutine(SpeechCoroutine(text, endDelay, onComplete));
    }

    public void OnFavourHitZero()
    {

    }

    void OnDrop(GameObject droppedObject)
    {
        if(disableDrop)
        {
            return;
        }

        Food food = droppedObject.GetComponent<Food>();
        if(currentCheck != null && food)
        {
            if(currentCheck.Check(food))
            {
                food.gameObject.SetActive(false);
                if (remainingFood.Contains(food))
                {
                    // The dropped food was in the remaining list
                    remainingFood.Remove(food);
                }
                else
                {
                    // The dropped food wasn't in the remaining list, so just clear one item from
                    // the remaining food list.
                    remainingFood.RemoveAt(0);
                }
                correct++;

                disableDrop = true;
                favour.FavourPunch(favourPunchPositive);
                Say(Mood.Happy, "Yes, that's what I want!", 1.0f, () => NextCheck());
            }
            else
            {
                wrong++;

                disableDrop = true;
                favour.FavourPunch(favourPunchNegative);
                Say(Mood.Annoyed, "No, ew, I don't want this!!", 1.0f, () => NextCheck());
            }
        }
    }
}
