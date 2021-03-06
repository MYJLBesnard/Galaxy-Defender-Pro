using UnityEngine;

public class BossWaypointNavigation : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private EnemyBoss _enemyBoss;
    public float _enemySpeed;
    public float startWaitTime;
    private float waitTime;
    private int randomSpot;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemyBoss = GameObject.Find("EnemyBoss(Clone)").GetComponent<EnemyBoss>();

        randomSpot = Random.Range(0, _spawnManager.bossWaypoints.Length);
        waitTime = startWaitTime;
       
        if (_gameManager == null)
        {
            Debug.LogError("The Game_Manager is null.");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manageris null.");
        }

        if (_enemyBoss == null)
        {
            Debug.LogError("The Enemy Boss script is null.");
        }
    }

    void Update()
    {
        _enemySpeed = _gameManager.currentBossEnemySpeed;

        transform.position = Vector2.MoveTowards(transform.position, _spawnManager.bossWaypoints[randomSpot].position, _enemySpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _spawnManager.bossWaypoints[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, _spawnManager.bossWaypoints.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}



