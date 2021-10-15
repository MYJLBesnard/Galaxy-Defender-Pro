using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    private PlayerScript _player;
    private Animator _animEnemyDestroyed;
    private SpawnManager _spawnManager;  //***********

    [SerializeField] private int _enemyType;
    [SerializeField] public float _enemySpeed;

    private float x, y, z;
    public float _randomXStartPos = 0;

    [SerializeField] private bool _stopUpdating = false;

    [SerializeField] private AudioClip _explosionSoundEffect;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;
    private float _enemyRateOfFire = 3.0f;
    private float _enemyCanFire = -1.0f;

    private GameManager _gameManager; // *************


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _animEnemyDestroyed = GetComponent<Animator>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();


        if (_player == null)
        {
            Debug.Log("The PlayerScript is null.");
        }

        if (_animEnemyDestroyed == null)
        {
            Debug.Log("The Enemy Dstroyed anim is null.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Enemy Audio Source is null.");
        }
        else
        {
            _audioSource.clip = _explosionSoundEffect;
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
            GameObject enemyLaser = Instantiate(_enemyDoubleShotLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            //PlayClip(_enemyLaserShotAudioClip);
        }
    }

    void CalculateMovement()
    {

        _enemySpeed = _gameManager.currentEnemySpeed;

        if (_stopUpdating == false)
        {
            y = transform.position.y;
            z = transform.position.z;
            x = transform.position.x;

                transform.position = new Vector3(_randomXStartPos, y, z);
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

                if (transform.position.y < -7.0f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7.0f, 0);
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

            _audioSource.Play();
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

            _audioSource.Play();
            DestroyEnemyShip();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(other.gameObject);

            _audioSource.Play();
            DestroyEnemyShip();
        }
    }

    private void DestroyEnemyShip()
    {
        _spawnManager.EnemyShipsDestroyedCounter();
        _stopUpdating = true;
        _animEnemyDestroyed.SetTrigger("OnEnemyDeath");
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 2.8f);
    }
}



