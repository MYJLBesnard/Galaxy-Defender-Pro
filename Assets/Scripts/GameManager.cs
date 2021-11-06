// ------------------------------------------------------------------------
// FILE	:	GameManager.cs
// DESC	:	Contains the Game Manager class and support classes
// ------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

// ------------------------------------------------------------------------
// Class	:	LevelInfo
// Desc		:	Contains the game settings for a given level of the game
// ------------------------------------------------------------------------
[Serializable]
public class LevelInfo
{
    public string Name = null;  // Name of the Level
    public int SizeOfWave = 10;  // How many enemy ships in the wave
    public float EnemySpeed = 5.0f;    // Default speed of Enemy ships(seconds)
    public float EnemyLaserSpeed = 8.0f;     // Default speed of Enemy laser(seconds)
    public float EnemyRateOfSpawningd = 5.0f;   // How often Enemy spawns (seconds)
    public float EnemyRateOfFire = 2.5f; // How often Enemy ships fire (seconds)
    public float PowerUpRateOfSpawning = 3.5f; // How often Power-Up spawns (seconds)
    public float EnemySensorRange = 3.0f; // How far the RayCast can sense a hit
    public float EnemyMineLayerChance = 5.0f; // % (out of 100) that a Enemy Mine Layer will spawn
}

