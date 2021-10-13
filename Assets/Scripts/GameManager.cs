using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Current level the player is on
    private int _currentWave = 0;
    public int currentWave { get { return _currentWave; } }
    public int currentAttackWave;

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

    /*
    // Number of lives remaining
    private int _lives = 3;
    public int lives { get { return _lives; } }

    // Play's current score
    private int _currentScore = 0;
    public int currentScore { get { return _currentScore; } }
    */

    private void Start()
    {
        currentAttackWave = currentWave;
    }

    public void UpdateAttackWave()
    {
        _currentWave = currentAttackWave;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); // Restart New Game
        }

        if (Input.GetKeyDown(KeyCode.M) && _isGameOver == true)
        {
            SceneManager.LoadScene(0); // Main Menu Scene
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



}
