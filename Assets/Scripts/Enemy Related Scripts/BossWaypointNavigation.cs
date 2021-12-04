using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaypointNavigation : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    public float _enemySpeed;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;
    public float startWaitTime;
    private float waitTime;
    private int randomSpot;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
    }

    void Update()
    {
        _enemySpeed = 1.5f;

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