// ---------------------------------------------------------------------------------
// Class	:	GameManager
// Desc		:	The GameManager is game global meaning that it is loaded once
//				at game startup and stays alive across all scene loads. It 
//				should contains information about all the levels of the game as
//				well as manages the player's current status - lives left,
//				current level and score. It also manages loading/saving
//				the highscore table to disk.
//				The GameManager game object should also have an Audio source
//				attached which it will use to manage the playback and fades
//				of game music.
// ---------------------------------------------------------------------------------
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver = false;

    // A list of all the levels in the game (Inspector Assigned)
    [SerializeField]
    private List<LevelInfo> Waves = new List<LevelInfo>();

    // A list of properties allowing external objects access to all the properties of the current level
    public int howManyLevels {  get { return Waves.Count; } }
    public string currentLevelName { get { return Waves[_currentWave].Name; } }
    public int currentSizeOfWave { get { return Waves[_currentWave].SizeOfWave; } }
    public float currentEnemySpeed { get { return Waves[_currentWave].EnemySpeed; } }
    public float currentEnemyLaserSpeed { get { return Waves[_currentWave].EnemyLaserSpeed; } }
    public float currentEnemyRateOfSpawning { get { return Waves[_currentWave].EnemyRateOfSpawningd; } }
    public float currentEnemyRateOfFire { get { return Waves[_currentWave].EnemyRateOfFire; } }
    public float currentPowerUpRateOfSpawning { get { return Waves[_currentWave].PowerUpRateOfSpawning; } }
    public float currentEnemySensorRange {  get { return Waves[_currentWave].EnemySensorRange; } }
    public float currentEnemyMineLayerChance { get { return Waves[_currentWave].EnemyMineLayerChance; } }


    // A list of AudioClips that can be played (such as varioius music tracks) which we
    // can instruct the GameManager to play by index
    [SerializeField]
    private List<AudioClip> MusicClips = new List<AudioClip>();

    // Used to cache AudioSource component
    private AudioSource _music = null;

    // Current music clip in the above list that is currently playing
    private int _currentClip = -1;

    // Number of lives remaining
    private int _lives = 3;
    public int lives { get { return _lives; } }

    // Play's current score
    private int _currentScore = 0;
    public int currentScore { get { return _currentScore; } }

    // Current level the player is on
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

    // -----------------------------------------------------------------------------
    // Name	:	Awake
    // Desc	:	Initialize the Game Manager state and loading
    //			previous scores from disk
    // -----------------------------------------------------------------------------
    private void Awake()
    {
        // Init Game State
        _lives = 3;
        _currentScore = 0;
        _currentWave = 0;

        // This is a app global object which
        // should survive scene loads.
        DontDestroyOnLoad(gameObject);
    }

    // ----------------------------------------------------------------------------
    // Name	:	Start
    // Desc	:	Cache audio component reference and make sure at startup that all
    //			music is turned off.
    // ----------------------------------------------------------------------------
    void Start()
    {
        // Cache AudioSource component is available
        _music = GetComponent<AudioSource>();

        // If an audio source exists
        if (_music)
        {
            // Make sure this isn't set to play on awake, make sure volume is initially
            // zero, set to loop, and that the source is not playing.
            _music.playOnAwake = false;
            _music.loop = true;
            _music.volume = 0;
        }
    }

    // -----------------------------------------------------------------------------
    // Name	:	PlayMusic
    // Desc	:	Plays an audio clip from the MusicClips list at the passed index
    //			and fades the music in over the specified duration.
    // -----------------------------------------------------------------------------
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

    // -------------------------------------------------------------------------------
    // Name	:	StopMusic
    // Desc	:	Stop any music that is playing by fading it out over the specified
    //			fade-time.
    // -------------------------------------------------------------------------------
    public void StopMusic(float fade = 2.0f)
    {
        // If no audio source component is attached to this game object then return
        if (!_music) return;

        // Set the current clip to -1 (none playing)
        _currentClip = -1;

        // Fade out the current clip
        StartCoroutine(FadeOutMusic(fade));
    }

    // ------------------------------------------------------------------------------
    // Name	:	FadeInMusic - Coroutine
    // Desc	:	The function that does the actual fading
    // ------------------------------------------------------------------------------
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


    // ------------------------------------------------------------------------------------
    // Name	:	FadeOutMusic	-	Coroutine
    // Desc :	Fades out the currently playing clip
    // ------------------------------------------------------------------------------------
    private IEnumerator FadeOutMusic(float fade)
    {
        // Minimum fade of 1.0
        if (fade < 1.0f) fade = 1.0f;

        // Must have an audio source
        if (_music)
        {
            // Force initial value to 1.0f
            _music.volume = 1.0f;

            // Reset timer
            float timer = 0.0f;

            // Loop for the fade time constantly calculting the volume factor
            while (timer < fade)
            {
                timer += Time.deltaTime;
                _music.volume = 1.0f - timer / fade;
                yield return null;
            }

            // Fade-out complete so make sure value is zero and stop music playing
            _music.volume = 0.0f;
            _music.Stop();
        }
    }

    // ----------------------------------------------------------------------------
    // Name	:	IncreaseScore
    // Desc	:	Called by the main SceneManager everytime an invader gets killed.
    // ----------------------------------------------------------------------------
    public void IncreaseScore(int points)
    {
        _currentScore += points;
    }

    // -------------------------------------------------------------------------------
    // Name	:	StartNewGame
    // Desc	:	Reset current score, number of lives and load the game level.
    // -------------------------------------------------------------------------------
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

    // -------------------------------------------------------------------------------
    // Name	:	WaveComplete
    // Desc	:	Called by the game scene when a level has been complete. This simply
    //			increases the current game level.
    // -------------------------------------------------------------------------------
    public void WaveComplete()
    {
            if (_currentWave < Waves.Count - 1)
                _currentWave++;
    }

    // -------------------------------------------------------------------------------
    // Name	:	DecreaseLives
    // Desc	:	Decreases the number of lives and returns the new life count
    // -------------------------------------------------------------------------------
    public int DecreaseLives()
    {
        if (_lives > 0) _lives--;
        return _lives;
    }

    // ---------------------------------------------------------------------
    // Name	:	QuitGame
    // Desc	:	Loads the closing credits. This is called by the MainMenu
    //			scene when the user presses ESC.
    // ---------------------------------------------------------------------
    public void QuitGame()
    {
        SceneManager.LoadScene("Credits");
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            Awake();
            PlayMusic(2, 5.0f);
            SceneManager.LoadScene(2); // Restart New Game
        }

        if (Input.GetKeyDown(KeyCode.M) && _isGameOver == true)
        {
            PlayMusic(3, 5.0f);
            SceneManager.LoadScene(1); // Main Menu Scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); // Quits the Application
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }





}
