using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int sceneToLoad = 1;

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
