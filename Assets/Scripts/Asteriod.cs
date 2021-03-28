using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] GameObject explosion = null;

    SpawnManager _spawnManager;
    WaveManager _wavesManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _wavesManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 15)* rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            _wavesManager.StartWaves();
            Destroy(gameObject);
        }
        
    }
}
