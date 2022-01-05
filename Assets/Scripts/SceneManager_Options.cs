using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SceneManager_Options : MonoBehaviour
{
    private GameManager _gameManager;
    private FadeEffect _fadeEffect;
    public AudioMixer audioMixer;
    public TMP_Dropdown difficultyDropdown;
    public Slider musicSlider;
    public AudioSource audioSource;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _fadeEffect = GameObject.Find("CanvasFader").GetComponent<FadeEffect>();
        audioSource = GetComponent<AudioSource>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL (Options).");
        }

        if (audioSource == null)
        {
            Debug.LogError("The audio source is null.");
        }

        SetDifficulty(_gameManager.difficultyLevel - 1);
        difficultyDropdown.SetValueWithoutNotify(_gameManager.difficultyLevel - 1);

        musicSlider.value = _gameManager.musicVolume;

        _fadeEffect.FadeIn();
    }

    private void Update()
    {
        // Looks for key inputs to set the difficulty level.  Default is Rookie.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { 
            difficultyDropdown.value = 0; // Rookie
            SetDifficulty(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            difficultyDropdown.value = 1; // Space Cadet
            SetDifficulty(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            difficultyDropdown.value = 2; // Space Captain
            SetDifficulty(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            difficultyDropdown.value = 3; // Galaxy Defender
            SetDifficulty(3);
        }

        if (Input.GetKey(KeyCode.Alpha6)) // Music Volume Increase
        {
            float musVolInc = 0.2f;
            float newVol = _gameManager.musicVolume + musVolInc;
            if (newVol > 0)
            {
                _gameManager.musicVolume = 0;
            }
            SetMusicVolume(_gameManager.musicVolume + musVolInc);
            musicSlider.value = _gameManager.musicVolume;
        }

        if (Input.GetKey(KeyCode.Alpha5)) // Music Volume Decrease
        {
            float musVolDec = -0.2f;
            float newVol = _gameManager.musicVolume + musVolDec;
            if (newVol < -20)
            {
                _gameManager.musicVolume = -20;
            }
            SetMusicVolume(_gameManager.musicVolume + musVolDec);
            musicSlider.value = _gameManager.musicVolume;
        }

        if (Input.GetKeyDown(KeyCode.B)) // returns to Main Menu scene
        {
            _fadeEffect.FadeOut();
            StartCoroutine(BackToMainMenuDelay());
        }
    }

    public void SetDifficulty(int val) // Sets difficulty level when dropdown manipulated
    {
            switch (val)
            {
                case 0:
                    _gameManager.difficultyLevel = 1;
                    break;
                case 1:
                    _gameManager.difficultyLevel = 2;
                    break;
                case 2:
                    _gameManager.difficultyLevel = 3;
                    break;
                case 3:
                    _gameManager.difficultyLevel = 4;
                    break;
            }
    }

    public void SetMusicVolume(float volumeMusic) // Sets Music volume when volume slider manipulated.
                                                  // See the SceneManager_Options_SFXVol and SceneManager_Options_DialogueVol
                                                  // scripts to make changes to the SFX or Dialogue volume management.
    {
        audioMixer.SetFloat("Music", volumeMusic);
        _gameManager.musicVolume = volumeMusic;
    }

    public void FadeBackToMainMenu()
    {
        _fadeEffect.FadeOut();
        StartCoroutine(BackToMainMenuDelay());
    }

    IEnumerator BackToMainMenuDelay() // Loads a new game
    {
        yield return new WaitForSeconds(2.0f);
        BackToMainMenu();
    }

    public void BackToMainMenu() // Returns to the Main Menu scene
    {
        _gameManager.comingFromInstructionsScene = false;
        SceneManager.LoadScene("Main Menu");
    }
}
