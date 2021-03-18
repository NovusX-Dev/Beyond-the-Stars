using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int maxLives = 3;

    [Header("Laser Properties")]
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] GameObject tripleShot = null;
    [SerializeField] float laserYOffset = 0.8f;
    [SerializeField] float fireRate = 0.15f;

    [Header("PowerUp Cooldowns")]
    [SerializeField] float tripleShotCooldown = 10f;

    private float _nextFire;
    private int _currentLives;
    private bool _tripleSHotActive = false;

    SpawnManager _spawnManager;

    private void Awake()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager doesn't Exist!!!");
        }
    }

    void Start()
    {
        transform.position = Vector3.zero;
        _currentLives = maxLives;
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

        transform.Translate(direction * moveSpeed * Time.deltaTime);

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
        _currentLives -= amount;

        if(_currentLives <= 0)
        {
            _spawnManager.OnPlayerDeath();

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
}
