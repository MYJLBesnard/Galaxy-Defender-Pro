using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic1 : MonoBehaviour
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;  //***********

    [SerializeField] private int _enemyType;
    [SerializeField] public float _enemySpeed;

    [SerializeField] private bool _stopUpdating = false;

    [SerializeField] private AudioClip _explosionSoundEffect;

    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;
    private float _enemyRateOfFire = 3.0f;
    private float _enemyCanFire = -1.0f;

    private GameManager _gameManager; // *************

    [SerializeField] private GameObject _explosionPrefab;

    public float startWaitTime;
    public Transform[] enemyWaypoints;

    private float waitTime;
    private int randomSpot;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        randomSpot = Random.Range(0, enemyWaypoints.Length);
        waitTime = startWaitTime;

        if (_player == null)
        {
            Debug.Log("The PlayerScript is null.");
        }

        if (_gameManager == null)
        {
            Debug.LogError("The Game_Manager is null.");
        }
    }

    void Update()
    {

        CalculateMovement();


        if (Time.time > _enemyCanFire && _stopUpdating == false)
        {
            _enemyRateOfFire = _gameManager.currentEnemyRateOfFire;

            _enemyCanFire = Time.time + _enemyRateOfFire;
            //    GameObject enemyLaser = Instantiate(_enemyDoubleShotLaserPrefab, transform.position, Quaternion.identity);
            //    Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            //    for (int i = 0; i < lasers.Length; i++)
            {
                //        lasers[i].AssignEnemyLaser();
            }

            //PlayClip(_enemyLaserShotAudioClip);
        }
    }

    void CalculateMovement()
    {
        _enemySpeed = _gameManager.currentEnemySpeed;

        transform.position = Vector2.MoveTowards(transform.position, enemyWaypoints[randomSpot].position, _enemySpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, enemyWaypoints[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, enemyWaypoints.Length);
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
            DestroyEnemyShip();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                if(_enemyType == 1)
                {
                    _player.AddScore(10);
                }
                else if (_enemyType == 2)
                {
                    _player.AddScore(15);
                }
            }

            _player.PlayClip(_explosionSoundEffect);
            DestroyEnemyShip();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(other.gameObject);

            _player.PlayClip(_explosionSoundEffect);
            DestroyEnemyShip();
        }
    }

    public void DestroyEnemyShip()
    {
        Debug.Log("destroy roaming enemy?");
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        _spawnManager.EnemyShipsDestroyedCounter();
        _stopUpdating = true;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }
}



