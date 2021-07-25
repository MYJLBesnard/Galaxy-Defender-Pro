using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SpawnManager _spawnManager;

    [Range(0, 360)]
    [SerializeField] private float SpreadAngle = 30;
    [SerializeField] private float _playerRateOfFire = 0.15f;
    [SerializeField] private float _speed = 5.0f;
    private float _speedMultiplier = 1.75f;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    [SerializeField] private int NumberOfProjectiles = 3;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerLaserPrefab, _playerDoubleShotLaserPrefab, _playerTripleShotLaserPrefab;
    [SerializeField] private GameObject _playerShield, _playerHealthPowerUpPrefab;
    [SerializeField] private GameObject _playerThrusterLeft, _playerThrusterRight;
    [SerializeField] private GameObject _playerDamage01, _playerDamage02, _playerDamage03, _playerDamage04;
    [SerializeField] private GameObject _bigExplosionPrefab;

    [SerializeField] private bool _hasPlayerLaserCooledDown = false;
    [SerializeField] private bool _gameFirstStart = true;
    [SerializeField] private bool _asteroidDestroyed = false;
    [SerializeField] private bool _isPlayerTripleShotActive = false, _isPlayerShieldActive = false;
    public bool _isPlayerSpeedBoostActive = false;

    public List<GameObject> poolDamageAnimations = new List<GameObject>();
    public List<GameObject> activatedDamageAnimations = new List<GameObject>();

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Manager on the Canvas is null.");
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
            float angleStep = SpreadAngle / NumberOfProjectiles;
            float centeringOffset = (SpreadAngle / 2) - (angleStep / 2);

            for (int i = 0; i < NumberOfProjectiles; i++)
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
            return;
        }

        if (poolDamageAnimations.Count == 0)
        {
            _lives--;
            _uiManager.UpdateLives(_lives);

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
