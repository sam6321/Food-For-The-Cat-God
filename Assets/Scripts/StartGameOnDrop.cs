using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameOnDrop : MonoBehaviour
{
    void OnDrop(GameObject droppedObject)
    {
        GameObject.Find("GameManager").GetComponent<FadeManager>().StartFade(() => SceneManager.LoadScene("MainScene"));
    }
}
