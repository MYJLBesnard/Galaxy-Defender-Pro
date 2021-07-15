using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

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
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
