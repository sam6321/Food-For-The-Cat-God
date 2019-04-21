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
    LevelManager levelManager;

    [SerializeField]
    float favourPunchPositive = 20.0f;

    [SerializeField]
    float favourPunchNegative = 30.0f;

    Coroutine speechCoroutine;

    Animator animator;

    List<Food> allFood;
    List<Food> remainingFood;
    Food.FoodCheck currentCheck;
    int correct;
    int wrong;
    bool disableDrop = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnStartLevel(List<Food> foods)
    {
        allFood = foods;
        correct = 0;
        wrong = 0;
        remainingFood = allFood.Where(food => food.ShouldGenerateFoodCheck()).ToList();
        NextCheck();
    }

    public void OnFavourHitZero()
    {
        if (speechCoroutine != null)
        {
            StopCoroutine(speechCoroutine);
        }

        speechPanel.SetActive(false);
        disableDrop = true;
        levelManager.OnFailLevel(correct, wrong);
    }

    void NextCheck()
    {
        if(remainingFood.Count == 0)
        {
            currentCheck = null;
            disableDrop = true;
            favour.Enabled = false;

            Say(Mood.Happy, "Yay! All done!", 1.0f, () =>
            {
                speechPanel.SetActive(false);
                levelManager.OnCompleteLevel(correct, wrong);
            });
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
                if(food.allAudio.Count != 0)
                {
                    GetComponent<AudioSource>().clip = food.allAudio[UnityEngine.Random.Range(0, food.allAudio.Count)];
                    GetComponent<AudioSource>().Play();
                }
                favour.FavourPunch(favourPunchPositive);
                Say(Mood.Happy, "Yes, that's what I want!", 0.5f, () => NextCheck());
            }
            else
            {
                wrong++;

                disableDrop = true;
                favour.FavourPunch(favourPunchNegative);
                Say(Mood.Annoyed, "No, ew, I don't want this!!", 0.5f, () => NextCheck());
            }
        }
    }
}
