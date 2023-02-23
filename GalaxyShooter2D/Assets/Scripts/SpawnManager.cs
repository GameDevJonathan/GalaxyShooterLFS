using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    
    [SerializeField]
    private bool _stopSpawning = false;
    
    [SerializeField]
    private float _spawnTime = 5f;

    [SerializeField]
    public GameObject[] _powerUps;
    // Start is called before the first frame update
      
    IEnumerator SpawnEnemies()
    {
        while (_stopSpawning == false)
        {
            float randomTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(randomTime);
            Vector2 spawnPos;
            spawnPos.x = Random.Range(-8f, 8f);
            spawnPos.y = 6;
            GameObject _clone = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            _clone.transform.name = "Enemy";
            _clone.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnTime);
        }
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
            int randomPowerUp = Random.Range(0,_powerUps.Length);
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
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUp());
    }
}
