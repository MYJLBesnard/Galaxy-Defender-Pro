using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager_MainMenu : MonoBehaviour
{
    private GameManager _gameManager;
    private FadeEffect _fadeEffect;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _fadeEffect = GameObject.Find("CanvasFader").GetComponent<FadeEffect>();


        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL (Main Menu).");
        }

        if (_fadeEffect == null)
        {
            Debug.Log("FadeEffect is NULL.");
        }

        if (_gameManager.comingFromInstructionsScene == true)
        {
            _gameManager.PlayMusic(3, 2.5f);
        }

        _fadeEffect.FadeIn();
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Slash))
        {
            //_fadeEffect.FadeOut();
            FadeToGameInstructionsScene();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            //_fadeEffect.FadeOut();
            FadeOutToGameScene();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            _fadeEffect.FadeOut();
            StartCoroutine(LoadOptionsDelay());
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _fadeEffect.FadeOut();
            FadeOutToCredits();
        }
    }

    public void FadeOutToGameScene() // Fades out and loads a new game at the default difficulty level (Rookie)
                                     // or to whichever difficulty level was set in the options scene.
    {
        _fadeEffect.FadeOut();
        _gameManager.StopMusic(2.0f);
        StartCoroutine(LoadMainGame());
    }

    IEnumerator LoadMainGame() // Loads a new game
    {
        yield return new WaitForSeconds(2.0f);
        _gameManager.LoadGame();
    }

    public void FadeToGameInstructionsScene() // Fades out and loads the instructions scene
    {
        _fadeEffect.FadeOut();
        _gameManager.StopMusic(2.0f);
        StartCoroutine(LoadInstructions());
    }

    IEnumerator LoadInstructions() // Loads a new game
    {
        yield return new WaitForSeconds(2.0f);
        _gameManager.LoadGameInstructions();
    }

    public void RunOptionsDelayCoroutine()
    {
        _fadeEffect.FadeOut();
        StartCoroutine(LoadOptionsDelay());
    }

    IEnumerator LoadOptionsDelay() // Loads a new game
    {
        yield return new WaitForSeconds(2.0f);
        LoadOptions();
    }

    public void LoadOptions() // Loads the options scene
    {
        SceneManager.LoadScene("Options"); // Loads options scene
    }

    public void FadeOutToCredits() // Quits and fades out to the credits scene
    {
        _gameManager.StopMusic(2.0f);
        StartCoroutine(LoadCredits());
    }

    IEnumerator LoadCredits() // Loads credit scene
    {
        yield return new WaitForSeconds(2.0f);
        _gameManager.QuitGame();
    }
}