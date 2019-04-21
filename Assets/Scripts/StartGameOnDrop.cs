﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameOnDrop : MonoBehaviour
{
    [SerializeField]
    Sprite eatingSprite;

    void OnDrop(GameObject droppedObject)
    {
        GetComponent<Image>().sprite = eatingSprite;
        GameObject.Find("GameManager").GetComponent<FadeManager>().StartFade(() => SceneManager.LoadScene("MainScene"));
    }
}
