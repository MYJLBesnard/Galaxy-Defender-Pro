using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CameraShaker _camera;
    private FadeEffect _fadeEffect;
    private int _score;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private EndOfLevelDialogue _endOfLevelDialogue;

    [Header("AudioClips")]
    public AudioSource audioSource;
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

    [Header("Game Objects")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerLaserPrefab, _playerDoubleShotLaserPrefab, _playerTripleShotLaserPrefab, _playerLateralLaserPrefab, _playerMissilesDispersed;
    [SerializeField] private GameObject _playerShield, _playerHealthPowerUpPrefab;
    [SerializeField] private GameObject _playerThrusterLeft, _playerThrusterRight;
    [SerializeField] private GameObject _playerDamage01, _playerDamage02, _playerDamage03, _playerDamage04;
    [SerializeField] private GameObject _bigExplosionPrefab;
    [SerializeField] private GameObject _lateralLaserCanonLeft, _lateralLaserCanonRight;
    public GameObject enemyBoss;

    [Header("First Start / New Game Variables")]
    [SerializeField] private bool _hasPlayerLaserCooledDown = false;
    [SerializeField] private int _ammoCount = 20;
    [SerializeField] private bool _gameFirstStart = true;
    [SerializeField] private bool _isPlayerInPosition = false;
    public int newGamePowerUpCollected = 0;
    [SerializeField] private bool _asteroidDestroyed = false;

    [Header("Triple Shot Variables")]
    [SerializeField] private bool _isPlayerTripleShotActive = false;
    [SerializeField] private int _numberOfProjectiles = 3;
    [Range(0, 360)] [SerializeField] private float _spreadAngle = 30;
    [SerializeField] private float _playerRateOfFire = 0.15f;
    [SerializeField] private float _speed = 5.0f;
    private float _speedMultiplier = 1.75f;

    [Header("Spped Boost / Lateral Laser Canons")]
    [SerializeField] private bool _isPlayerSpeedBoostActive = false;
    [SerializeField] private bool _isPlayerLateralLaserActive = false;
    Coroutine lateralLaserTimer;

    [Header("Homing Missile Variables")]
    [SerializeField] private GameObject _playerHomingMissilePrefab;
    [SerializeField] private bool _isPlayerHomingMissilesActivate = false;
    [SerializeField] private int _homingMissileCount = 20;

    [Header("Thruster Core Variables")]
    [SerializeField] private bool _coreOnline = true;
    [SerializeField] private int _coreTempDecrease;
    [SerializeField] private bool _coreTempCooledDown = true;
    [SerializeField] private bool _hasPlayerThrustersCooledDown = true;
    public ThrustersCoreTemp thrustersCoreTemp;
    public int maxCoreTemp = 1000;
    public int currentCoreTemp = 0;
    public bool canPlayerUseThrusters = false;
    public bool resetExceededCoreTempWarning = false;

    [Header("Shields / Damage Variables")]
    [SerializeField] private bool _isPlayerShieldActive = false;
    [SerializeField] private int _shieldHits = 0;
    [SerializeField] private float _playerShieldAlpha = 1.0f;
    public List<GameObject> poolDamageAnimations = new List<GameObject>();
    public List<GameObject> activatedDamageAnimations = new List<GameObject>();

    [Header("Tractor Beam Variables")]
    [SerializeField] private GameObject _tractorBeam;
    public TractorBeam tractorBeam;
    public int minTractorBeam = 0;
    public int maxTractorBeam = 1000;
    public int currentTractorBeam = 1000;
    [SerializeField] private bool _canPlayerUseTractorBeam;
    [SerializeField] private bool _isTractorBeamOn = false;
    private Vector3 _scaleChange; // scale of Tractor Beam

    void Start()
    {
        _scaleChange = new Vector3(-0.25f, -0.25f, -0.25f);
        transform.position = new Vector3(0, -12, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audioSource = GetComponent<AudioSource>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShaker>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _fadeEffect = GameObject.Find("CanvasFader").GetComponent<FadeEffect>();
        _endOfLevelDialogue = GameObject.Find("DialoguePlayer").GetComponent<EndOfLevelDialogue>();

        currentTractorBeam = 1000;
        tractorBeam.SetTractorBeam(currentTractorBeam);
        _canPlayerUseTractorBeam = false;

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

        if (audioSource == null)
        {
            Debug.LogError("The audio source is null.");
        }

        if (_camera == null)
        {
            Debug.LogError("The CameraShaker on the Main Camera is null.");
        }

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL.");
        }

        if (_endOfLevelDialogue == null)
        {
            Debug.Log("Dialogue Player is NULL.");
        }

        _gameManager.PlayMusic(1, 5.0f);
        _fadeEffect.FadeIn();
    }

    public void FadeOut() // Fades out the Scene to black
    {
        _fadeEffect.FadeOut();
    }

    void Update()
    {
        // Moves the Player from off screen into start position at -4.5f on the Y-axis
        if (_isPlayerInPosition == false)
        {
            transform.Translate(1.25f * Time.deltaTime * Vector3.up);

            if (transform.position.y >= -4.5f)
            {
                transform.position = new Vector3(transform.position.x, -4.5f, 0);
                _isPlayerInPosition = true;
            }

            if (_isPlayerInPosition == true)
            {
                StartCoroutine(ResetPlayerPosition());
            }
        }

        if (_gameManager.continueToNextDifficultyLevel == true)
        {
            _spawnManager.AdvanceToNextLevel();
            _gameManager.continueToNextDifficultyLevel = false;
        }

        if (_gameFirstStart == false)
        {
            CalculateMovement();
            CalculateThrustersScale();
            ActivateTractorBeam();
            DeactivateTractorBeam(2);
            ThrusterCoreLogic();

            if (_coreOnline == false)
            {
                StartCoroutine(LoseCore());
            }

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

        if (transform.position.x > 13.2f)
        {
            transform.position = new Vector3(-13.2f, transform.position.y, 0);
        }
        else if (transform.position.x < -13.2f)
        {
            transform.position = new Vector3(13.2f, transform.position.y, 0);
        }
    }

    public void ContinueOptionTxt()
    {
        _uiManager.continueOptionTxt.gameObject.SetActive(true);
        _uiManager.playerDecidesYes.gameObject.SetActive(true);
        _uiManager.playerDecidesNo.gameObject.SetActive(true);
    }

    public void TurnOffContinueOptionText()
    {
        _uiManager.continueOptionTxt.gameObject.SetActive(false);
        _uiManager.playerDecidesYes.gameObject.SetActive(false);
        _uiManager.playerDecidesNo.gameObject.SetActive(false);
    }

    public void PlayClip(AudioClip soundEffectClip)
    {
        if (soundEffectClip != null)
        {
            audioSource.PlayOneShot(soundEffectClip);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score); // communicate with the UI to update the score
    }


    // Tractor Beam Logic
    void ActivateTractorBeam()
    {
        if (Input.GetKey(KeyCode.T)) // Turns on the Tractor Beam
        {
            if (_canPlayerUseTractorBeam == true && currentTractorBeam > minTractorBeam)
            {
                CollectPowerUps.isPwrUpTractorBeamActive = true;
                _tractorBeam.SetActive(true);
                _isTractorBeamOn = true;
                PlayerTractorBeamActivate(5);
            }
        }

        void PlayerTractorBeamActivate(int tractorBeamDecrease)
        {
            currentTractorBeam -= tractorBeamDecrease;
            tractorBeam.SetTractorBeam(currentTractorBeam);
            if (currentTractorBeam < 0)
            {
                currentTractorBeam = 0;

                CollectPowerUps.isPwrUpTractorBeamActive = false;
                _tractorBeam.SetActive(false);
                _isTractorBeamOn = false;
            }

            if (_isTractorBeamOn == true)
            {
                _tractorBeam.transform.localScale += _scaleChange * 1f;

                if (_tractorBeam.transform.localScale.y < 4.0f || _tractorBeam.transform.localScale.y > 40.0f)
                {
                    _scaleChange = -_scaleChange * 1f;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.T)) // Turns off the Tractor Beam
        {
            CollectPowerUps.isPwrUpTractorBeamActive = false;
            _tractorBeam.SetActive(false);
            _isTractorBeamOn = false;
        }
    }

    void DeactivateTractorBeam(int tractorBeamIncrease) // replenish TB when T released
    {
        if (currentTractorBeam < maxTractorBeam)
        {
            currentTractorBeam += tractorBeamIncrease;
            tractorBeam.SetTractorBeam(currentTractorBeam);
        }
    }


    // Player Thrusters Logic
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
            currentCoreTemp -= _coreTempDecrease;
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
            currentCoreTemp -= _coreTempDecrease;
            thrustersCoreTemp.SetCoreTemp(currentCoreTemp);

            PlayerCoreTempExceededDrifting();
        }

        if (currentCoreTemp == 250 && _coreTempCooledDown == false && canPlayerUseThrusters == false)
        {
            _coreTempCooledDown = true;
            canPlayerUseThrusters = true;
            _uiManager.CoreShutdown(false);
            currentCoreTemp -= _coreTempDecrease;
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
        _endOfLevelDialogue.PlayDialogueClip(_warningCoreTempCritical);
        yield return new WaitForSeconds(3.0f);
        resetExceededCoreTempWarning = false;
    }

    IEnumerator PlayWarningCoreTempExceeded()
    {
        _endOfLevelDialogue.StopDialogueAudio();
        _endOfLevelDialogue.PlayDialogueClip(_warningCoreTempExceeded);

        yield return new WaitForSeconds(5.0f);
    }

    /*
    //start it wherever you decide to start the animation. On key press, on trigger enter, on whatever.
    //in this example I'm rotating 'this', towards (0,0,0), for 1 second
    StartCoroutine(AnimateRotationTowards(this.transform, Quaternion.identity, 1f));
    */

    IEnumerator AnimateRotationTowards(Transform target, Quaternion rot, float dur)
    {
        _endOfLevelDialogue.PlayDialogueClip(_coreTempNominal);

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
        transform.Rotate(-50f * Time.deltaTime * Vector3.forward);
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


    // Player Laser Logic
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

    IEnumerator PlayerLaserCoolDownTimer()
    {
        yield return new WaitForSeconds(_playerRateOfFire);
        _hasPlayerLaserCooledDown = true;
    }


    // player Damage Logic
    public void Damage() // Damage() using the Lists and random damage locations
    {
        if (_isPlayerShieldActive == true)
        {
            _shieldHits++;

            switch (_shieldHits)
            {
                case 1:
                    _playerShieldAlpha = 0.75f;
                    _endOfLevelDialogue.PlayPowerUpDialogue(_playerShields65AudioClip);
                    _playerShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
                    break;
                case 2:
                    _playerShieldAlpha = 0.40f;
                    _endOfLevelDialogue.PlayPowerUpDialogue(_playerShields35AudioClip);
                    _playerShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
                    break;
                case 3:
                    _isPlayerShieldActive = false;
                    _endOfLevelDialogue.PlayPowerUpDialogue(_playerShieldsDepletedAudioClip);
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
            _gameManager.DecreaseLives();
            _uiManager.UpdateLives(_gameManager.lives);
            _camera.StartDamageCameraShake(0.2f, 0.35f);
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);

            if (_gameManager.lives != 0)
            {
                //ResetDamageAnimationList();
                ResetStateOfCore();
                StartCoroutine(DisperseHomingMissiles());

                _isPlayerLateralLaserActive = false;
                _lateralLaserCanonLeft.SetActive(false);
                _lateralLaserCanonRight.SetActive(false);

                StartCoroutine(ResetPlayerPosition());
            }
        }

        if (_gameManager.lives < 1)
        {
            ResetStateOfCore();
            _spawnManager.OnPlayerDeath();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
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
            _endOfLevelDialogue.PlayPowerUpDialogue(_shipRepairsUnderwayAudioClip);
        }
    }

    public void ShieldActivate()
    {
        _isPlayerShieldActive = true;
        _playerShield.SetActive(true);
        _endOfLevelDialogue.PlayPowerUpDialogue(_playerShields100AudioClip);
        _shieldHits = 0;
        _playerShieldAlpha = 1.0f;
        _playerShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
    }

    IEnumerator ResetPlayerPosition()
    {
        if (_gameFirstStart == false)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        _speed = 0;
        _hasPlayerLaserCooledDown = false;
        canPlayerUseThrusters = false;
        _canPlayerUseTractorBeam = false;
        _gameManager.isPlayerDestroyed = true;
        ResetDamageAnimationList();

        _uiManager.ReadySetGo();
        yield return new WaitForSeconds(0.8f);
        transform.position = new Vector3(0, -4.5f, 0); // Player returns to default start position
        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(3.2f);

        if (_asteroidDestroyed == true)
        {
            _hasPlayerLaserCooledDown = true;
            canPlayerUseThrusters = true;
            _canPlayerUseTractorBeam = true;
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
        _endOfLevelDialogue.StopDialogueAudio();
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

    public void BossDefeatedRestartNextDifficultyLevel()
    {
        _spawnManager.SetupBossAlien(); // sets up Boss Alien (or resets SpawnManager when Player defeats Boss and continues at higher Difficulty Level) 
        StartCoroutine(WarningIncomingWave(3.0f));
    }


    // Start Asteroid Logic
    public void AsteroidBlockingSensors()
    {
        _endOfLevelDialogue.PlayDialogueClip(_asteroidBlockingSensors);
        StartCoroutine(WeaponsFree());
    }

    IEnumerator WeaponsFree()
    {
        yield return new WaitForSeconds(4.5f);
        canPlayerUseThrusters = true;
        _canPlayerUseTractorBeam = true;
        _uiManager.WeaponsFreeMsg();
    }

    public void LaserIsWpnsFree()
    {
        _hasPlayerLaserCooledDown = true;
        newGamePowerUpCollected = 0;
    }

    public void AsteroidDestroyed()
    {
        _asteroidDestroyed = true;
        StartCoroutine(WarningIncomingWave(3.0f));
    }

    IEnumerator WarningIncomingWave(float time)
    {
        yield return new WaitForSeconds(time);
        _endOfLevelDialogue.PlayDialogueClip(_warningIncomingWave);
        _spawnManager.StartSpawning();
    }


    // Homing Missile Logic
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

    private void FireHomingMissile()
    {
        Instantiate(_playerHomingMissilePrefab, transform.position + new Vector3(0, -0.141f, 0), Quaternion.identity);
        _homingMissileCount--;
        _uiManager.UpdateHomingMissileCount(_homingMissileCount);
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


    // Triple Lsaer Shot Logic
    public void TripleShotActivate()
    {
        _isPlayerTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownTimer());
    }

    IEnumerator TripleShotPowerDownTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _isPlayerTripleShotActive = false;
    }


    // Lateral Laser Canon Logic
    public void LateralLaserShotActive()
    {
        if (_isPlayerLateralLaserActive == false) // equips Player with lateral laser canons and starts the power down timer.
        {
            Debug.Log("Lateral Canons collected.");
            _isPlayerLateralLaserActive = true;
            _lateralLaserCanonLeft.SetActive(true);
            _lateralLaserCanonRight.SetActive(true);
            StartLateralLaserTimerCoroutine();
            return;
        }

        if (_isPlayerLateralLaserActive == true) // drops old canons if equipped, loads new ones, and resets the power down timer.
        {
            StopLateralLaserTimerCoroutine();
            DropLateralLaserCanons();
            _isPlayerLateralLaserActive = true;
            _lateralLaserCanonLeft.SetActive(true);
            _lateralLaserCanonRight.SetActive(true);
            StartLateralLaserTimerCoroutine();
        }
    }

    public void StartLateralLaserTimerCoroutine() // starts the coroutine to power down lateral laser canons after 15 seconds
    {
        lateralLaserTimer = StartCoroutine(LateralShotPowerDownTimer());
    }

    public void StopLateralLaserTimerCoroutine() // stops the coroutine to power down lateral laser canons (resets timer)
    {
        StopCoroutine(lateralLaserTimer);
    }

    IEnumerator LateralShotPowerDownTimer()
    {
        yield return new WaitForSeconds(15.0f);
        DropLateralLaserCanons();
    }

    public void DropLateralLaserCanons()
    {
        _isPlayerLateralLaserActive = false;
        _lateralLaserCanonLeft.SetActive(false);
        _lateralLaserCanonRight.SetActive(false);

        Vector3 pxToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
        Instantiate(_spawnManager.depletedLateralLaserCanons, transform.position, Quaternion.identity);

        transform.Translate(Vector3.down * _gameManager.currentPowerUpSpeed * Time.deltaTime);

        if (transform.position.y < -9.0f)
        {
            Destroy(this.gameObject);
        }

        _lateralLaserCanonLeft.SetActive(false);
        _lateralLaserCanonRight.SetActive(false);
    }


    // Spped Boost Logic
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

    IEnumerator SpeedBoostPowerDownTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        _isPlayerSpeedBoostActive = false;
    }


    // Negative Power Up Logic
    public void NegativePowerUpCollision()
    {
        int _itemLost = Random.Range(0, 4);

        switch (_itemLost)
        {
            case 0:
                LoseAmmo();
                break;
            case 1:
                LoseMissiles();
                break;
            case 2:
                _coreOnline = false;
                StartCoroutine(LoseCore());
                break;
            case 3:
                LoseShields();
                break;
        }
    }

    void LoseAmmo()
    {
        _ammoCount -= (Random.Range(5, 10));

        if (_ammoCount < 0)
        {
            _ammoCount = 0;
        }
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    void LoseMissiles()
    {
        _homingMissileCount -= (Random.Range(1, 10));
        if (_homingMissileCount < 0)
        {
            _homingMissileCount = 0;
            _isPlayerHomingMissilesActivate = false;
        }
        _uiManager.UpdateHomingMissileCount(_homingMissileCount);
    }

    IEnumerator LoseCore()
    {
        transform.Rotate(Vector3.forward * -50f * Time.deltaTime);
        _hasPlayerLaserCooledDown = false;
        canPlayerUseThrusters = false;
        _playerThrusterLeft.gameObject.SetActive(false);
        _playerThrusterRight.gameObject.SetActive(false);
        _speed = 0.15f;

        yield return new WaitForSeconds(5.0f);

        canPlayerUseThrusters = true;
        _playerThrusterLeft.gameObject.SetActive(true);
        _playerThrusterRight.gameObject.SetActive(true);
        _speed = 5.0f;

        if (transform.rotation.z != 0)
        {
            StartCoroutine(RotatePlayerUp(this.transform, Quaternion.identity, 1f));
        }   

        _coreOnline = true;
    }

    void LoseShields()
    {
        _isPlayerShieldActive = false;
        _endOfLevelDialogue.PlayPowerUpDialogue(_playerShieldsDepletedAudioClip);
        _playerShield.SetActive(false);
    }

    IEnumerator RotatePlayerUp(Transform target, Quaternion rot, float dur)
        {
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
        }
    }
