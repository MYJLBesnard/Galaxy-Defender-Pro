using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontalShieldDamage : MonoBehaviour
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;

    [SerializeField] private GameObject _minigunLeftPrefab, _minigunRightPrefab;
    [SerializeField] private GameObject _frontalShield;

    [SerializeField] private bool _isFrontalShieldActive = true;
    [SerializeField] private int _frontalShieldHits = 0;
    [SerializeField] private float _frontalShielddAlpha = 1.0f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
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

    void FrontalShield()
    {
        Debug.Log("Running LTD on LeftTurret script");

        if (_isFrontalShieldActive == true)
        {
            _frontalShieldHits++;

            switch (_frontalShieldHits)
            {
                case 1:
                    _frontalShielddAlpha = 0.75f;
                    _frontalShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _frontalShielddAlpha);
                    break;
                case 2:
                    _frontalShielddAlpha = 0.40f;
                    _frontalShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _frontalShielddAlpha);
                    break;
                case 3:
                    _isFrontalShieldActive = false;
                    _frontalShield.SetActive(false);
                   // _minigunLeftPrefab.GetComponent<BoxCollider2D>().enabled = true;
                   // _minigunRightPrefab.GetComponent<BoxCollider2D>().enabled = true;
                    break;
            }
            return;
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
            FrontalShield();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(50);
            }

            //_audioSource.Play();
            FrontalShield();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(50);
            }

            Destroy(other.gameObject);

            //_audioSource.Play();
            FrontalShield();
        }
    }
}



