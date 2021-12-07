using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTurretDamage : MonoBehaviour
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;

    [SerializeField] private GameObject _rightTurretPrefab;
    [SerializeField] private GameObject _rightTurretShield;
    [SerializeField] private GameObject _craterPrefab;

    [SerializeField] private bool _isRightTurretShieldActive = true;
    [SerializeField] private int _rightTurretShieldHits = 0;
    [SerializeField] private float _rightTurretShielddAlpha = 1.0f;

    private RightTurretDamage _rightTurretDamage;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _rightTurretDamage = GameObject.Find("RightTurret").GetComponent<RightTurretDamage>();

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

        if (_rightTurretDamage == null)
        {
            Debug.LogError("The TurretShield script is null.");
        }
    }

    void TurretDamage()
    {
        Debug.Log("Running LTD on LeftTurret script");

        if (_isRightTurretShieldActive == true)
        {
            _rightTurretShieldHits++;

            switch (_rightTurretShieldHits)
            {
                case 1:
                    _rightTurretShielddAlpha = 0.75f;
                    _rightTurretShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _rightTurretShielddAlpha);
                    break;
                case 2:
                    _rightTurretShielddAlpha = 0.40f;
                    _rightTurretShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _rightTurretShielddAlpha);
                    break;
                case 3:
                    _isRightTurretShieldActive = false;
                    _rightTurretShield.SetActive(false);
                    break;
            }
            return;
        }

        DestroyRightTurret();
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
            TurretDamage();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(50);
            }

            _audioSource.Play();
            TurretDamage();
        }

        if (other.tag == "PlayerHomingMissile")
        {
            if (_player != null)
            {
                _player.AddScore(50);
            }

            Destroy(other.gameObject);

            _audioSource.Play();
            TurretDamage();
        }
    }

    public void DestroyRightTurret()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
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



