using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private PlayerScript _player;
    private Animator _animEnemyDestroyed;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;
    [SerializeField] private bool _stopUpdating = false;
    [SerializeField] private bool _incomingPlayerLaser = false;
    public float _enemySpeed;
    public float _randomXStartPos;
    [SerializeField] public int _randomNumber;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _animEnemyDestroyed = GetComponent<Animator>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

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
            Debug.LogError("The Game Manager is null.");
        }

        _randomNumber = Random.Range(-10, 10); // used to randomly pick left or right dodge
    }

    void Update()
    {

    }

    void CalculateMovement()
    {
        _enemySpeed = _gameManager.currentEnemySpeed;

        if (_stopUpdating == false)
        {
            transform.Translate(_enemySpeed * Time.deltaTime * Vector3.down);

            if (_incomingPlayerLaser == true) // dodges incoming Player laser
            {
                if (_randomNumber > 0)
                {
                    transform.Translate(20f * Time.deltaTime * Vector3.left);
                    transform.Translate(_enemySpeed * Time.deltaTime * Vector3.down);
                }
                else if (_randomNumber < 0)
                {
                    transform.Translate(20f * Time.deltaTime * Vector3.right);
                    transform.Translate(_enemySpeed * Time.deltaTime * Vector3.down);
                }
            }

            if (transform.position.x > 10.25f)
            {
                transform.position = new Vector3(-10.25f, transform.position.y, 0);
            }
            else if (transform.position.x < -10.25f)
            {
                transform.position = new Vector3(10.25f, transform.position.y, 0);
            }

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
                _player.AddScore(40);
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



