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
    private GameObject[] _playerPowerUps;

    [SerializeField]
    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f); // delays start of enemy spawns
        while (_stopSpawning == false)
        {
            Vector3 pxToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, pxToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 pxToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_playerPowerUps[randomPowerUp], pxToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6f, 10f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
