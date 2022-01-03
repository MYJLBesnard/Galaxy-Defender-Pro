using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTurretDamage : MonoBehaviour
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;

    [SerializeField] private GameObject _leftTurretPrefab;
    [SerializeField] private GameObject _leftTurretShield;
    [SerializeField] private GameObject _craterPrefab;

    [SerializeField] private bool _isLeftTurretShieldActive = true;
    [SerializeField] private int _leftTurretShieldHits = 0;
    [SerializeField] private float _leftTurretShielddAlpha = 1.0f;

    private LeftTurretDamage _leftTurretDamage;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _leftTurretDamage = GameObject.Find("LeftTurret").GetComponent<LeftTurretDamage>();

        _craterPrefab.gameObject.SetActive(false);

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

        if (_leftTurretDamage == null)
        {
            Debug.LogError("The TurretShield script is null.");
        }
    }

    void TurretDamage()
    {
        if (_isLeftTurretShieldActive == true)
        {
            _leftTurretShieldHits++;

            switch (_leftTurretShieldHits)
            {
                case 1:
                    _leftTurretShielddAlpha = 0.75f;
                    _leftTurretShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _leftTurretShielddAlpha);
                    break;
                case 2:
                    _leftTurretShielddAlpha = 0.40f;
                    _leftTurretShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _leftTurretShielddAlpha);
                    break;
                case 3:
                    _isLeftTurretShieldActive = false;
                    _leftTurretShield.SetActive(false);
                    break;
            }
            return;
        }

        DestroyLeftTurret();
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

 //           _audioSource.Play();
            TurretDamage();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(50);
            }

 //           _audioSource.Play();
            TurretDamage();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(50);
            }

            Destroy(other.gameObject);

 //           _audioSource.Play();
            TurretDamage();
        }
    }

    public void DestroyLeftTurret()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _audioSource.Play(); //**************************************************************
        _spawnManager.BossTurretDestroyedCounter();
        Destroy(this.gameObject, 0.1f);
        _craterPrefab.gameObject.SetActive(true);

        StartCoroutine(CraterDelay());
    }

    IEnumerator CraterDelay()
    {
        yield return new WaitForSeconds(2.0f); // delays appearance of crater
        _craterPrefab.GetComponent<Renderer>().enabled = true;
    }

}



