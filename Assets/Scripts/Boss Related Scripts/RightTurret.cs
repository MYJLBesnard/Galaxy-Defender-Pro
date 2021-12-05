using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTurret : MonoBehaviour // Rear Burst
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyRearShotLaserPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _thrusters;
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private bool _isEnemyShieldActive = true;
    [SerializeField] public int _shield5Hits = 0;
    [SerializeField] private float _enemyShieldAlpha = 1.0f;
    [SerializeField] private bool _stopUpdating = false;
    private Enemy5Shield _enemy5Shield;
    public float _enemySpeed;
    public float _randomXStartPos;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemy5Shield = GameObject.Find("Enemy5Shield").GetComponent<Enemy5Shield>();

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

        if (_enemy5Shield == null)
        {
            Debug.LogError("The Enemy5Shield script is null.");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    void Enemy5Damage()
    {
        if (_isEnemyShieldActive == true)
        {
            _shield5Hits++;

            switch (_shield5Hits)
            {
                case 1:
                    _enemyShieldAlpha = 0.75f;
                    _enemyShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _enemyShieldAlpha);
                    break;
                case 2:
                    _enemyShieldAlpha = 0.40f;
                    _enemyShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _enemyShieldAlpha);
                    break;
                case 3:
                    _isEnemyShieldActive = false;
                    _enemyShield.SetActive(false);
                    break;
            }
            return;
        }

        DestroyEnemyShip();
    }

    void CalculateMovement()
    {

        _enemySpeed = _gameManager.currentEnemySpeed;

        if (_stopUpdating == false)
        {
            transform.Translate(_enemySpeed * Time.deltaTime * Vector3.down);

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
            Enemy5Damage();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(50);
            }

            _audioSource.Play();
            Enemy5Damage();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(50);
            }

            Destroy(other.gameObject);

            _audioSource.Play();
            Enemy5Damage();
        }
    }

    public void DestroyEnemyShip()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        _spawnManager.EnemyShipsDestroyedCounter();
        _stopUpdating = true;
        _thrusters.SetActive(false);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }

    public IEnumerator RearLaserBurst()
    {
        if (_stopUpdating == false)
        {
            //Debug.Log("Running Rear Laser Burst");
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



