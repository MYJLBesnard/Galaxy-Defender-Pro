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
    private GameObject _playerLaserPrefab;

    [SerializeField]
    private GameObject _playerTripleShotLaserPrefab;

    [SerializeField]
    private GameObject _playerShield;

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


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
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

       // transform.Translate(direction * _speed * Time.deltaTime);

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
            Instantiate(_playerLaserPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
        }
        
        _hasPlayerLaserCooledDown = false;
        StartCoroutine(PlayerLaserCoolDownTimer());
    }

    IEnumerator PlayerLaserCoolDownTimer()
    {
        yield return new WaitForSeconds(_playerRateOfFire);
        _hasPlayerLaserCooledDown = true;
    }

    public void Damage()
    {
        if (_isPlayerShieldActive == true)
        {
            _isPlayerShieldActive = false;
            _playerShield.SetActive(false);
            return;
        }
        
            _lives--;
        

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
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
