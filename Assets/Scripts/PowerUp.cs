using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float speedBoostPowerup = 1.5f;
    [SerializeField] int ammoRefillAmount = 15;
    [SerializeField] int healAmount = 1;
    [SerializeField] AudioClip powerupClip;

    [Range(0,6)] [Tooltip("tripleshot = 0, speed =1, shield = 2, ammo = 3, health = 4, heatseek = 5, weaponFailure = 6")]
    [SerializeField] int powerUpID;


    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if(transform.position.y < -6.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                AudioSource.PlayClipAtPoint(powerupClip, transform.position);

                switch (powerUpID)
                {
                    case 0: 
                        player.ONTripleShotEnter();
                        break;

                    case 1: 
                        player.OnSpeedBoostEnter(speedBoostPowerup);
                        break;

                    case 2:
                        player.OnShieldPowerUp(true);
                        break;
                    case 3:
                        player.OnAmmoPowerUp(ammoRefillAmount);
                        break;
                    case 4:
                        player.OnHealthPowerUp(healAmount);
                        break;
                    case 5:
                        player.OnHeatSeekingPowerup();
                        break;
                    case 6: player.OnWeaponFailurePickUp();
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
