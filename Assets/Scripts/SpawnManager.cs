using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _dodgingEnemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] public GameObject[] _playerPowerUps;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerScript _playerScript;

    private int _shipsInWave = 0;
    public int totalEnemyShipsDestroyed = 0;
    public int waveCurrent = 0;
    public int enemyType = 0;

    [SerializeField] private bool _stopSpawning = false;
    [SerializeField] private bool _stopSpawningEnemies = false;

    [SerializeField] private AudioClip _warningIncomingWave;
    [SerializeField] private AudioClip _attackWaveRepelled;


    private GameObject _typeOfEnemy;

    private float _xPos;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game_Manager is null.");
        }

        if (_playerScript == null)
        {
            Debug.LogError("The PlayerScript is null.");
        }
 }

    public void EnemyShipsDestroyedCounter()
    {
        totalEnemyShipsDestroyed++;

        if (totalEnemyShipsDestroyed == _gameManager.currentSizeOfWave)
        {
            if (waveCurrent == _gameManager.howManyLevels - 1)
            {
                Debug.Log("No more waves!");
                _stopSpawning = true;
                _stopSpawningEnemies = true;
                _playerScript.PlayClip(_attackWaveRepelled);
            }
            else
            {
                Debug.Log("Wave completed!");
                waveCurrent++;
                _gameManager.currentAttackWave++;
                _gameManager.UpdateAttackWave();
                _playerScript.PlayClip(_attackWaveRepelled);

                StartCoroutine(StartNewWave());
            }
        }
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

            if (_stopSpawningEnemies == false)
            {
                _typeOfEnemy = null;
                switch (_gameManager.currentAttackWave)
                {
                    case 0:
                        enemyType = 1;
                        _typeOfEnemy = _enemyPrefab;
                        break;

                    case 1:
                        enemyType = 2;
                        _typeOfEnemy = _dodgingEnemyPrefab;
                        break;

                    case 2:
                        enemyType = 1;
                        _typeOfEnemy = _enemyPrefab;
                        break;

                    default:
                        break;
                }
 
                GameObject newEnemy = Instantiate(_typeOfEnemy, pxToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _shipsInWave++;

            }

            if (_shipsInWave == _gameManager.currentSizeOfWave)
            {
                _stopSpawningEnemies = true;
            }

            yield return new WaitForSeconds(_gameManager.currentEnemyRateOfSpawning);
        }
    }

    IEnumerator StartNewWave()
    {
        _stopSpawning = true;
        _shipsInWave = 0;
        totalEnemyShipsDestroyed = 0;
        yield return new WaitForSeconds(5.0f);
        _playerScript.PlayClip(_warningIncomingWave);
        _stopSpawning = false;
        _stopSpawningEnemies = false;
        OnPlayerReady();
        StartSpawning();
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 pxToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 7);
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
