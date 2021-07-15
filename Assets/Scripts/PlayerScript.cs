using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 1.75f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _score;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private GameObject _playerLaserPrefab;

    [SerializeField]
    private GameObject _playerDoubleShotLaserPrefab;

    [SerializeField]
    private GameObject _playerTripleShotLaserPrefab;

    [SerializeField]
    private GameObject _playerHealthPowerUpPrefab;

    [SerializeField]
    private GameObject _playerShield;

    [SerializeField]
    private GameObject _playerThrusterLeft, _playerThrusterRight;

    [SerializeField]
    private GameObject _playerDamage01, _playerDamage02, _playerDamage03, _playerDamage04;

    [SerializeField]
    private bool _hasPlayerLaserCooledDown = true;

    [SerializeField]
    private float _playerRateOfFire = 0.15f;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isPlayerTripleShotActive = false;

    [SerializeField]
    private bool _isPlayerShieldActive = false;

    [SerializeField]
    private bool _isPlayerSpeedBoostActive = false;

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
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _hasPlayerLaserCooledDown)
        {
            PlayerFireLaser();
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
            Instantiate(_playerTripleShotLaserPrefab, transform.position, Quaternion.identity);
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

            ResetDamageAnimationList();
            StartCoroutine(ResetPlayerPosition());
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
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

    IEnumerator ResetPlayerPosition()
    {
        transform.position = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.01f);

        GetComponent<BoxCollider2D>().enabled = false;
        _hasPlayerLaserCooledDown = false;
        _uiManager.ReadySetGo();

        yield return new WaitForSeconds(6.0f);

        GetComponent<BoxCollider2D>().enabled = true;
        _hasPlayerLaserCooledDown = true;
        _spawnManager.OnPlayerReady();
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
        // Reverses damage by removing random (if more than 1 active) damages area and returning it to the pool.  This should also
        // add a life (?)
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
