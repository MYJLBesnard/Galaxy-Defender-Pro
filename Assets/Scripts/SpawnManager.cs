using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] public GameObject[] _playerPowerUps;
    [SerializeField] public GameObject[] _typesOfEnemy;
    [SerializeField] private bool _stopSpawning = false;
    [SerializeField] private bool _stopSpawningEnemies = false;
    [SerializeField] private AudioClip _warningIncomingWave;
    [SerializeField] private AudioClip _attackWaveRepelled;
    public Transform[] enemyWaypoints;
    private GameObject _typeOfEnemy;
    private int _shipsInWave = 0;
    public int totalEnemyShipsDestroyed = 0;
    public int waveCurrent = 0;
    public int enemyType = 0;
    private float _xPos;
    private float _yPos;
    private float _triggerMineLayer = 0f;
    [SerializeField] public bool _isMineLayerDeployed = false;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null.");
        }

        if (_playerScript == null)
        {
            Debug.LogError("The PlayerScript is null.");
        }

        waveCurrent = _gameManager.currentWave;
        //Debug.Log(_typesOfEnemy.Length);
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
                _gameManager.WaveComplete();
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
            _yPos = Random.Range(-5.5f, 5.5f);
            Vector3 pxToSpawn = new Vector3(_xPos, 7, 0);
            _triggerMineLayer = Random.Range(0, 100);

            Debug.Log("Trigger Mine Layer: " + _triggerMineLayer);
            Debug.Log("Game Manager Mine Layer Chance: " + _gameManager.currentEnemyMineLayerChance);
            Debug.Log("If Trigger greater than Chance, Mine Layer should get spawned.");


            if (_stopSpawningEnemies == false)
            {
                _typeOfEnemy = null;
                waveCurrent = _gameManager.currentWave;

                switch (_gameManager.currentWave)
                {
                    case 0: // spawn basic Enenmy
                        int type0 = 0;
                        _typeOfEnemy = _typesOfEnemy[type0];
                        enemyType = type0;
                        break;

                    case 1: // spawn basic Enemy
                        int type1 = 1;
                        _typeOfEnemy = _typesOfEnemy[type1];
                        enemyType = type1;
                        break;

                    case 2: // spawn dodging Enemy
                        int type2 = 2;
                        _typeOfEnemy = _typesOfEnemy[type2];
                        enemyType = type2;
                        break;

                    case 3: // spawn dodging and speed burst Enemy
                        int type3 = 3;
                        //int type3 = Random.Range(2, 4);
                        _typeOfEnemy = _typesOfEnemy[type3];
                        enemyType = type3;
                        break;

                    case 4: // spawn dodging, speed burst Enemy and laser burst / Player Laser avoidance Enemy
                        int type4 = 4;
                        //int type4 = Random.Range(2, 5);
                        _typeOfEnemy = _typesOfEnemy[type4];
                        enemyType = type4;
                        break;

                    case 5: // spawn all from case 4 plus rear shooting Enemy
                        int type5 = 5; 
                        //int type5 = Random.Range(3, 6);
                        _typeOfEnemy = _typesOfEnemy[type5];
                        enemyType = type5;
                        break;

                    case 6: // spawn all from case 5 plus arc laser Enemy
                        int type6 = 6; 
                        //int type6 = Random.Range(4, 7);
                        _typeOfEnemy = _typesOfEnemy[type6];
                        enemyType = type6;
                        break;

                    default:
                        break;
                }

                if (_triggerMineLayer >= _gameManager.currentEnemyMineLayerChance && _isMineLayerDeployed == false)
                {
                    int type7 = 7;
                    _typeOfEnemy = _typesOfEnemy[type7];
                    Vector3 pxToSpawnEnemy7 = new Vector3(-13.0f, _yPos, 0);
                    GameObject newEnemy = Instantiate(_typeOfEnemy, pxToSpawnEnemy7, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _isMineLayerDeployed = true;

                }
                else
                {
                    GameObject newEnemy = Instantiate(_typeOfEnemy, pxToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _shipsInWave++;
                }
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
            int randomPowerUp = Random.Range(0, 8); // spawn Power Ups Elements 0 to 7
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
