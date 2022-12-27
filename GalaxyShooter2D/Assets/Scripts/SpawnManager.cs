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
    private GameObject _tripleShotPrefab;
    
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private float _spawnTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUp());        
    }

   
    IEnumerator SpawnEnemies()
    {
        while (_stopSpawning == false)
        {
            Vector2 spawnPos;
            spawnPos.x = Random.Range(-8f, 8f);
            spawnPos.y = 6;
            GameObject _clone = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
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
            GameObject _clone = Instantiate(_tripleShotPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
