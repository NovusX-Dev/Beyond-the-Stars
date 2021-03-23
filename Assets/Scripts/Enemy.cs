using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    [SerializeField] float yNewPos = 7f;
    [SerializeField] float yRespawnPos = -6.8f;
    [SerializeField] int attackPower = 1;
    [SerializeField] int scoreAmount = 10;
    [SerializeField] GameObject enemyLasers;
    [SerializeField] float fireNext =3f;

    private float _canFire = -1;
    private bool _isDying = false;

    Player _player;
    Animator _myAnime;
    AudioSource _audioSource;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _myAnime = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CalculateMovement();
        if(Time.time > _canFire)
        {
            fireNext = Random.Range(3f, 7f);
            _canFire = Time.time + fireNext;
            FireLasers();
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (!_isDying)
        {
            if (transform.position.y <= yRespawnPos)
            {
                float randomX = Random.Range(-8f, 8.1f);
                transform.position = new Vector3(randomX, yNewPos, 0);
            }
        }
    }

    private void FireLasers()
    {
        var laserContainer = Instantiate(enemyLasers, transform.position, Quaternion.identity);
        Laser[] lasers = laserContainer.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {
            if(!other.GetComponent<Laser>().ISEnemyLaser())
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(scoreAmount);
                }

                OnEnemyDeath();
            }
        }

        else if(other.CompareTag("Player"))
        {
            //damage player
            if(_player != null)
            {
                _player.DamagePlayer(attackPower);
            }

            OnEnemyDeath();
        }
    }

    private void OnEnemyDeath()
    {
        _isDying = true;
        _myAnime.SetTrigger("die");
        speed = 2f;
        var collider = GetComponent<Collider2D>();
        collider.enabled = false;
        _audioSource.Play();
        Destroy(gameObject, 2.75f);
    }
}
