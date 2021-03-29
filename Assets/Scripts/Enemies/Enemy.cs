using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected int attackPower = 1;
    [SerializeField] protected int scoreAmount = 10;

    [Header("Movement")]
    [SerializeField] protected float yNewPos = 7f;
    [SerializeField] protected float yRespawnPos = -6.8f;
    
    [Header("Attack")]
    [SerializeField] protected GameObject enemyLasers = null;
    [SerializeField] protected bool hasWeaponSystem = false;
    [SerializeField] protected float fireNext =3f;

    [Header("Defence")]
    [SerializeField] protected GameObject _shields;
    [Range(0f,0.5f)] [SerializeField] protected float _shieldProbability = .25f;
    
    protected float _canFire = -1;
    protected bool _isDying = false;
    protected bool _hasShields = false;
    protected float _currentProbability;

    protected Player _player;
    protected Animator _myAnime;
    protected AudioSource _audioSource;
    protected WaveManager _wavesManager;

    protected virtual void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _myAnime = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _wavesManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();

        _currentProbability = Random.Range(0f, 0.5f);
        ShieldProbability();
    }

    protected virtual void Update()
    {
        CalculateMovement();

        if(hasWeaponSystem)
        {
            if (Time.time > _canFire)
            {
                fireNext = Random.Range(3f, 7f);
                _canFire = Time.time + fireNext;
                FireLasers();
            }
        }
    }

    protected void CalculateMovement()
    {
        MoveDirection();

        if (!_isDying)
        {
            if (transform.position.y <= yRespawnPos)
            {
                float randomX = Random.Range(-8f, 8.1f);
                transform.position = new Vector3(randomX, yNewPos, 0);
            }
        }
    }

    protected virtual void MoveDirection()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    protected void FireLasers()
    {
        var laserContainer = Instantiate(enemyLasers, transform.position, Quaternion.identity);
        Laser[] lasers = laserContainer.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    protected void ShieldProbability()
    {
        if (_currentProbability <= _shieldProbability)
        {
            _hasShields = true;
            _shields.gameObject.SetActive(true);
        }
        else
        {
            _hasShields = false;
            _shields.gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
   
            if (other.CompareTag("Laser"))
            {
                if (!other.GetComponent<Laser>().ISEnemyLaser())
                {
                    Destroy(other.gameObject);
                    if (_player != null)
                    {
                        _player.AddScore(scoreAmount);
                    }

                    OnEnemyDeath();
                }
            }

            else if (other.CompareTag("Player"))
            {
                //damage player
                if (_player != null)
                {
                    _player.DamagePlayer(attackPower);
                }

                OnEnemyDeath();
            }

            else if (other.CompareTag("Seeking Laser"))
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(scoreAmount);
                }

                OnEnemyDeath();
            }
    }

    protected void OnEnemyDeath()
    {
        if (_hasShields)
        {
            _shields.gameObject.SetActive(false);
            _hasShields = false;
        }
        else
        {
            _isDying = true;
            _myAnime.SetTrigger("die");
            speed = 2f;
            var collider = GetComponent<Collider2D>();
            collider.enabled = false;
            _audioSource.Play();
            WaveManager.enemiesRemaining--;
            UIManager.Instance.UpdateEnemiesRemainingUI(WaveManager.enemiesRemaining);
            Destroy(gameObject, 2.75f);
        }
    }

    public bool GetIsDying()
    {
        return _isDying;
    }
    
}
