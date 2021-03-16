using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    [SerializeField] float yNewPos = 7f;
    [SerializeField] float yRespawnPos = -6.8f;
    [SerializeField] int attackPower = 1;

    void Start()
    {
        
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        else if(other.CompareTag("Player"))
        {
            //damage player
            var player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                player.DamagePlayer(attackPower);
            }

            Destroy(this.gameObject);
        }
    }
}
