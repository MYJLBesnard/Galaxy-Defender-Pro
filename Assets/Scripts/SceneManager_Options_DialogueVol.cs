using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class SceneManager_Options_DialogueVol : MonoBehaviour
{
    private GameManager _gameManager;
    public AudioMixer audioMixer;
    public Slider dialogueSlider;
    public AudioSource audioSource;
    public AudioClip testDialogueAudioClip;

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

        dialogueSlider.value = _gameManager.dialogueVolume;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0)) // Dialogue Volume Increase
        {
            float diaVolInc = 0.2f;
            float newVol = _gameManager.musicVolume + diaVolInc;
            if (newVol > 0)
            {
                _gameManager.musicVolume = 0;
            }
            SetDialogueVolume(_gameManager.dialogueVolume + diaVolInc);
            dialogueSlider.value = _gameManager.dialogueVolume;
        }

        if (Input.GetKey(KeyCode.Alpha9)) // Dialogue Volume Decrease
        {
            float diaVolDec = -0.2f;
            float newVol = _gameManager.musicVolume + diaVolDec;
            if (newVol < -10)
            {
                _gameManager.musicVolume = -10;
            }
            SetDialogueVolume(_gameManager.dialogueVolume + diaVolDec);
            dialogueSlider.value = _gameManager.dialogueVolume;
        }
    }

public void SetDialogueVolume(float volumeDialogue)
    {
        audioMixer.SetFloat("Dialogue", volumeDialogue);
        _gameManager.dialogueVolume = volumeDialogue;
        audioSource.PlayOneShot(testDialogueAudioClip);
    }
}
