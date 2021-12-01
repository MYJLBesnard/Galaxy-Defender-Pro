using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SceneManager_Options : MonoBehaviour
{
    private GameManager _gameManager;
    public AudioMixer audioMixer;
    public TMP_Dropdown difficultyDropdown;
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Slider musicSlider;
    public Slider SFXSlider;
    public AudioSource audioSource;
    public AudioClip testSFXAudioClip;

    Resolution[] resolutions;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL.");
        }

        if (audioSource == null)
        {
            Debug.LogError("The audio source is null.");
        }

        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + " Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
            resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = CurrentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        SetDifficulty(_gameManager.difficultyLevel - 1);
        difficultyDropdown.SetValueWithoutNotify(_gameManager.difficultyLevel - 1);

        SetGraphicsQuality(_gameManager.graphicQualityLevel - 1);
        graphicsDropdown.SetValueWithoutNotify(_gameManager.graphicQualityLevel - 1);

        musicSlider.value = _gameManager.musicVolume;
        SFXSlider.value = _gameManager.SFXVolume;

    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetDifficulty(int val)
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

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        _gameManager.graphicQualityLevel = qualityIndex + 1;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetMusicVolume(float volumeMusic)
    {
        audioMixer.SetFloat("Music", volumeMusic);
        _gameManager.musicVolume = volumeMusic;
    }

    public void SetSFXVolume(float volumeSFX)
    {
        audioMixer.SetFloat("SFX", volumeSFX);
        _gameManager.SFXVolume = volumeSFX;
        audioSource.PlayOneShot(testSFXAudioClip);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game"); // Game Scene
        if (GameManager.instance) GameManager.instance.PlayMusic(2, 5.0f);
    }

    public void Back()
    {
        SceneManager.LoadScene("Main Menu"); // Game Scene
        if (GameManager.instance) GameManager.instance.PlayMusic(3, 5.0f);
    }
}
