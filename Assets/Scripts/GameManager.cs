using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable] public class LevelInfo // Contains the game settings for a given level of the game
{
    public int LevelNumber = 1;  // Number of Level
    public string Name = null;  // Name of the Level
    public int SizeOfWave = 10;  // How many enemy ships in the wave
    public float EnemySpeed = 5.0f;    // Default speed of Enemy ships(seconds)
    public float EnemyLaserSpeed = 8.0f;     // Default speed of Enemy laser(seconds)
    public float PowerUpSpeed = 0.5f; // Default speed of Power Ups
    public float EnemyRateOfSpawning = 5.0f;   // How often Enemy spawns (seconds)
    public float EnemyRateOfFire = 2.5f; // How often Enemy ships fire (seconds)
    public float PowerUpRateOfSpawning = 3.5f; // How often Power-Up spawns (seconds)
    public float EnemySensorRange = 3.0f; // How far the RayCast can sense a hit
    public float EnemyMineLayerChance = 5.0f; // % (out of 100) that a Enemy Mine Layer will spawn
    public float BossEnemySpeed = 1.5f;    // Default speed of Boss Enemy ships(seconds)
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private EndOfLevelDialogue _endOfLevelDialogue;


    public int difficultyLevel = 1; // default difficulty level if not modified in Options Scene
    public int graphicQualityLevel = 1; // default graphic quality set to Low
    public float musicVolume = 0f;
    public float SFXVolume = 0f;
    public float dialogueVolume = 0f;
    public bool isPlayerDestroyed = false;
    public bool isBossDefeated = false;
    public bool enemyMineLayerDirectionRight = true;
    public bool continueToNextDifficultyLevel = false;
    public bool readyToLoadNextScene = false;
    public bool comingFromInstructionsScene = false;

    // A list of all the levels in the game (Inspector Assigned)
    [SerializeField] private List<LevelInfo> Waves = new List<LevelInfo>();

    // A list of properties allowing external objects access to all the properties of the current level.
    // These are modified by the difficulty level selected.
    public int howManyLevels { get { return Waves.Count; } }
    public int currentLevelNumber { get { return Waves[_currentWave].LevelNumber; } }
    public string currentLevelName { get { return Waves[_currentWave].Name; } }
    public int currentSizeOfWave { get { return Waves[_currentWave].SizeOfWave + difficultyLevel; } }
    public float currentEnemySpeed { get { return Waves[_currentWave].EnemySpeed + difficultyLevel; } }
    public float currentEnemyLaserSpeed { get { return Waves[_currentWave].EnemyLaserSpeed + difficultyLevel; } }
    public float currentEnemyRateOfSpawning { get { return Waves[_currentWave].EnemyRateOfSpawning - difficultyLevel; } }
    public float currentEnemyRateOfFire { get { return Waves[_currentWave].EnemyRateOfFire - difficultyLevel; } }
    public float currentPowerUpSpeed { get { return Waves[_currentWave].PowerUpSpeed + (difficultyLevel / 2); } }
    public float currentPowerUpRateOfSpawning { get { return Waves[_currentWave].PowerUpRateOfSpawning + difficultyLevel; } }
    public float currentEnemySensorRange { get { return Waves[_currentWave].EnemySensorRange + difficultyLevel; } }
    public float currentEnemyMineLayerChance { get { return Waves[_currentWave].EnemyMineLayerChance - (currentLevelNumber * difficultyLevel); } }
    public float currentBossEnemySpeed { get { return Waves[_currentWave].BossEnemySpeed + (difficultyLevel / 2); } }

    // A list of AudioClips that can be played
    [SerializeField] private List<AudioClip> MusicClips = new List<AudioClip>();

    // Used to cache AudioSource component
    private AudioSource _music = null;

    // Music clip from the above list that is currently playing
    private int _currentClip = -1;

    // Number of lives remaining
    private int _lives = 3;
    public int lives { get { return _lives; } }

    // Player's current score
    private int _currentScore = 0;
    public int currentScore { get { return _currentScore; } }

    // Player's current level
    private int _currentWave = 0;
    public int currentWave { get { return _currentWave; } }

    // Singleton Accessor - Only one GameManager should ever exist
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
            }
            return _instance;
        }
    }

    private void Awake() // Initialize the Game Manager state 
    {
        // Init Game State
        _lives = 3;
        _currentScore = 0;
        _currentWave = 0;

        // This is a app global object which
        // should survive scene loads.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicVolume = -10f;
        SFXVolume = -13f;
        //dialogueVolume = 0f;

        // Cache AudioSource component is available
        _music = GetComponent<AudioSource>();

        // If an audio source exists
        if (_music)
        {
            // Make sure this isn't set to play on awake, make sure volume is initially
            // zero, set to loop, and that the source is not playing.
            _music.playOnAwake = false;
            _music.loop = true;
            _music.volume = 0f;
        }
    }

    public void PlayMusic(int clip, float fade)
    {
        // If no audio source component is attached to this game object then return.
        if (!_music)
        {
            _music = GetComponent<AudioSource>();
        }

        // If clip index is out of range then return
        if (MusicClips == null || MusicClips.Count <= clip || MusicClips[clip] == null)
        {
            Debug.LogError("Audio Clip index is out of range.");
            return;
        }

        // If the clip specified is current already playing then return
        if (_currentClip == clip && _music.isPlaying)
        {
            Debug.LogError("Clip already playing.");
            return;
        }

        // Make a note of the clip that we are about to play as this will become the current
        // playing clip
        _currentClip = clip;
        Debug.Log("The current clip playing is: " + clip);

        // Start a coroutine to fade the music in over time
        StartCoroutine(FadeInMusic(clip, fade));
    }

    public void StopMusic(float fade = 2.0f)
    {
        // If no audio source component is attached to this game object then return
        if (!_music) return;

        // Set the current clip to -1 (none playing)
        _currentClip = -1;

        // Fade out the current clip
        StartCoroutine(FadeOutMusic(fade));
    }

    private IEnumerator FadeInMusic(int clip, float fade)
    {
        // Minimum fade of 0.1 seconds
        if (fade < 0.1f) fade = 0.1f;

        // We can only proceed if we have an audio source
        if (_music)
        {
            // If any music is current playing then stop it. This class assumes external
            // script will have requested that any previous music be faded out first.
            _music.volume = 0.0f;
            _music.Stop();

            // Assign the new clip to the audio source and start playing it
            _music.clip = MusicClips[clip];
            _music.Play();

            // Lets start recording the time that passes and fade in the volume
            float timer = 0.0f;

            // while we are still within the fade time and volume clamped at 50%
            while (timer <= fade && _music.volume < 0.5f)
            {
                // Increment timer
                timer += Time.deltaTime;

                // Calculcate current volume factor
                _music.volume = timer / fade;
                yield return null;
            }

            // Fadein is complete, clamped at 50%
            _music.volume = 0.5f;
        }
    }

    private IEnumerator FadeOutMusic(float fade)
    {
        // Minimum fade of 1.0
        if (fade < 1.0f) fade = 1.0f;

        // Must have an audio source
        if (_music)
        {
            // Force initial value to current volume clamped value
            _music.volume = 0.5f;

            // Reset timer
            float timer = 0.0f;

            // Loop for the fade time constantly calculting the volume factor
            while (timer < fade)
            {
                timer += Time.deltaTime;
                _music.volume = 0.5f - timer / fade;
                yield return null;
            }

            // Fade-out complete so make sure value is zero and stop music playing
            _music.volume = 0.0f;
            _music.Stop();
            readyToLoadNextScene = true;
        }
    }

    public void IncreaseScore(int points)
    {
        _currentScore += points;
    }

    public void StartNewGame()
    {
        // Reset score and lives.
        _currentScore = 0;
        _currentWave = 0;
        _lives = 3;

        // Stop any music (3 second fade - ie from main menu to game scene)
        StopMusic(3);

        // Load the game scenne and start playing.
        SceneManager.LoadScene("Game");
    }

    public void WaveComplete()
    {
        if (_currentWave < Waves.Count - 1)
            _currentWave++;
    }

    public int DecreaseLives()
    {
        if (_lives > 0) _lives--;
        return _lives;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopMusic(2.0f);
            Application.Quit(); // Quits the Application, no credits
        }

        if (Input.GetKeyDown(KeyCode.Y) && isBossDefeated == true)
        {
            _endOfLevelDialogue.playerAcceptsMsn = true;
            _endOfLevelDialogue.GeneratePlayerDecision();
        }
        if (Input.GetKeyDown(KeyCode.N) && isBossDefeated == true)
        {
            _endOfLevelDialogue.playerAcceptsMsn = false;
            _endOfLevelDialogue.GeneratePlayerDecision();
        }
    }

    public void GameOver()
    {
        isPlayerDestroyed = true;
        
 //       _isGameOver = true;
    }

    public void EnemyBossDefeated()
    {
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        _endOfLevelDialogue = GameObject.Find("DialoguePlayer").GetComponent<EndOfLevelDialogue>();

        if (_playerScript == null)
        {
            Debug.LogError("The PlayerScript is null.");
        }

        if (_endOfLevelDialogue == null)
        {
            Debug.LogError("The EndOfLevelDialogue script is null.");
        }

        isBossDefeated = true;
        _endOfLevelDialogue.powerUpAudioIsBossDefeated = true;

        if (difficultyLevel != 4)
        {
            _endOfLevelDialogue.lastLevel = false;
        }
        else if (difficultyLevel == 4)
        {
            _endOfLevelDialogue.lastLevel = true;
            _playerScript.TurnOffContinueOptionText();
        }

        _endOfLevelDialogue.PlaySequentialSounds();
    }

    public void DisplayContinueOptionText()
    {
        if (_endOfLevelDialogue.msgDonePlaying == true)
        {
            _playerScript.ContinueOptionTxt();
            _endOfLevelDialogue.msgDonePlaying = false; // resets value
        }
    }

    public void TurnOffContinueOptionText()
    {
        _playerScript.TurnOffContinueOptionText();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadGame()
    {
        // Reset score and lives.
        _currentScore = 0;
        _currentWave = 0;
        _lives = 3;

        SceneManager.LoadScene("Game");
    }

    public void LoadGameInstructions()
    {
        SceneManager.LoadScene("Instructions"); // Loads options scene
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Credits");
    }

    public void NextLevel() // option to advance to next Difficulty Level when Boss defeated
    {
        isBossDefeated = false;
        _currentWave = 0;
        continueToNextDifficultyLevel = true;
        _playerScript.TurnOffContinueOptionText();
    }

    public void CachePlayerScript()
    {
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (_playerScript == null)
        {
            Debug.LogError("The PlayerScript is null.");
        }
    }

    /*
    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        _fadeImage.color = firstColor;
        float currentTime = 0f;

        while (currentTime < durationFader)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / durationFader);
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOutRoutine()
    {
        _fadeImage.color = lastColor;
        float currentTime = 0f;

        while (currentTime < durationFader)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / durationFader);
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
    */

}
