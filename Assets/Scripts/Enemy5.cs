using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : MonoBehaviour // Rear Burst
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;  //***********

    [SerializeField] private GameObject _enemyPrefab;
    public float _enemySpeed;
    private float y, z;
    public float _randomXStartPos;
    [SerializeField] private bool _stopUpdating = false;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyRearShotLaserPrefab;
    [SerializeField] private GameObject _explosionPrefab;


    //private float _enemyRateOfFire = 3.0f;
    //private float _enemyCanFire = -1.0f;

    private GameManager _gameManager; // *************


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
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
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {

        _enemySpeed = _gameManager.currentEnemySpeed;

        if (_stopUpdating == false)
        {
            y = transform.position.y;
            z = transform.position.z;

            transform.position = new Vector3(_randomXStartPos, y, z);
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);

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
                _player.AddScore(20);
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
        Debug.Log("Destroy Rear Shooting Enemy?");
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        _spawnManager.EnemyShipsDestroyedCounter();
        _stopUpdating = true;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }

    public IEnumerator RearLaserBurst()
    {
        if (_stopUpdating == false)
        {
            Debug.Log("Running Rear Laser Burst");
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            GameObject enemyLaser = Instantiate(_enemyRearShotLaserPrefab, position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            yield return new WaitForSeconds(0.1f);
        }  
    }
}



