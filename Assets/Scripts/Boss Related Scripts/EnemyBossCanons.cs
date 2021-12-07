using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossCanons : MonoBehaviour
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
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
            DestroyCanon();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                    _player.AddScore(10);
            }

            //_audioSource.Play();
            DestroyCanon();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(other.gameObject);

            //_audioSource.Play();
            DestroyCanon();
        }
    }

    private void DestroyCanon()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager.BossCanonsDestroyedCounter();
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 0.5f);
    }
}



