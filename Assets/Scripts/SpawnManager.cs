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

    [Header("Ammo Health")]
    [SerializeField] GameObject[] _refillPowerups;
    [SerializeField] float _minRefillTime = 5f, _maxRefillTime = 10f;

    [Header("Rare PowerUps")]
    [SerializeField] GameObject[] _rarePowerUps;
    [SerializeField] float _minRareTime = 10f, _maxRareTime = 15f;

    bool _canSpawn = true;

    public void StartSpawning()
    {
       // StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRefillRoutine());
        StartCoroutine(RarePowerUps());
    }


    /*public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            var posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var enemy = Instantiate(_enemyPrefab, posToSpawn , Quaternion.identity);
            enemy.transform.parent = _enemiesContainer.transform;

            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }*/

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

    IEnumerator SpawnRefillRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            yield return new WaitForSeconds(Random.Range(_minRefillTime, _maxRefillTime));

            var posToSpawn = new Vector3(Random.Range(-6f, 7f), 7f, 0);
            int randomPowerup = Random.Range(0, 2);
            Instantiate(_refillPowerups[randomPowerup], posToSpawn, Quaternion.identity);
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
