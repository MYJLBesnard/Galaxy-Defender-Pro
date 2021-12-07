using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private PlayerScript _player;
   // private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    public bool isEnemyBossActive = true;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
      //  _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
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
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.tag == "Player")
        {
            PlayerScript player = other.transform.GetComponent<PlayerScript>();

            if (player != null)
            {
                player.Damage();
            }

            _audioSource.Play();
            DestroyEnemyBossShip();
        }
        */

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(40);
            }

            _audioSource.Play();
            DestroyEnemyBossShip();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(other.gameObject);

            _audioSource.Play();
            DestroyEnemyBossShip();
        }
    }

    private void DestroyEnemyBossShip()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Debug.Log("Boss has yaken a hit!!!");

        //Destroy(this.gameObject, 2.8f);
    }
}



