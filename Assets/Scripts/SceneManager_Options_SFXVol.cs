using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SceneManager_Options_SFXVol : MonoBehaviour
{
    private GameManager _gameManager;
    public AudioMixer audioMixer;
    public Slider SFXSlider;
    public AudioSource audioSource;
    public AudioClip testSFXAudioClip;

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

        SFXSlider.value = _gameManager.SFXVolume;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha8)) // SFX Volume Increase
        {
            float SFXvolInc = 0.2f;
            float newVol = _gameManager.musicVolume + SFXvolInc;
            if (newVol > 0)
            {
                _gameManager.musicVolume = 0;
            }
            SetSFXVolume(_gameManager.SFXVolume + SFXvolInc);
            SFXSlider.value = _gameManager.SFXVolume;
        }

        if (Input.GetKey(KeyCode.Alpha7)) // SFX Volume Decrease
        {
            float SFXvolDec = -0.2f;
            float newVol = _gameManager.musicVolume + SFXvolDec;
            if (newVol < -25)
            {
                _gameManager.musicVolume = -25;
            }
            SetSFXVolume(_gameManager.SFXVolume + SFXvolDec);
            SFXSlider.value = _gameManager.SFXVolume;
        }
    }

public void SetSFXVolume(float volumeSFX)
    {
        audioMixer.SetFloat("SFX", volumeSFX);
        _gameManager.SFXVolume = volumeSFX;
        audioSource.PlayOneShot(testSFXAudioClip);
    }
}
