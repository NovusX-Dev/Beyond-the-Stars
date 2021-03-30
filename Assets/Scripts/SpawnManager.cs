using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /*[Header("Enemies")]
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemiesContainer;
    [SerializeField] float _enemySpawnTime = 5f;*/

    [Header("PowerUps")]
    [SerializeField] GameObject[] _powerUps;
    [SerializeField] float _minSpawnTime = 7f, _maxSpawnTime = 15f;

    [Header("Health")]
    [SerializeField] GameObject[] _healthPowerups;
    [SerializeField] float _minHTime = 5f, _maxHTime = 10f;
    
    [Header("Ammo")]
    [SerializeField] GameObject[] _ammoPowerups;
    [SerializeField] float _minATime = 5f, _maxATime = 10f;

    [Header("Rare PowerUps")]
    [SerializeField] GameObject[] _rarePowerUps;
    [SerializeField] float _minRareTime = 10f, _maxRareTime = 15f;

    bool _canSpawn = true;

    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnHealthRoutine());
        StartCoroutine(SpawnAmmoRoutine());
        StartCoroutine(RarePowerUps());
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));

            var posToSpawn = new Vector3(Random.Range(-6f, 7f), 7f, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }

    IEnumerator SpawnHealthRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            yield return new WaitForSeconds(Random.Range(_minHTime, _maxHTime));

            var posToSpawn = new Vector3(Random.Range(-6f, 7f), 7f, 0);
            int randomPowerup = Random.Range(0, 1);
            Instantiate(_healthPowerups[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }
    
    IEnumerator SpawnAmmoRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            yield return new WaitForSeconds(Random.Range(_minATime, _maxATime));

            var posToSpawn = new Vector3(Random.Range(-6f, 7f), 7f, 0);
            int randomPowerup = Random.Range(0, 1);
            Instantiate(_ammoPowerups[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }

    IEnumerator RarePowerUps()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            yield return new WaitForSeconds(Random.Range(_minRareTime, _maxRareTime));

            var posToSpawn = new Vector3(Random.Range(-6f, 7f), 7f, 0);
            int randomPowerup = 0;
            Instantiate(_rarePowerUps[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }

    public void StopSpawning()
    {
        _canSpawn = false;
    }
}
