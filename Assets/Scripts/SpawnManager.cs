using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemiesContainer;
    [SerializeField] float _spawnTime = 5f;

    bool _canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_canSpawn)
        {
            var posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var enemy = Instantiate(_enemyPrefab, posToSpawn , Quaternion.identity);
            enemy.transform.parent = _enemiesContainer.transform;

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    public void OnPlayerDeath()
    {
        _canSpawn = false;
    }
}
