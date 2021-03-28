using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int enemyCountPerWave;
        public Enemy[] enemyTypesToSpawn;
        public bool spawnBoss = false;
        public float enemySpawnTime = 5f;
        public AudioClip waveStartClip = null;
    }
    public static int enemiesRemaining = 0;

    [SerializeField] GameObject _enemiesContainer;

    [SerializeField] Wave[] _waves;
    

    private int _currentWave;

    bool _canSpawn = true;

    SpawnManager _spawnManager;

    private void Awake()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    private void Start()
    {
        UIManager.Instance.UpdateWavesUI(_waves.Length);
        UIManager.Instance.UpdateEnemiesRemainingUI(0);
    }

    public void StartWaves()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            //for (int i = 0; i < _waves.Length; i++)
            for (int i = _waves.Length - 1; i >= 0; i--)
            {
                UIManager.Instance.UpdateWavesUI(i);
                _currentWave = i;
                enemiesRemaining = _waves[i].enemyCountPerWave;

                UIManager.Instance.UpdateEnemiesRemainingUI(enemiesRemaining);
                UIManager.Instance.EnableWaveStartText(_waves[i].waveName);
                AudioSource.PlayClipAtPoint(_waves[i].waveStartClip,  Camera.main.transform.position);
                yield return new WaitForSeconds(2f);
                UIManager.Instance.DisableWaveStartText();

                while (enemiesRemaining > 0)
                {
                    UIManager.Instance.UpdateEnemiesRemainingUI(enemiesRemaining);
                    
                    if (_waves[i].enemyCountPerWave > 0)
                    {
                        var posToSpawn = new Vector3(Random.Range(-6f, 6f), 7f, 0);
                        var enemies = _waves[i].enemyTypesToSpawn;
                        var enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], posToSpawn, Quaternion.identity);
                        enemy.transform.parent = _enemiesContainer.transform;
                    }

                    _waves[i].enemyCountPerWave--;

                    yield return new WaitForSeconds(_waves[i].enemySpawnTime);
                }

                if (_currentWave == 0 && enemiesRemaining == 0)
                {
                    UIManager.Instance.WinSequence();
                    _spawnManager.StopSpawning();
                    StopSpawningEnemies();
                }
                yield return new WaitForSeconds(2f);
            }
        }
    }

    public void StopSpawningEnemies()
    {
        _canSpawn = false;
    }
}
