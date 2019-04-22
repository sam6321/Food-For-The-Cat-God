using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameOnDrop : MonoBehaviour
{
    [SerializeField]
    Sprite eatingSprite;

    [SerializeField]
    private AudioClip[] onDropSounds;

    void OnDrop(GameObject droppedObject)
    {
        GetComponent<Image>().sprite = eatingSprite;
        Utils.PlayRandomSound(GetComponent<AudioSource>(), onDropSounds);
        GameObject.Find("GameManager").GetComponent<FadeManager>().StartFade(() => SceneManager.LoadScene("MainScene"));
    }
}
