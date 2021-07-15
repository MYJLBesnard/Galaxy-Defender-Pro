using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private TMP_Text _readyText;

    [SerializeField]
    private TMP_Text _setText;

    [SerializeField]
    private TMP_Text _defendText;

    [SerializeField]
    private TMP_Text _gameOverText;

    [SerializeField]
    private TMP_Text _pressToRestart;

    [SerializeField]
    private TMP_Text _pressForMainMenu;

    [SerializeField]
    private Sprite[] _livesSprite;

    [SerializeField]
    private GameManager _gameManager;


    void Start()
    {
        _scoreText.text = "SCORE: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _pressToRestart.gameObject.SetActive(false);
        _pressForMainMenu.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.Log("Game Manager is NULL.");
        }
    }

    public void ReadySetGo()
    {
        StartCoroutine(CountDownToDefend());
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "SCORE: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _livesSprite[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _pressToRestart.gameObject.SetActive(true);
        _pressForMainMenu.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlicker());
    }

    IEnumerator CountDownToDefend()
    {
        yield return new WaitForSeconds(0.5f);
        _readyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _readyText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _setText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _setText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _defendText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _defendText.gameObject.SetActive(false);
    }

    IEnumerator GameOverTextFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }  
    

}

