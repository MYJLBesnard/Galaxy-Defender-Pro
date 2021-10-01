using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _dodgingEnemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _playerPowerUps;
    [SerializeField] private GameObject _enemyOnePrefab;

    [SerializeField] private int _shipsInWave = 10;
    public int totalEnemyShipsDestroyed = 0;
    public int waveCurrent = 0;
    public int enemyType = 1;


    [SerializeField] private bool _stopSpawning = false;

    private float _xPos;

    public void EnemyShipsDestroyedCounter()
    {
        totalEnemyShipsDestroyed++;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f); // delays start of enemy spawns
        while (_stopSpawning == false)

        {
            _xPos = Random.Range(-8.0f, 8.0f);
            Vector3 pxToSpawn = new Vector3(_xPos, 7, 0);

            if (waveCurrent > 0)
            {
                enemyType = 2;
                GameObject newEnemy = Instantiate(_dodgingEnemyPrefab, pxToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            else if (waveCurrent == 0)
            {
                enemyType = 1;
                GameObject newEnemy = Instantiate(_enemyPrefab, pxToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }         

            _shipsInWave++;

                if (_shipsInWave == 3)
            {
                _shipsInWave = 0;
                waveCurrent++;
            }            

            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 pxToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            Instantiate(_playerPowerUps[randomPowerUp], pxToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void OnPlayerReset()
    {
        _stopSpawning = true;
    }

    public void OnPlayerReady()
    {
        _stopSpawning = false;
    }
}
