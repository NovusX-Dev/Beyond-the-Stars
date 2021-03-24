using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] TextMeshProUGUI restartLevelText;
    [SerializeField] TextMeshProUGUI ammotText;

    [Header("Other")]
    [SerializeField] GameObject ammoWarning;
    [SerializeField] AudioClip outOfAmmoClip;

    [Header("Lives")]
    [SerializeField] Image livesDisplay;
    [SerializeField] Sprite[] livesSprites = null;

    GameManager _gameManager;
    AudioSource _audioSource;

    private bool _playedNoAmmoClip = false;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();
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

    public void UpdateAmmoUI(int ammo)
    {
        ammotText.text = ammo.ToString();
        if(ammo > 1 )
        {
            _playedNoAmmoClip = false;
            ammoWarning.SetActive(false);
        }
        else if(ammo == 0)
        {
            ammoWarning.SetActive(true);
            if(!_playedNoAmmoClip)
            {
                _audioSource.clip = outOfAmmoClip;
                _audioSource.PlayDelayed(0.15f);
                _playedNoAmmoClip = true;
            }
        }
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
