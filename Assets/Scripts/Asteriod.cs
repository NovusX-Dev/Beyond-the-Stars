using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] GameObject explosion = null;
    
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 15)* rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
