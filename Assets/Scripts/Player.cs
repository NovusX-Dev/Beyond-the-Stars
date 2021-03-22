using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float speedBoost = 1f;
    [SerializeField] int maxLives = 3;
    [SerializeField] int score;

    [Header("Laser Properties")]
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] GameObject tripleShot = null;
    [SerializeField] float laserYOffset = 0.8f;
    [SerializeField] float fireRate = 0.15f;

    [Header("PowerUp Cooldowns")]
    [SerializeField] float tripleShotCooldown = 10f;
    [SerializeField] float speedBoostCooldown = 5f;

    [Header("References")]
    [SerializeField] GameObject shields = null;
    [SerializeField] GameObject fireDamageRight = null;
    [SerializeField] GameObject fireDamageLeft = null;

    private float _nextFire;
    private int _currentLives;
    private bool _tripleSHotActive = false;
    private bool _shieldsActive = false;

    SpawnManager _spawnManager;
    Animator _myAnim;
    UIManager _UI;

    private void Awake()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _UI = GameObject.Find("HUD UI").GetComponent<UIManager>();

        _myAnim = GetComponent<Animator>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager doesn't Exist!!!");
        }
        if (_UI == null)
        {
            Debug.LogError("UI Doesn't Exist!!!");
        }
    }

    void Start()
    {
        transform.position = Vector3.zero;
        _currentLives = maxLives;
        score = 0;

        fireDamageRight.SetActive(false);
        fireDamageLeft.SetActive(false);
    }

    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        float horiontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        var direction = new Vector3(horiontalInput, verticalInput, 0);

        transform.Translate(direction * (moveSpeed * speedBoost) * Time.deltaTime);

        BindingMovement();
    }

    private void BindingMovement()
    {
        float xBound = 11.5f;
        float yBound = -4f;

        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, 0);
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3((-xBound), transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, yBound, 0), 0);
    }

    private void FireLaser()
    {
         _nextFire = Time.time + fireRate;

         if(!_tripleSHotActive)
        {
            var laserOffset = new Vector3(laserPrefab.transform.position.x, laserYOffset, 0);
            Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
        }
        else
        {
            Instantiate(tripleShot, transform.position, Quaternion.identity);
        }
        
    }

    public void DamagePlayer(int amount)
    {
        if(_shieldsActive)
        {
            OnShieldPowerUp(false);
        }
        else
        {
            _currentLives -= amount;
        }

        _UI.UpdateLives(_currentLives);
        
        if(_currentLives == 2)
        {
            fireDamageRight.SetActive(true);
        }
        else if(_currentLives == 1)
        {
            fireDamageLeft.SetActive(true);
        }

        if(_currentLives <= 0)
        {
            _spawnManager.OnPlayerDeath();

            fireDamageRight.SetActive(false);
            fireDamageLeft.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void ONTripleShotEnter()
    {
        StartCoroutine(TripleShotCooldownRoutine());
    }

    IEnumerator TripleShotCooldownRoutine()
    {
        _tripleSHotActive = true;
        yield return new WaitForSeconds(tripleShotCooldown);
        _tripleSHotActive = false;
    }

    public void OnSpeedBoostEnter(float speedBoostPowerup)
    {
        speedBoost = speedBoostPowerup;
        StartCoroutine(SpeedBoostRoutine());
    }

    IEnumerator SpeedBoostRoutine()
    {
        yield return new WaitForSeconds(speedBoostCooldown);
        speedBoost = 1f;
    }

    public void OnShieldPowerUp(bool active)
    {
        _shieldsActive = active;
        shields.SetActive(active);
    }

    public void AddScore(int amount)
    {
        score += amount;
        _UI.UpdateScoreUI(score);
    }

    
}
