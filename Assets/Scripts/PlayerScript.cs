using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SpawnManager _spawnManager;

    [SerializeField] private int _numberOfProjectiles = 3;
    [Range(0, 360)][SerializeField] private float _spreadAngle = 30;
    [SerializeField] private float _playerRateOfFire = 0.15f;
    [SerializeField] private float _speed = 5.0f;
    private float _speedMultiplier = 1.75f;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;

    [SerializeField] private AudioClip _powerupAudioClip;
    [SerializeField] private AudioClip _playerLaserShotAudioClip;
    private AudioSource _audioSource;


    [SerializeField] private GameObject _playerLaserPrefab, _playerDoubleShotLaserPrefab, _playerTripleShotLaserPrefab;
    [SerializeField] private GameObject _playerShield, _playerHealthPowerUpPrefab;
    [SerializeField] private GameObject _playerThrusterLeft, _playerThrusterRight;
    [SerializeField] private GameObject _playerDamage01, _playerDamage02, _playerDamage03, _playerDamage04;
    [SerializeField] private GameObject _bigExplosionPrefab;

    [SerializeField] private GameObject _leftEngineDamage, _rightEngineDamage;

    [SerializeField] private bool _hasPlayerLaserCooledDown = false;
    [SerializeField] private bool _gameFirstStart = true;
    [SerializeField] private bool _asteroidDestroyed = false;
    [SerializeField] private bool _isPlayerTripleShotActive = false, _isPlayerShieldActive = false, _isPlayerSpeedBoostActive = false;

    public List<GameObject> poolDamageAnimations = new List<GameObject>();
    public List<GameObject> activatedDamageAnimations = new List<GameObject>();

    private CameraShaker _camera;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShaker>();


        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager on the Canvas is null.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The audio source is null.");
        }

        if (_camera == null)
        {
            Debug.LogError("The CameraShaker on the Main Camera is null.");
        }

        StartCoroutine(ResetPlayerPosition());
    }

    void Update()
    {
        if (_gameFirstStart == false)
        {
            CalculateMovement();

            if (Input.GetKeyDown(KeyCode.Space) && _hasPlayerLaserCooledDown)
            {
                PlayerFireLaser();
            }
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void PlayerFireLaser()
    {
        if (_isPlayerTripleShotActive == true)
        {
            float angleStep = _spreadAngle / _numberOfProjectiles;
            float centeringOffset = (_spreadAngle / 2) - (angleStep / 2);

            for (int i = 0; i < _numberOfProjectiles; i++)
            {
                float currentBulletAngle = angleStep * i;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, currentBulletAngle - centeringOffset));
                Instantiate(_playerLaserPrefab, new Vector3(transform.position.x, transform.position.y + 0.404f, transform.position.z), rotation);
            }
        }
        else
        {
            Instantiate(_playerDoubleShotLaserPrefab, transform.position, Quaternion.identity);
        }

        _hasPlayerLaserCooledDown = false;
        StartCoroutine(PlayerLaserCoolDownTimer());

        PlayClip(_playerLaserShotAudioClip);
        
    }

    public void PlayClip(AudioClip soundEffectClip)
    {
        _audioSource.PlayOneShot(soundEffectClip);
    }


    IEnumerator PlayerLaserCoolDownTimer()
    {
        yield return new WaitForSeconds(_playerRateOfFire);
        _hasPlayerLaserCooledDown = true;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score); // communicate with the UI to update the score
    }

    
    // Damage() using the Lists and random damage locations
    public void Damage()
    {
        if (_isPlayerShieldActive == true)
        {
            _isPlayerShieldActive = false;
            _playerShield.SetActive(false);
            return;
        }

        // randomly selects damaged area and sets it active.  The removes from pool to "Active" list.
        if (poolDamageAnimations.Count > 0)
        {
            var rdmDamage = Random.Range(0, poolDamageAnimations.Count);
            var temp = poolDamageAnimations[rdmDamage];
            activatedDamageAnimations.Add(temp);
            temp.SetActive(true);
            poolDamageAnimations.Remove(temp);
            _camera.StartDamageCameraShake(0.2f, 0.15f);
            return;
        }

        if (poolDamageAnimations.Count == 0)
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            _camera.StartDamageCameraShake(0.2f, 0.35f);
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);

            if (_lives != 0)
            {
                ResetDamageAnimationList();
                StartCoroutine(ResetPlayerPosition());
            }
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    /*
    // Damage() using the left/right engine damage sprites
    public void Damage()
    {
        if (_isPlayerShieldActive == true)
        {
            _isPlayerShieldActive = false;
            _playerShield.SetActive(false);
            return;
        }

            _lives--;

        if (_lives == 2)
        {
            _leftEngineDamage.SetActive(true);
        }

        else if (_lives == 1)
        {
            _rightEngineDamage.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    */

    public void ResetDamageAnimationList()
    {
        _spawnManager.OnPlayerReset();

        while (activatedDamageAnimations.Count > 0)
        {
            var rdmDamage = Random.Range(0, activatedDamageAnimations.Count);
            var temp = activatedDamageAnimations[rdmDamage];
            poolDamageAnimations.Add(temp);
            temp.SetActive(false);
            activatedDamageAnimations.Remove(temp);
        }
    }

    public void AsteroidDestroyed()
    {
        _asteroidDestroyed = true;
    }

    IEnumerator ResetPlayerPosition()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        _speed = 0;
        _hasPlayerLaserCooledDown = false;
        _uiManager.ReadySetGo();
        yield return new WaitForSeconds(0.8f);

        transform.position = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(3.2f);

        GetComponent<BoxCollider2D>().enabled = true;
        _hasPlayerLaserCooledDown = true;
        _speed = 5.0f;

        if (_gameFirstStart == true && _asteroidDestroyed == false)
        {
            _gameFirstStart = false;
            _spawnManager.OnPlayerReady();
        }

        if (_asteroidDestroyed == true)
        {
            _spawnManager.OnPlayerReady();
            _spawnManager.StartSpawning();
        }
    }

    public void TripleShotActivate()
    {
        _isPlayerTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownTimer());
    }

    public void SpeedBoostActivate()
    {
        _isPlayerSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownTimer());
    }

    public void HealthBoostActivate()
    {
        // Reverses damage by removing random (if more than 1 active) damages area and returning it to the pool.
        if (activatedDamageAnimations.Count > 0)
        {
            var rdmDamage = Random.Range(0, activatedDamageAnimations.Count);
            var temp = activatedDamageAnimations[rdmDamage];
            poolDamageAnimations.Add(temp);
            temp.SetActive(false);
            activatedDamageAnimations.Remove(temp);
        }
    }

    public void ShieldActivate()
    {
        _isPlayerShieldActive = true;
        _playerShield.SetActive(true);
    }

    IEnumerator TripleShotPowerDownTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _isPlayerTripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        _isPlayerSpeedBoostActive = false;
    }
}
