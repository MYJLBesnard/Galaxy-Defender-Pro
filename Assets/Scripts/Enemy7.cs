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
   // private float x, z;
    public float enemySpeed;
    [SerializeField] public float releasePoint;
    [SerializeField] private bool _isMineLayerArmed = true;
   // public float _randomYStartPos;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
      //  _randomYStartPos = Random.Range(-5.5f, 5.5f);
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

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
            Debug.LogError("The Game_Manager is null.");
        }

        releasePoint = Random.Range(-12.0f, 12.0f);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.L))
        {
            DeployMines();
        }
    }

    void DeployMines()
    {

        Instantiate(_enemyRoamingSpaceMines, new Vector3(transform.position.x - 0.88f, transform.position.y, transform.position.z), Quaternion.identity);

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
          //  x = transform.position.x;
          //  z = transform.position.z;

          //  transform.position = new Vector3(x, _randomYStartPos, z);
            transform.Translate(enemySpeed * Time.deltaTime * Vector3.right);
            Debug.Log(transform.position.x);

            if (transform.position.x >= releasePoint && _isMineLayerArmed == true)
            {
                Debug.Log("Release the mines!...");
                DeployMines();
                _isMineLayerArmed = false;
            }

            if (transform.position.x > 13.5f)
            {
                _noSound = true;
                DestroyEnemyShip();
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

        _spawnManager.EnemyShipsDestroyedCounter();
        _stopUpdating = true;
        _thrusters.SetActive(false);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }

}



