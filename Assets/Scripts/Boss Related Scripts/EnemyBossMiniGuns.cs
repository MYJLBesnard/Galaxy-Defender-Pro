using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMiniGuns : MonoBehaviour
{
    private PlayerScript _player;
    //private Animator _animEnemyDestroyed;
   // private SpawnManager _spawnManager;
   // private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    //[SerializeField] private AudioClip _enemyLaserShotAudioClip;
    //[SerializeField] private GameObject _enemyDoubleShotLaserPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    //[SerializeField] private GameObject _thrusters;
    //[SerializeField] private int _enemyType;
    //[SerializeField] private bool _stopUpdating = false;
   // private float _enemyRateOfFire = 3.0f;
   // private float _enemyCanFire = -1.0f;
   // public float _enemySpeed;
   // public float _randomXStartPos;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
      //  _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
       // _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _audioSource = GetComponent<AudioSource>();
      //  _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();


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

     //   if (_gameManager == null)
     //   {
     //       Debug.LogError("The Game Manager is null.");
     //   }
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

            //_audioSource.Play();
            DestroyMiniGun();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                    _player.AddScore(10);
            }

            //_audioSource.Play();
            DestroyMiniGun();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(other.gameObject);

            //_audioSource.Play();
            DestroyMiniGun();
        }
    }

    private void DestroyMiniGun()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

     //   _spawnManager.EnemyShipsDestroyedCounter();

        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }
}



