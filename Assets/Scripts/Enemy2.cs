using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour // Dodging Enemy
{
    private PlayerScript _player;
    private Animator _animEnemyDestroyed;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;
    [SerializeField] private GameObject _thrusters;
    [SerializeField] private int _enemyType;
    [SerializeField] private bool _stopUpdating = false;
    //[SerializeField] private GameObject _enemyPrefab;


    [SerializeField] public float _dodgingEnemySpeed;
    [SerializeField] private float _dodgingAmplitude;
    [SerializeField] private float _dodgingFrequency = 0.5f;
    private float x, y, z;
    private float _enemyRateOfFire = 3.0f;
    private float _enemyCanFire = -1.0f;
    public float _enemySpeed;
    public float _randomXStartPos = 0;




    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _animEnemyDestroyed = GetComponent<Animator>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _dodgingAmplitude = Random.Range(1.0f, 2.5f);
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

        //_enemySpeed = _gameManager.currentEnemySpeed;
        _dodgingEnemySpeed = _gameManager.currentEnemySpeed;


        if (_stopUpdating == false)
        {
            y = transform.position.y;
            z = transform.position.z;
            x = Mathf.Cos((_dodgingEnemySpeed * Time.time * _dodgingFrequency) * _dodgingAmplitude);

                transform.position = new Vector3((x + _randomXStartPos), y, z);
                transform.Translate(Vector3.down * _dodgingEnemySpeed * Time.deltaTime);

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
        _thrusters.SetActive(false);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 2.8f);
    }

    public IEnumerator SpeedBurst()
    {
        Debug.Log("Running Speed Burst");
        _enemySpeed = 10.0f;

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.25f);
        _enemySpeed = _gameManager.currentEnemySpeed;
    }
}



