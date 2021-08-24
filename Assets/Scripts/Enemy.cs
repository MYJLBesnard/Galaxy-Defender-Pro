﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerScript _player;
    private Animator _animEnemyDestroyed;

    [SerializeField] private GameObject _enemyOnePrefab;
    [SerializeField] private float _enemyOneSpeed;
    private float y, z;
    private float _randomXStartPos = 0;

    [SerializeField] private bool _stopUpdating = false;

    [SerializeField] private AudioClip _explosionSoundEffect;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _enemyLaserShotAudioClip;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;




    void Start()
    {
        _enemyOneSpeed = Random.Range(2.5f, 4.5f);
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _animEnemyDestroyed = GetComponent<Animator>();
        _randomXStartPos = Random.Range(-8.0f, 8.0f);
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The PlayerScript is null.");
        }

        if (_animEnemyDestroyed == null)
        {
            Debug.LogError("The Enemy Dstroyed anim is null.");
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
        if (_stopUpdating == false)
        {
            y = transform.position.y;
            z = transform.position.z;

            transform.position = new Vector3((_randomXStartPos), y, z);
            transform.Translate(Vector3.down * _enemyOneSpeed * Time.deltaTime);

            if (transform.position.y < -7.0f)
            {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 7.0f, 0);
                _enemyOneSpeed = Random.Range(2.5f, 4.5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemyFireLaser();
        }

    }

    void EnemyFireLaser()
    {
        
            Instantiate(_enemyDoubleShotLaserPrefab, transform.position, Quaternion.identity);
        

        //_hasPlayerLaserCooledDown = false;
        //StartCoroutine(PlayerLaserCoolDownTimer());

        PlayClip(_enemyLaserShotAudioClip);

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
                _player.AddScore(10); // calls the AddScore() method in the PlayerScript to add 10 points to the score
                                      // the value of 10 is set to this type of enemy, but we could expand later with a
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
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
    }
}




