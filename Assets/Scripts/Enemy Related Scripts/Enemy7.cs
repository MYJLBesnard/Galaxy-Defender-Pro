using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy7 : MonoBehaviour // Mine Layer
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _thrusters;
    [SerializeField] private GameObject _enemyRoamingSpaceMines;
    [SerializeField] private bool _stopUpdating = false;
    [SerializeField] private bool _noSound = false;
    [SerializeField] private bool _isMineLayerArmed = true;
    [SerializeField] public float releasePoint;
    public float enemySpeed;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_player == null)
        {
            Debug.Log("The PlayerScript is null.");
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

        releasePoint = Random.Range(-12.0f, 12.0f);
    }

    void Update()
    {
        CalculateMovement();
    }

    void DeployMines()
    {
        float minesSpawnPoint = 0.88f;
        int totalMines = 0;
        int mineQty = 3 + _gameManager.difficultyLevel; // D1 = 4
                                                        // D2 = 5
                                                        // D3 = 6
                                                        // D4 = 7

        while (totalMines <= mineQty)
        {
            if (_gameManager.enemyMineLayerDirectionRight == true)
            {
                Instantiate(_enemyRoamingSpaceMines, new Vector3(transform.position.x - minesSpawnPoint,
                    transform.position.y, transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(_enemyRoamingSpaceMines, new Vector3(transform.position.x + minesSpawnPoint,
                    transform.position.y, transform.position.z), Quaternion.identity);
            }

            totalMines++;
        }
    }

    void Enemy7Damage()
    {
        DestroyEnemyShip();
    }

    void CalculateMovement()
    {
        enemySpeed = _gameManager.currentEnemySpeed;

        if (_stopUpdating == false)
        {
            if (_gameManager.enemyMineLayerDirectionRight == true)
            {
                transform.Translate(enemySpeed * Time.deltaTime * Vector3.right);

                if (transform.position.x >= releasePoint && _isMineLayerArmed == true)
                {
                    DeployMines();
                    _isMineLayerArmed = false;
                }

                if (transform.position.x > 13.5f)
                { 
                    _noSound = true;
                    DestroyEnemyShip();
                }
            }

            if (_gameManager.enemyMineLayerDirectionRight == false)
            {
                transform.Translate(enemySpeed * Time.deltaTime * Vector3.right);

                if (transform.position.x <= releasePoint && _isMineLayerArmed == true)
                {
                    DeployMines();
                    _isMineLayerArmed = false;
                }

            if (transform.position.x < -13.5f)
                {
                    _noSound = true;
                    DestroyEnemyShip();
                }
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
            Enemy7Damage();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(50);
            }

            _audioSource.Play();
            Enemy7Damage();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(50);
            }

            Destroy(other.gameObject);

            _audioSource.Play();
            Enemy7Damage();
        }
    }

    public void DestroyEnemyShip()
    {
        if (_noSound == false)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }

        _spawnManager.isMineLayerDeployed = false;
        _stopUpdating = true;
        _thrusters.SetActive(false);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }
}



