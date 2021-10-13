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
    [SerializeField] private int _ammoCount =20;

    [SerializeField] private AudioClip _powerupAudioClip;
    [SerializeField] private AudioClip _asteroidBlockingSensors;
    [SerializeField] private AudioClip _warningIncomingWave;
    [SerializeField] private AudioClip _playerLaserShotAudioClip;
    [SerializeField] private AudioClip _warningCoreTempCritical;
    [SerializeField] private AudioClip _warningCoreTempExceeded;
    [SerializeField] private AudioClip _coreTempNominal;
    [SerializeField] private AudioClip _playerShields100AudioClip;
    [SerializeField] private AudioClip _playerShields65AudioClip;
    [SerializeField] private AudioClip _playerShields35AudioClip;
    [SerializeField] private AudioClip _playerShieldsDepletedAudioClip;
    [SerializeField] private AudioClip _shipRepairsUnderwayAudioClip;
    [SerializeField] private AudioClip _explosionSoundEffect;
    private AudioSource _audioSource;


    [SerializeField]
    private GameObject _playerLaserPrefab, _playerDoubleShotLaserPrefab, _playerTripleShotLaserPrefab, _playerLateralLaserPrefab, _playerMissilesDispersed;
    [SerializeField] private GameObject _playerShield, _playerHealthPowerUpPrefab;
    [SerializeField] private GameObject _playerThrusterLeft, _playerThrusterRight;
    [SerializeField] private GameObject _playerDamage01, _playerDamage02, _playerDamage03, _playerDamage04;
    [SerializeField] private GameObject _bigExplosionPrefab;
    [SerializeField] private GameObject _lateralLaserCanonLeft, _lateralLaserCanonRight;

    [SerializeField] private GameObject _leftEngineDamage, _rightEngineDamage;

    [SerializeField] private bool _hasPlayerLaserCooledDown = false;
    
    [SerializeField] private bool _gameFirstStart = true;
    [SerializeField] private bool _asteroidDestroyed = false;
    [SerializeField] private bool _isPlayerTripleShotActive = false, _isPlayerShieldActive = false, _isPlayerSpeedBoostActive = false;

    public List<GameObject> poolDamageAnimations = new List<GameObject>();
    public List<GameObject> activatedDamageAnimations = new List<GameObject>();

    private CameraShaker _camera;

    [Header("Thruster Core Variables")]
    [SerializeField] private int coreTempDecrease;
    [SerializeField] private bool _coreTempCooledDown = true;
    [SerializeField] private bool _hasPlayerThrustersCooledDown = true;
    public ThrustersCoreTemp thrustersCoreTemp;
    public int maxCoreTemp = 1000;
    public int currentCoreTemp = 0;
    public bool canPlayerUseThrusters = false;
    public bool resetExceededCoreTempWarning = false;

    [SerializeField] private int _shieldHits = 0;
    [SerializeField] private float _playerShieldAlpha = 1.0f;

    [SerializeField] private GameObject _playerHomingMissilePrefab;
    [SerializeField] private bool _isPlayerHomingMissilesActivate = false; 
    [SerializeField] private int _homingMissileCount = 0;

    [SerializeField] private bool _isPlayerLateralLaserActive = false; 

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShaker>();

        currentCoreTemp = 0;
        thrustersCoreTemp.SetCoreTemp(currentCoreTemp);
        canPlayerUseThrusters = false;

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
            CalculateThrustersScale();
            ThrusterCoreLogic();

            if (Input.GetKeyDown(KeyCode.Space) && _hasPlayerLaserCooledDown)
            {
                PlayerFireLaser();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_isPlayerHomingMissilesActivate == true && _homingMissileCount > 0)
            {
                FireHomingMissile();
            }
        }

        if (_homingMissileCount == 0)
        {
            _isPlayerHomingMissilesActivate = false;
        }
    }

    private void FireHomingMissile()
    {
        Instantiate(_playerHomingMissilePrefab, transform.position + new Vector3(0, -0.141f, 0), Quaternion.identity);
        _homingMissileCount--;
        _uiManager.UpdateHomingMissileCount(_homingMissileCount);

    }

    void CalculateThrustersScale()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _hasPlayerThrustersCooledDown && canPlayerUseThrusters)
        {
            PlayerThrustersActivate(5);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerThrustersDeactivate();
        }
    }

    void ThrusterCoreLogic()
    {
        if (currentCoreTemp > 0 && _coreTempCooledDown == true && canPlayerUseThrusters == true)
        {
            currentCoreTemp -= coreTempDecrease;
            thrustersCoreTemp.SetCoreTemp(currentCoreTemp);
        }

        if (currentCoreTemp > 750 && _coreTempCooledDown == true && canPlayerUseThrusters == true)
        {
            _uiManager.CoreTempWarning(true);

            if (resetExceededCoreTempWarning == false)
            {
                resetExceededCoreTempWarning = true;
                StartCoroutine(PlayWarningCoreTempCritical()); 
            }
        }
        else
        {
            _uiManager.CoreTempWarning(false);
        }

        if (currentCoreTemp >= 999 && _coreTempCooledDown == true && canPlayerUseThrusters == true)
        {
            StartCoroutine(PlayWarningCoreTempExceeded());  
            _coreTempCooledDown = false;
            canPlayerUseThrusters = false;

            PlayerCoreTempExceededDrifting();

            _uiManager.CoreShutdown(true);
        }

        if (currentCoreTemp > 250 && _coreTempCooledDown == false)
        {
            currentCoreTemp -= coreTempDecrease;
            thrustersCoreTemp.SetCoreTemp(currentCoreTemp);

            PlayerCoreTempExceededDrifting();
        }

        if (currentCoreTemp == 250 && _coreTempCooledDown == false && canPlayerUseThrusters == false)
        {
            _coreTempCooledDown = true;
            canPlayerUseThrusters = true;
            _uiManager.CoreShutdown(false);
            currentCoreTemp -= coreTempDecrease;
            thrustersCoreTemp.SetCoreTemp(currentCoreTemp);

            _playerThrusterLeft.gameObject.SetActive(true);
            _playerThrusterRight.gameObject.SetActive(true);
            _speed = 5.0f;

            _uiManager.CoreTempStable(true);

            if (transform.rotation.z != 0)
            {
                StartCoroutine(AnimateRotationTowards(this.transform, Quaternion.identity, 1f));
            }
        }
    }

    IEnumerator PlayWarningCoreTempCritical()
    {
        _audioSource.PlayOneShot(_warningCoreTempCritical);
        yield return new WaitForSeconds(3.0f);
        resetExceededCoreTempWarning = false;

    }

    IEnumerator PlayWarningCoreTempExceeded()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_warningCoreTempExceeded);
        yield return new WaitForSeconds(5.0f);
    }

    /*
    //start it wherever you decide to start the animation. On key press, on trigger enter, on whatever.
    //in this example I'm rotating 'this', towards (0,0,0), for 1 second
    StartCoroutine(AnimateRotationTowards(this.transform, Quaternion.identity, 1f));
    */

    IEnumerator AnimateRotationTowards(Transform target, Quaternion rot, float dur)
    {
        PlayClip(_coreTempNominal); //************

        float t = 0f;
        Quaternion start = target.rotation;
        while (t < dur)
        {
            target.rotation = Quaternion.Slerp(start, rot, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        target.rotation = rot;

        _hasPlayerLaserCooledDown = true;
        _uiManager.CoreTempStable(false);
    }

    void PlayerCoreTempExceededDrifting()
    {
        _playerThrusterLeft.gameObject.SetActive(false);
        _playerThrusterRight.gameObject.SetActive(false);
        _hasPlayerLaserCooledDown = false;
        _speed = 0.25f;
        transform.Rotate(Vector3.forward * -50f * Time.deltaTime);
    }

    void PlayerThrustersActivate(int coreTempIncrease)
    {
        currentCoreTemp += coreTempIncrease;
        thrustersCoreTemp.SetCoreTemp(currentCoreTemp);
        if (currentCoreTemp > maxCoreTemp)
        {
            currentCoreTemp = maxCoreTemp;
        }

        _speed = 10.0f;
    }

    void PlayerThrustersDeactivate()
    {
        _speed = 5.0f;

        if (_isPlayerSpeedBoostActive == true)
        {
            _speed *= _speedMultiplier;
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (_asteroidDestroyed == false)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 1.5f), 0);
        }

        if (_asteroidDestroyed == true)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 3.0f), 0);
        }

        if (transform.position.x > 10.25f)
        {
            transform.position = new Vector3(-10.25f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.25f)
        {
            transform.position = new Vector3(10.25f, transform.position.y, 0);
        }
    }

    public void PlayerFireLaser()
    {
        if (_ammoCount <= 0)
        {
            _ammoCount = 0;
            return;
        }
   
        _ammoCount--;
        _uiManager.UpdateAmmoCount(_ammoCount);

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

        if (_isPlayerLateralLaserActive == true)
        {
            Quaternion rotationLeft = Quaternion.Euler(new Vector3(0, 0, 90));
            Quaternion rotationRight = Quaternion.Euler(new Vector3(0, 0, 270));
            Instantiate(_playerLateralLaserPrefab, new Vector3(transform.position.x - 0.93f, transform.position.y + 0.024f, transform.position.z), rotationLeft);
            Instantiate(_playerLateralLaserPrefab, new Vector3(transform.position.x + 0.93f, transform.position.y + 0.024f, transform.position.z), rotationRight);
        }

        _hasPlayerLaserCooledDown = false;
        StartCoroutine(PlayerLaserCoolDownTimer());

        PlayClip(_playerLaserShotAudioClip);
    }

    public void PlayerRegularAmmo()
    {
        _ammoCount += (Random.Range(3, 6));

        if (_ammoCount > 25)
        {
            _ammoCount = 25;
        }
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    public void PlayerHomingMissiles()
    {
        _homingMissileCount = _homingMissileCount + 5;
        if (_homingMissileCount > 25)
        {
            _homingMissileCount = 25;
        }
        _uiManager.UpdateHomingMissileCount(_homingMissileCount);
        _isPlayerHomingMissilesActivate = true;
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
            _shieldHits++;

            switch (_shieldHits)
            {
                case 1:
                    _playerShieldAlpha = 0.75f;
                    PlayClip(_playerShields65AudioClip);
                    _playerShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
                    break;
                case 2:
                    _playerShieldAlpha = 0.40f;
                    PlayClip(_playerShields35AudioClip);
                    _playerShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
                    break;
                case 3:
                    _isPlayerShieldActive = false;
                    PlayClip(_playerShieldsDepletedAudioClip);
                    _playerShield.SetActive(false);
                    break;
            }
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
                ResetStateOfCore();
                StartCoroutine(DisperseHomingMissiles());

                _isPlayerLateralLaserActive = false;
                _lateralLaserCanonLeft.SetActive(false);
                _lateralLaserCanonRight.SetActive(false);

                StartCoroutine(ResetPlayerPosition());
            }
        }

        if (_lives < 1)
        {
            ResetStateOfCore();
            _spawnManager.OnPlayerDeath();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    IEnumerator ResetPlayerPosition()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        _speed = 0;
        _hasPlayerLaserCooledDown = false;
        canPlayerUseThrusters = false;

        _uiManager.ReadySetGo();
        yield return new WaitForSeconds(0.8f);
        transform.position = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(3.2f);

        if (_asteroidDestroyed == true)
        {
            _hasPlayerLaserCooledDown = true;
            canPlayerUseThrusters = true;
        }

        GetComponent<BoxCollider2D>().enabled = true;
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

    public void ResetStateOfCore()
    {
        _audioSource.Stop();
        transform.rotation = Quaternion.identity;
        _uiManager.CoreTempStable(false);
        _uiManager.CoreTempWarning(false);
        _uiManager.CoreShutdown(false);
        _coreTempCooledDown = true;
        currentCoreTemp = 0;
        thrustersCoreTemp.SetCoreTemp(currentCoreTemp);
        _playerThrusterLeft.gameObject.SetActive(true);
        _playerThrusterRight.gameObject.SetActive(true);
    }

    public void AsteroidDestroyed()
    {
        _asteroidDestroyed = true;
        StartCoroutine(WarningIncomingWave(3.0f));
    }

    public void AsteroidBlockingSensors()
    {
        PlayClip(_asteroidBlockingSensors);
        StartCoroutine(WeaponsFree());
    }

    IEnumerator WeaponsFree()
    {
        yield return new WaitForSeconds(5.0f);
        _hasPlayerLaserCooledDown = true;
        canPlayerUseThrusters = true;
    }


    public void TripleShotActivate()
    {
        _isPlayerTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownTimer());
    }

    public void LateralLaserShotActive()
    {
        _isPlayerLateralLaserActive = true;
        _lateralLaserCanonLeft.SetActive(true);
        _lateralLaserCanonRight.SetActive(true);
        StartCoroutine(LateralShotPowerDownTimer());
    }

    public void SpeedBoostActivate()
    {
        if (_isPlayerSpeedBoostActive == false) // only give the Player a temp speed boost if the PowerUp is not already collected
        {
            _isPlayerSpeedBoostActive = true;
            _speed *= _speedMultiplier;
            StartCoroutine(SpeedBoostPowerDownTimer());
        }
        else
        {
            return;
        }    
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
            PlayClip(_shipRepairsUnderwayAudioClip);
        }
    }

    public void ShieldActivate()
    {
        _isPlayerShieldActive = true;
        _playerShield.SetActive(true);
        PlayClip(_playerShields100AudioClip);
        _shieldHits = 0;
        _playerShieldAlpha = 1.0f;
        _playerShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
    }

    public void HomingMissilesActivate()
    {
        _isPlayerHomingMissilesActivate = true;
    }

    IEnumerator DisperseHomingMissiles() // upon player destruction, scatters remaining homing missiles around the screen to be picked up by
                                         // player on his next life.
    {
        yield return new WaitForSeconds(0.5f);

        int homingMissilesRemaining = _homingMissileCount / 5;
        for (int i = 0; i < homingMissilesRemaining; i++)
        {
            float x = Random.Range(-6f, 6f);
            float y = Random.Range(0f, -3.5f);
            Instantiate(_playerMissilesDispersed, new Vector3(x, y, transform.position.z), Quaternion.identity);

            if (transform.position.x > 9.75f)
            {
                transform.position = new Vector3(9.5f, y, transform.position.z);
            }
            else if (transform.position.x < -9.75f)
            {
                transform.position = new Vector3(-9.5f, y, transform.position.z);
            }
        }
        _homingMissileCount = 0;
        _uiManager.UpdateHomingMissileCount(_homingMissileCount);
    }

    IEnumerator TripleShotPowerDownTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _isPlayerTripleShotActive = false;
    }

    IEnumerator LateralShotPowerDownTimer()
    {
        yield return new WaitForSeconds(15.0f);
        _isPlayerLateralLaserActive = false;
        _lateralLaserCanonLeft.SetActive(false);
        _lateralLaserCanonRight.SetActive(false);
    }

    IEnumerator SpeedBoostPowerDownTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        _isPlayerSpeedBoostActive = false;
    }

    IEnumerator WarningIncomingWave(float time)
    {
        yield return new WaitForSeconds(time);
        PlayClip(_warningIncomingWave);
        _spawnManager.StartSpawning();
    }
}
