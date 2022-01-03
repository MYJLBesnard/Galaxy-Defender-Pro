using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private PlayerScript _playerScript;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _bigExplosionPrefab;
    public bool isEnemyBossActive = true;

    void Start()
    {
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_playerScript == null)
        {
            Debug.Log("The PlayerScript is null.");
        }

        if (_spawnManager == null)
        {
            Debug.Log("The SpawnManager is null.");
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
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_playerScript != null)
            {
                _playerScript.AddScore(40);
            }

            _audioSource.Play();
            DestroyEnemyBossShip();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_playerScript != null)
            {
                _playerScript.AddScore(10);
            }

            Destroy(other.gameObject);

            _audioSource.Play();
            DestroyEnemyBossShip();
        }
    }

    private void DestroyEnemyBossShip()
    {
        StartCoroutine(DisperseBossExplosions());
        _spawnManager.stopSpawningEnemies = true;
        _spawnManager.stopSpawning = true;
        _spawnManager.isBossActive = false;
        _gameManager.EnemyBossDefeated();
        Destroy(this.gameObject, 0.25f);
        _gameManager.PlayMusic(1, 5.0f);
    }

    IEnumerator DisperseBossExplosions() // upon Boss destruction, instantiates multiple explosions
    {
        yield return new WaitForSeconds(0.1f);
        int multipleExplosions = Random.Range(5, 7);
        for (int i = 0; i < multipleExplosions; i++)
        {
            float x = Random.Range(-4.5f, 4.5f);
            float y = Random.Range(1.75f, -1.25f);
            Instantiate(_bigExplosionPrefab, new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z), Quaternion.identity);

            if (transform.position.x > 9.75f)
            {
                transform.position = new Vector3(9.5f, y, transform.position.z);
            }
            else if (transform.position.x < -9.75f)
            {
                transform.position = new Vector3(-9.5f, y, transform.position.z);
            }
        }
    }
}



