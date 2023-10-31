using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            RestartLevel();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.QuitPanel(true);
            Time.timeScale = 0;
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameEnd()
    {
        _isGameOver = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1;
        UIManager.Instance.QuitPanel(false);
    }
}
