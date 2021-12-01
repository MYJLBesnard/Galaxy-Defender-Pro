using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Game"); // Game Scene
        if (GameManager.instance) GameManager.instance.PlayMusic(2, 5.0f);
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options"); // Options Scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}