using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretDamage : MonoBehaviour // Arc Laser Burst
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _thrusters;
    [SerializeField] private GameObject _enemyArcLaserPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private bool _isEnemyShieldActive = true;
    [SerializeField] public int _shield6Hits = 0;
    [SerializeField] private float _enemyShieldAlpha = 1.0f;
    [SerializeField] private bool _stopUpdating = false;
    private Enemy6Shield _enemy6Shield;
    public float _enemySpeed;
    public float _randomXStartPos;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemy6Shield = GameObject.Find("Enemy6Shield").GetComponent<Enemy6Shield>();

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

        if (_enemy6Shield == null)
        {
            Debug.LogError("The Enemy6Shiled script is null.");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    public void Enemy6Damage()
    {
        if (_isEnemyShieldActive == true)
        {
            _shield6Hits++;

            switch (_shield6Hits)
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
            Enemy6Damage();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(60);
            }

            _audioSource.Play();
            Enemy6Damage();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(60);
            }

            Destroy(other.gameObject);

            _audioSource.Play();
            Enemy6Damage();
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

    public IEnumerator ArcBurst()
    {
        _enemyArcLaserPrefab.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _enemyArcLaserPrefab.SetActive(false);
    }
}



