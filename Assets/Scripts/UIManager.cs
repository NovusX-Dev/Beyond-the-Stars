using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The UI Manager is null");
            return _instance;
        }
    }
    #region Serialized Variables
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI restartLevelText;
    [SerializeField] TextMeshProUGUI ammotText;
    [SerializeField] TextMeshProUGUI wavesText;
    [SerializeField] TextMeshProUGUI enemiesRemainingText;
    [SerializeField] TextMeshProUGUI waveStartText;

    [Header("Other")]
    [SerializeField] GameObject ammoWarning;
    [SerializeField] AudioClip outOfAmmoClip;

    [Header("Lives")]
    [SerializeField] Image livesDisplay;
    [SerializeField] Sprite[] livesSprites = null;

    [Header("Thruster Cooldown")]
    [SerializeField] Image thrusterCDImage;

    #endregion

    GameManager _gameManager;
    AudioSource _audioSource;

    private bool _playedNoAmmoClip = false;
    //Variables for thrustercooldown
    private bool _isCoolingDown = false;
    private float _coolDownTime;
    private float _coolDownTimer = 0f;

    private void Awake()
    {
        _instance = this;
    }

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

        thrusterCDImage.gameObject.SetActive(false);
        _coolDownTime = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().speedBoostCooldown;
    }

    private void Update()
    {
        if(_isCoolingDown)
        {
            AppleThrusterCoolDown();
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
        _gameManager.GameEnd();
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

    public void WinSequence()
    {
        _gameManager.GameEnd();
        winText.gameObject.SetActive(true);
        restartLevelText.gameObject.SetActive(true);
    }

    private void AppleThrusterCoolDown()
    {
        _coolDownTimer -= Time.deltaTime;

        if (_coolDownTimer < 0)
        {
            _isCoolingDown = false;
            thrusterCDImage.gameObject.SetActive(false);
        }
        else
        {
            thrusterCDImage.fillAmount = _coolDownTimer / _coolDownTime;
        }
    }

    public void AppleThrusterUI()
    {

            _isCoolingDown = true;
            thrusterCDImage.gameObject.SetActive(true);

            _coolDownTimer = _coolDownTime;
    }

    public void UpdateWavesUI(int waves)
    {
        wavesText.text = waves.ToString();
    }

    public void UpdateEnemiesRemainingUI(int enemies)
    {
        enemiesRemainingText.text = enemies.ToString();
    }

    public void DisableWaveStartText()
    {
        waveStartText.gameObject.SetActive(false);
    }

    public void EnableWaveStartText(string text)
    {
        waveStartText.gameObject.SetActive(true);
        waveStartText.text = text;
    }
    
}
