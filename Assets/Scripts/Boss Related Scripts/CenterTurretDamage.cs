using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTurretDamage : MonoBehaviour
{
    private PlayerScript _player;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionPrefab;

    [SerializeField] private GameObject _centerTurretPrefab;
    [SerializeField] private GameObject _centerTurretShield;
    [SerializeField] private GameObject _craterPrefab;

    [SerializeField] private bool _isCenterTurretShieldActive = true;
    [SerializeField] private int _centerTurretShieldHits = 0;
    [SerializeField] private float _centerTurretShielddAlpha = 1.0f;

    private CenterTurretDamage _centerTurretDamage;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _centerTurretDamage = GameObject.Find("CenterTurret").GetComponent<CenterTurretDamage>();

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

        if (_centerTurretDamage == null)
        {
            Debug.LogError("The TurretShield script is null.");
        }
    }

    void TurretDamage()
    {
        Debug.Log("Running LTD on LeftTurret script");

        if (_isCenterTurretShieldActive == true)
        {
            _centerTurretShieldHits++;

            switch (_centerTurretShieldHits)
            {
                case 1:
                    _centerTurretShielddAlpha = 0.75f;
                    _centerTurretShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _centerTurretShielddAlpha);
                    break;
                case 2:
                    _centerTurretShielddAlpha = 0.40f;
                    _centerTurretShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _centerTurretShielddAlpha);
                    break;
                case 3:
                    _isCenterTurretShieldActive = false;
                    _centerTurretShield.SetActive(false);
                    break;
            }
            return;
        }

        DestroyCenterTurret();
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

    public void DestroyCenterTurret()
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



