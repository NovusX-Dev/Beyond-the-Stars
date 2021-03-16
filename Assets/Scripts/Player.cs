using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] float laserYOffset = 0.8f;
    [SerializeField] float fireRate = 0.15f;

    private float _nextFire;

    void Start()
    {
        transform.position = Vector3.zero;
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

         var laserOffset = new Vector3(laserPrefab.transform.position.x, laserYOffset, 0);
         Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
        
    }
}
