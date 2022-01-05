using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private EndOfLevelDialogue _endOfLevelDialogue;
    [SerializeField] private EnemyBoss _enemyBossScript;
    [SerializeField] public GameObject _enemyContainer;
    [SerializeField] private GameObject _enemyBoss;
    public GameObject[] _playerPowerUps;
    public GameObject[] _typesOfEnemy;
    public GameObject depletedLateralLaserCanons;

    [SerializeField] private AudioClip _warningIncomingWave;
    [SerializeField] private AudioClip _attackWaveRepelled;

    public Transform[] enemyWaypoints;
    public Transform[] bossWaypoints;
    private GameObject _typeOfEnemy;

    public int totalEnemyShipsDestroyed = 0;
    public int bossTurretsDestroyed = 0;
    [SerializeField] private bool _bossTurretsDestroyed = false;
    public int bossMiniGunsDestroyed = 0;
    [SerializeField] private bool _bossMiniGunsDestroyed = false;
    public int bossCanonsDestroyed = 0;
    [SerializeField] private bool _bossCanonsDestroyed = false;

    public int waveCurrent = 0;
    private int _shipsInWave = 0;
    public int enemyType = 0;
    private float _xPos;
    private float _yPos;
    private float _triggerMineLayer = 0f;
    public bool isBossActive = false;
    public bool isPlayerDestroyed = false;
    public bool isMineLayerDeployed = false;
    public bool stopSpawningEnemies = false;
    public bool stopSpawning = false;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        _endOfLevelDialogue = GameObject.Find("DialoguePlayer").GetComponent<EndOfLevelDialogue>();  //************************************

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager on the Canvas is null.");
        }

        if (_playerScript == null)
        {
            Debug.LogError("The PlayerScript is null.");
        }

        if (_endOfLevelDialogue == null)
        {
            Debug.Log("Dialogue Player is NULL.");
        }

        waveCurrent = _gameManager.currentWave;
    }

    public void SetupBossAlien()
    {
        Debug.Log("Running SetupBossAlien");

        _gameManager.PlayMusic(2, 5.0f);

        if (isBossActive != true)
        {
            Vector3 pxToSpawn = new Vector3(Random.Range(-8f, 8f), 12.0f, 0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);
            Instantiate(_enemyBoss, pxToSpawn, spawnRotation);

            isBossActive = true;
            stopSpawningEnemies = true;
            stopSpawning = false;
            _bossTurretsDestroyed = false;
            _bossMiniGunsDestroyed = false;
            _bossCanonsDestroyed = false;

            bossTurretsDestroyed = 0;
            bossMiniGunsDestroyed = 0;
            bossCanonsDestroyed = 0;

            if (isBossActive == true)
            {
                _enemyBossScript = GameObject.Find("EnemyBoss(Clone)").GetComponent<EnemyBoss>();

                if (_enemyBossScript == null)
                {
                    Debug.LogError("The Enemy Boss script is null.");
                }
            }

            StartCoroutine(SpawnPowerUps());
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
                stopSpawning = true;
                stopSpawningEnemies = true;
                _endOfLevelDialogue.PlayDialogueClip(_attackWaveRepelled);

                //_playerScript.PlayClip(_attackWaveRepelled);
            }
            else
            {
                Debug.Log("Wave completed!");
                _gameManager.WaveComplete();
                _endOfLevelDialogue.PlayDialogueClip(_attackWaveRepelled);
                totalEnemyShipsDestroyed = 0;

                StartCoroutine(StartNewWave());
            }
        }
    }

    public void BossTurretDestroyedCounter() // tracks destroyed turrets
    {
        bossTurretsDestroyed++;

        if (bossTurretsDestroyed == 3)
        {
            _bossTurretsDestroyed = true;
            BossWeaponsDestroyedCheck();
        }
    }

    public void BossMiniGunsDestroyedCounter() // tracks destroyed miniguns
    {
        bossMiniGunsDestroyed++;

        if (bossMiniGunsDestroyed == 2)
        {
            _bossMiniGunsDestroyed = true;
            BossWeaponsDestroyedCheck();
        }
    }

    public void BossCanonsDestroyedCounter() // tracks destroyed canons
    {
        bossCanonsDestroyed++;

        if (bossCanonsDestroyed == 2)
        {
            _bossCanonsDestroyed = true;
            BossWeaponsDestroyedCheck();
        }
    }

    public void BossWeaponsDestroyedCheck() // checks to see if Boss still has weapons.  If no more weapons, Boss vulnerable and can be destroyed
    {
        if (_bossTurretsDestroyed == true && _bossMiniGunsDestroyed == true && _bossCanonsDestroyed == true)
        {
            _enemyBossScript.GetComponent<PolygonCollider2D>().enabled = true;
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
        while (stopSpawning == false)

        {
            _xPos = Random.Range(-8.0f, 8.0f);
            _yPos = Random.Range(-5.5f, 5.5f);
            Vector3 pxToSpawn = new Vector3(_xPos, 9, 0);
            _triggerMineLayer = Random.Range(0, 100);

            /*
            Debug.Log("Trigger Mine Layer: " + _triggerMineLayer);
            Debug.Log("Game Manager Mine Layer Chance: " + _gameManager.currentEnemyMineLayerChance);
            Debug.Log("If Trigger greater than Chance, Mine Layer should get spawned.");
            */

            if (stopSpawningEnemies == false)
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

                    case 3: // spawn speed burst Enemy
                        int type3 = 3;
                        //int type3 = Random.Range(2, 4);
                        _typeOfEnemy = _typesOfEnemy[type3];
                        enemyType = type3;
                        break;

                    case 4: // spawn laser burst / Player Laser avoidance Enemy
                        int type4 = 4;
                        //int type4 = Random.Range(2, 5);
                        _typeOfEnemy = _typesOfEnemy[type4];
                        enemyType = type4;
                        break;

                    case 5: // spawn rear shooting Enemy
                        int type5 = 5; 
                        //int type5 = Random.Range(3, 6);
                        _typeOfEnemy = _typesOfEnemy[type5];
                        enemyType = type5;
                        break;

                    case 6: // spawn arc laser Enemy
                        int type6 = 6; 
                        //int type6 = Random.Range(4, 7);
                        _typeOfEnemy = _typesOfEnemy[type6];
                        enemyType = type6;
                        break;

                    case 7: // Boss
                        SetupBossAlien();
                        _enemyBossScript.isEnemyBossActive = true;
                        break;

                    default:
                        break;
                }

                // Mine Layer Logic
                if (_triggerMineLayer >= _gameManager.currentEnemyMineLayerChance && isMineLayerDeployed == false)
                {
                    int type7 = 7;
                    _typeOfEnemy = _typesOfEnemy[type7];
                    int enemyDirection = Random.Range(0, 100);

                    if (enemyDirection <= 50) // Mine Layer travels Left to Right
                    {
                        Vector3 pxToSpawnEnemy7 = new Vector3(-13.0f, _yPos, 0);
                        GameObject newEnemy = Instantiate(_typeOfEnemy, pxToSpawnEnemy7, Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                        _gameManager.enemyMineLayerDirectionRight = true;
                    }
                    else if (enemyDirection > 51) // Mine Layer travels Right to Left
                    {
                        Vector3 pxToSpawnEnemy7 = new Vector3(13.0f, _yPos, 0);
                        GameObject newEnemy = Instantiate(_typeOfEnemy, pxToSpawnEnemy7, Quaternion.identity);
                        newEnemy.transform.Rotate(0, 0, 180);
                        newEnemy.transform.parent = _enemyContainer.transform;
                        _gameManager.enemyMineLayerDirectionRight = false;
                    }

                    isMineLayerDeployed = true;

                }
                else
                {
                    if (isBossActive != true)
                    {
                        GameObject newEnemy = Instantiate(_typeOfEnemy, pxToSpawn, Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                        _shipsInWave++;
                    }
                }
            }

            if (_shipsInWave == _gameManager.currentSizeOfWave)
            {
                stopSpawningEnemies = true;
            }

            yield return new WaitForSeconds(_gameManager.currentEnemyRateOfSpawning);
        }
    }

    public void AdvanceToNextLevel()
    {
        if (isPlayerDestroyed == false)
        {
            StartCoroutine(StartNewWave());
        }
    }

    IEnumerator StartNewWave()
    {
        stopSpawning = true;
        _shipsInWave = 0;
        totalEnemyShipsDestroyed = 0;
        yield return new WaitForSeconds(3.5f);
        _endOfLevelDialogue.PlayDialogueClip(_warningIncomingWave);

        stopSpawning = false;
        stopSpawningEnemies = false;
        OnPlayerReady();
        StartSpawning();
        _uiManager.UpdateLevelInfo();
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(2.0f);
        while (stopSpawning == false)
        {
            Vector3 pxToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, _playerPowerUps.Length); // spawn Power Ups Elements 0 to Length of Array
            Instantiate(_playerPowerUps[randomPowerUp], pxToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
        isPlayerDestroyed = true;
        _enemyBossScript.isEnemyBossActive = false;
    }

    public void OnPlayerReset()
    {
        stopSpawning = true;
        isPlayerDestroyed = true;
        _enemyBossScript.isEnemyBossActive = false;
    }

    public void OnPlayerReady()
    {
        stopSpawning = false;
        isPlayerDestroyed = false;
        StartCoroutine(TurnBossWeaponsBackOn());
    }

    IEnumerator TurnBossWeaponsBackOn()
    {
        yield return new WaitForSeconds(2.5f);
        _enemyBossScript.isEnemyBossActive = true;
    }
}
