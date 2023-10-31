using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement & Attributes")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float speedBoost = 1f;
    [SerializeField] int maxLives = 3;
    [SerializeField] int maxAmmo = 15;
    [SerializeField] int score;

    [Header("Laser Properties")]
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] GameObject tripleShot = null;
    [SerializeField] GameObject heatSeekingLaser = null;
    [SerializeField] float laserYOffset = 0.8f;
    [SerializeField] float fireRate = 0.15f;

    [Header("PowerUp Cooldowns")]
    [SerializeField] float tripleShotCooldown = 10f;
    [SerializeField] float heatSeekingCooldown = 5f;
    [SerializeField] public float speedBoostCooldown = 5f;
    [SerializeField] float weaponFailureCooldown = 5f;

    [Header("Audio")]
    [SerializeField] AudioClip laserAudioClip;

    [Header("References")]
    [SerializeField] GameObject shields = null;
    [SerializeField] GameObject fireDamageRight = null;
    [SerializeField] GameObject fireDamageLeft = null;
    [SerializeField] GameObject deathExplosion;
    [SerializeField] PlayerShieldsDurability shieldsDurability;

    private float horiontalInput;
    private float verticalInput;
    private float _nextFire;
    private int _currentLives;
    private int _currentAmmo;

    private bool _hasAmmo = true;
    private bool _tripleSHotActive = false;
    private bool _heatSeekingActive = false;
    private bool _shieldsActive = false;
    private bool _weaponFailureActive = false;
    private bool _isBossAction = false;

    SpawnManager _spawnManager;
    WaveManager _wavesManager;
    Animator _myAnim;
    AudioSource _audioSource;
    CameraShake _camera;
    

    private void Awake()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _wavesManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();

        _myAnim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager doesn't Exist!!!");
        }
    }

    void Start()
    {
        transform.position = new Vector3(0, -2.1f, 0) ;
        _currentLives = maxLives;
        _currentAmmo = maxAmmo;
        UIManager.Instance.UpdateAmmoUI(_currentAmmo);
        score = 0;
        UIManager.Instance.UpdateScoreUI(score);

        fireDamageRight.SetActive(false);
        fireDamageLeft.SetActive(false);

    }

    void Update()
    {
        horiontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        _myAnim.SetFloat("xSpeed", horiontalInput);

        CalculateMovement();
        if (!_isBossAction)
        {
            CheckAmmoandFire();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PowerUp.PullPowerUp();
        }

        //for Debugging porpuses
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentAmmo = maxAmmo;
            _hasAmmo = true;
            UIManager.Instance.UpdateAmmoUI(_currentAmmo);
        }

    }
    private void CalculateMovement()
    {
        var direction = new Vector3(horiontalInput, verticalInput, 0);

        if(Input.GetKey(KeyCode.LeftShift))
        {
            float speedMultiplier = 2f;
            transform.Translate(direction * (moveSpeed * speedBoost * speedMultiplier) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (moveSpeed * speedBoost) * Time.deltaTime);
        }
        

        BindingMovement();
    }

    private void BindingMovement()
    {
        float xBound = 11.5f;
        float yBound = -4.5f;

        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, 0);
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3((-xBound), transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, yBound, -2), 0);
        
    }

    private void CheckAmmoandFire()
    {
        if (_currentAmmo <= (maxAmmo + _currentAmmo))
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _hasAmmo)
            {
                if (!_weaponFailureActive)
                {
                    FireLaser();
                    _currentAmmo--;
                    UIManager.Instance.UpdateAmmoUI(_currentAmmo);
                    UIManager.Instance.OnWeaponFailure(false);
                }
                
            }
        }
        if (_currentAmmo < 1)
        {
            UIManager.Instance.UpdateAmmoUI(_currentAmmo);
            _hasAmmo = false;
        }
    }

    private void FireLaser()
    {
         _nextFire = Time.time + fireRate;

             if (!_tripleSHotActive && !_heatSeekingActive)
             {
                 var laserOffset = new Vector3(laserPrefab.transform.position.x, laserYOffset, 0);
                 Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
             }
             else if (_tripleSHotActive)
             {
                 Instantiate(tripleShot, transform.position, Quaternion.identity);
             }
             else if (_heatSeekingActive) 
             {
                 var laserOffset = new Vector3(heatSeekingLaser.transform.position.x, laserYOffset, 0);
                 Instantiate(heatSeekingLaser, transform.position + laserOffset, Quaternion.identity);
             }
         _audioSource.clip = laserAudioClip;
        _audioSource.Play();
    }

    public void DamagePlayer(int amount)
    {
        if (_shieldsActive)
        {
            shieldsDurability.DamageDurability();
        }
        else
        {
            _currentLives -= amount;
            StartCoroutine(_camera.Shake(0.5f, 0.5f));
        }

        UIManager.Instance.UpdateLives(_currentLives);

        LivesVisualization();

        if (_currentLives <= 0)
        {
            _currentLives = 0;
            _spawnManager.StopSpawning();
            _wavesManager.StopSpawningEnemies();

            Instantiate(deathExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void ChangeBossAction(bool active)
    {
        _isBossAction = active;
    }
    private void LivesVisualization()
    {
        if (_currentLives == 2)
        {
            fireDamageRight.SetActive(true);
            fireDamageLeft.SetActive(false);
        }
        else if (_currentLives == 1)
        {
            fireDamageLeft.SetActive(true);
        }
        else if (_currentLives == 3)
        {
            fireDamageLeft.SetActive(false);
            fireDamageRight.SetActive(false);
        }
    }

    #region On PowerUp Enter Region
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
        UIManager.Instance.AppleThrusterUI();
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

    public void OnAmmoPowerUp(int amount)
    {
        _hasAmmo = true;
        _currentAmmo += amount;
        UIManager.Instance.UpdateAmmoUI(_currentAmmo);
    }

    public void OnHealthPowerUp(int amount)
    {
        if(_currentLives < maxLives)
        {
            _currentLives += amount;
        }
        LivesVisualization();
        UIManager.Instance.UpdateLives(_currentLives);
    }

    public void OnHeatSeekingPowerup()
    {
        StartCoroutine(HeakSeekingRoutine());
    }

    IEnumerator HeakSeekingRoutine()
    {
        _heatSeekingActive = true;
        yield return new WaitForSeconds(heatSeekingCooldown);
        _heatSeekingActive = false;
    }

    public void OnWeaponFailurePickUp()
    {
        StartCoroutine(WeaponFailureRoutine());
        UIManager.Instance.OnWeaponFailure(true);
    }

    IEnumerator WeaponFailureRoutine()
    {
        _weaponFailureActive = true;
        yield return new WaitForSeconds(weaponFailureCooldown);
        _weaponFailureActive = false;
    }

    #endregion

    public void AddScore(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScoreUI(score);
    }

    
}
