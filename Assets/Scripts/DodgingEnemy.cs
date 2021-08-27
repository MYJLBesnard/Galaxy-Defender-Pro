using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingEnemy : MonoBehaviour
{
    private PlayerScript _player;
    private Animator _animEnemyDestroyed;

    [SerializeField] private float _dodgingEnemySpeed;
    [SerializeField] private float _dodgingAmplitude;
    [SerializeField] private float _dodgingFrequency = 0.5f;
    private float x, y, z;
    public float _randomXStartPos = 0;

    [SerializeField] private GameObject _dodgingEnemyPrefab;

    [SerializeField] private bool _stopUpdating = false;

    [SerializeField] private AudioClip _explosionSoundEffect;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;
    private float _enemyRateOfFire = 3.0f;
    private float _enemyCanFire = -1.0f;

    void Start()
    {
        _dodgingEnemySpeed = Random.Range(2.5f, 4.5f);
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _animEnemyDestroyed = GetComponent<Animator>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _dodgingAmplitude = Random.Range(1.0f, 2.5f);
        _audioSource = GetComponent<AudioSource>();

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
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _enemyCanFire && _stopUpdating == false)
        {
            _enemyRateOfFire = Random.Range(3f, 7f);
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
                _dodgingEnemySpeed = Random.Range(2.5f, 4.5f);
            }
        }
    }

    public void PlayClip(AudioClip soundEffectClip)
    {
        _audioSource.PlayOneShot(soundEffectClip);
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
                _player.AddScore(15); // calls the AddScore() method in the PlayerScript to add 15 points to the score
                                      // the value of 15 is set to this type of enemy, but we could expand later with a
                                      // Switch statement to attribute different values to "points"
            }

            _audioSource.Play();
            DestroyEnemyShip();
        }
    }

    private void DestroyEnemyShip()
    {
        _stopUpdating = true;
        _animEnemyDestroyed.SetTrigger("OnEnemyDeath");
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 2.8f);
    }
}




