using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] TextMeshProUGUI restartLevelText;
    [SerializeField] Image livesDisplay;
    [SerializeField] Sprite[] livesSprites = null;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreText.text = 0.ToString();
        livesDisplay.sprite = livesSprites[3];
        gameoverText.gameObject.SetActive(false);
        restartLevelText.gameObject.SetActive(false);

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
    }


    public void UpdateScoreUI(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        
        if(currentLives < 0)
        {
            currentLives = 0;
            livesDisplay.sprite = livesSprites[currentLives];
        }

        livesDisplay.sprite = livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        gameoverText.gameObject.SetActive(true);
        restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            gameoverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.35f);
            gameoverText.text = "";
            yield return new WaitForSeconds(0.35f);
        }
    }

    
}
