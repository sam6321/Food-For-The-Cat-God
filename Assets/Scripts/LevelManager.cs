using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private CatGod catGod;

    [SerializeField]
    private RPGTalk dialogueIntroduction;

    [SerializeField]
    private RPGTalk dialogueBetweenLevels;

    [SerializeField]
    private RPGTalk dialogueFail;

    [SerializeField]
    private Favour favour;

    [SerializeField]
    private AudioClip[] successSounds;

    [SerializeField]
    private AudioClip[] failSounds;

    private FoodManager foodManager;
    private AudioSource audioSource;

    private List<Food> currentFood;

    private int totalCorrect = 0;
    private int totalWrong = 0;
    private int totalLevels = 0;

    private int foodsToGenerate = 2;

    void Start()
    {
        foodManager = GetComponent<FoodManager>();
        audioSource = GetComponent<AudioSource>();
        OnStart();
    }

    public void OnStart()
    {
        // Show introduction
        StartDialogue(dialogueIntroduction);
    }

    public void OnDialogueIntroductionComplete()
    {
        StopDialogue(dialogueIntroduction);
        // Start first level
        CreateLevel();
    }

    public void OnFailLevel(int correct, int wrong)
    {
        // Show cat god fail text
        totalCorrect += correct;
        totalWrong += wrong;

        dialogueFail.variables = new RPGTALK.Helper.RPGTalkVariable[]
        {
            new RPGTALK.Helper.RPGTalkVariable()
            {
                variableName = "foodCorrectCount",
                variableValue = totalCorrect.ToString()
            },

            new RPGTALK.Helper.RPGTalkVariable()
            {
                variableName = "foodWrongCount",
                variableValue = totalWrong.ToString()
            },

            new RPGTALK.Helper.RPGTalkVariable()
            {
                variableName = "levelCount",
                variableValue = totalLevels.ToString()
            }
        };

        Utils.PlayRandomSound(audioSource, failSounds);
        StartDialogue(dialogueFail);
    }

    public void OnDialogueFailComplete()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnCompleteLevel(int correct, int wrong)
    {
        // Show LevelComplete then go to the next level.
        totalCorrect += correct;
        totalWrong += wrong;
        totalLevels++;
        foodsToGenerate = Mathf.Min(foodsToGenerate + 1, foodManager.MaxFood);

        // Clean up any remaining foods that weren't used (cat and dog food do not generate food checks and are not mandatory)
        foodManager.CleanupFoods(currentFood);

        dialogueBetweenLevels.variables = new RPGTALK.Helper.RPGTalkVariable[]
        {
            new RPGTALK.Helper.RPGTalkVariable()
            {
                variableName = "foodCorrectCount",
                variableValue = totalCorrect.ToString()
            },

            new RPGTALK.Helper.RPGTalkVariable()
            {
                variableName = "foodWrongCount",
                variableValue = totalWrong.ToString()
            },

            new RPGTALK.Helper.RPGTalkVariable()
            {
                variableName = "levelCount",
                variableValue = totalLevels.ToString()
            }
        };

        Utils.PlayRandomSound(audioSource, successSounds);
        StartDialogue(dialogueBetweenLevels);
    }

    public void OnDialogueBetweenLevelsComplete()
    {
        StopDialogue(dialogueBetweenLevels);

        // TODO: Up difficulty for next level

        CreateLevel();
    }

    void StartDialogue(RPGTalk dialogue)
    {
        // RPGTalk doesn't like multiple RPGTalk instances using the same
        // dialogue instance. So we need to ensure all other dialogues are
        // stopped before starting this new one.
        StopDialogue(dialogueIntroduction);
        StopDialogue(dialogueBetweenLevels);
        StopDialogue(dialogueIntroduction);

        dialogue.gameObject.SetActive(true);
        dialogue.NewTalk();
    }

    void StopDialogue(RPGTalk dialogue)
    {
        dialogue.gameObject.SetActive(false);
    }

    void CreateLevel()
    {
        currentFood = foodManager.GenerateFoods(foodsToGenerate);
        favour.DecreasePerSecond += 0.5f;
        favour.ResetFavour();
        catGod.OnStartLevel(currentFood);
    }
}
