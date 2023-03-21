using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private bool _stopSpawning = false;

    [SerializeField]
    public GameObject[] _powerUps;

    [Header("Wave Variables")]
    [SerializeField]
    private int _waveIndex = 0; //index of current wave
    [SerializeField]
    private float _timeBetweenWaves = 5f; // default time before each wave starts
    [SerializeField]
    private float _waveCountdown = 0f; //time before each wave starts
    [SerializeField]
    private int _enemiesKilled = 0; // used to keep track of how many enemies were killed in the wave
    [SerializeField]
    private enum SpawnState { Spawning, Waiting, Counting, Complete }; // state machine for wave system
    [SerializeField]
    private SpawnState state = SpawnState.Counting; // default state
    [SerializeField]
    private Transform[] _spawnPoints; // where I want to spawn my enemies

    [Header("Enemy Waves")]
    public Wave[] waves; // wave class
    [SerializeField]
    private Coroutine _enemyWaves; // when and how to spawn enemies

    // Start is called before the first frame update
    private void Start()
    {
        _waveCountdown = _timeBetweenWaves;

    }

    private void Update()
    {
        if (_waveIndex >= waves.Length) return;

        


        if (state == SpawnState.Waiting)
        {
            
            if (!EnemiesAlive())
            {
                //Begin a new round
                WaveCompleted();

            }
            else
            {
                return;
            }
        }

        if (_waveCountdown <= 0 && _waveIndex < waves.Length)
        {
            if (state != SpawnState.Spawning)
            {
                if (_enemyWaves == null)
                    _enemyWaves = StartCoroutine(EnemyWave(waves[_waveIndex]));
            }
        }
        else
        {
            _waveCountdown -= Time.deltaTime;
        }
    }

    private bool EnemiesAlive()
    {
        if (_enemiesKilled == waves[_waveIndex].enemy.Length)
        {
            return false;
        }
        return true;
    }

    IEnumerator EnemyWave(Wave _wave)
    {
        state = SpawnState.Spawning; //change state to spawning
        //Spawn
        for (int i = 0; i < _wave.enemy.Length; i++) //loop through all enemies in the wave
        {
            SpawnEnemy(_wave.enemy[i]);
            yield return new WaitForSeconds(_wave.spawnRate); //balance spawning time.
        }
        state = SpawnState.Waiting; // now wait till all enemies are killed
        _enemyWaves = null; // reset coroutine.
        yield break; //exit coroutine. 
    }

    void SpawnEnemy(Transform enemy)
    {
        int randomPoint = Random.Range(0, _spawnPoints.Length);
        Debug.Log("Spawning Enemy: " + enemy.transform.name);
        Transform _clone = Instantiate(enemy, _spawnPoints[randomPoint].position, Quaternion.identity);
        _clone.transform.name = "Enemy";
        _clone.transform.parent = _enemyContainer.transform;

    }


    public void KillCount()
    {
        _enemiesKilled++;
    }

    private void WaveCompleted()
    {
        state = SpawnState.Counting;
        _waveCountdown = _timeBetweenWaves;
        _enemiesKilled = 0;
        _waveIndex++;
    }



    IEnumerator SpawnPowerUp()
    {
        while (_stopSpawning == false)
        {
            float randomTime = Random.Range(3f, 7f);
            yield return new WaitForSeconds(randomTime);
            Vector2 spawnPos;
            spawnPos.x = Random.Range(-8f, 8f);
            spawnPos.y = 7;
            int randomPowerUp = Random.Range(0, _powerUps.Length);
            Instantiate(_powerUps[randomPowerUp], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void StartSpawning()
    {
        Debug.Log("SpawnManager::StartSpawing() Called");
        //StartCoroutine(SpawnEnemies());
        //StartCoroutine(SpawnPowerUp());
    }

    public Vector3 RandomPoint()
    {
        int randomPoint = Random.Range(0, _spawnPoints.Length);

        return _spawnPoints[randomPoint].position;
    }
}

[System.Serializable]
public class Wave
{
    public string currentWave;
    public Transform[] enemy;
    public float spawnRate;

}