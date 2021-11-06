using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointNavigation : MonoBehaviour
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    [SerializeField] public float _enemySpeed;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;
    public float startWaitTime;
    private float waitTime;
    private int randomSpot;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        randomSpot = Random.Range(0, _spawnManager.enemyWaypoints.Length);
        waitTime = startWaitTime;

        if (_player == null)
        {
            Debug.Log("The PlayerScript is null.");
        }
       
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

        transform.position = Vector2.MoveTowards(transform.position, _spawnManager.enemyWaypoints[randomSpot].position, _enemySpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _spawnManager.enemyWaypoints[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, _spawnManager.enemyWaypoints.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerScript player = other.transform.GetComponent<PlayerScript>();

            if (player != null)
            {
                player.Damage();
            }
            _player.PlayClip(_explosionSoundEffect);
            DestroyEnemyMine();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);
            _player.AddScore(5);
            _player.PlayClip(_explosionSoundEffect);
            DestroyEnemyMine();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(other.gameObject);
            _player.PlayClip(_explosionSoundEffect);
            DestroyEnemyMine();
        }
    }

    public void DestroyEnemyMine()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }
}



