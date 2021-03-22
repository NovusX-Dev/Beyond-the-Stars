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

    Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y <= yRespawnPos)
        {
            float randomX = Random.Range(-8f, 8.1f);
            transform.position = new Vector3(randomX, yNewPos, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(scoreAmount);
            }

            Destroy(this.gameObject);
        }

        else if(other.CompareTag("Player"))
        {
            //damage player
            if(_player != null)
            {
                _player.DamagePlayer(attackPower);
            }

            Destroy(this.gameObject);
        }
    }

}
