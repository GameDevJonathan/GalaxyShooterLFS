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
    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(SpawnEnemies());
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnEnemies()
    {
        while (_stopSpawning == false)
        {
            Vector2 spawnPos;
            spawnPos.x = Random.Range(-9f, 9f);
            spawnPos.y = 6;
            GameObject _clone = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            _clone.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
