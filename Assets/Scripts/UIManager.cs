using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerScript _player;
    [SerializeField] private BoxCollider2D _playerBoxCollider2D;
    [SerializeField] private SpawnManager _spawnManager;

    [Header("UI Images")]
    [SerializeField] private Sprite[] _livesSprite;
    [SerializeField] private Image _LivesImg;

    [Header("UI Text Fields")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _readyText;
    [SerializeField] private TMP_Text _setText;
    [SerializeField] private TMP_Text _defendText;
    [SerializeField] private TMP_Text _pressToRestart;
    [SerializeField] private TMP_Text _pressForMainMenu;

    [Header("Game Over Display")]
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private float _letterDisplayDelay;
    [SerializeField] private float _flashDelay;
    [SerializeField] private int _flashCount;

    void Start()
    {
        _scoreText.text = "SCORE: " + 0;
        //_gameOverText.gameObject.SetActive(false);
        _gameOverText.enabled = false;
        _pressToRestart.gameObject.SetActive(false);
        _pressForMainMenu.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _playerBoxCollider2D = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL.");
        }
   
        if (_player == null)
        {
            Debug.Log("The PlayerScript is null.");
        }

        if (_playerBoxCollider2D == null)
        {
            Debug.Log("Player BoxCollider is NULL.");
        }

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is null.");
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
        //_gameOverText.gameObject.SetActive(true);

        _gameOverText.enabled = true;
        string msg = _gameOverText.text;
        _gameOverText.text = null;
        StartCoroutine(GameOverRoutine(msg));

        _pressToRestart.gameObject.SetActive(true);
        _pressForMainMenu.gameObject.SetActive(true);
        //StartCoroutine(GameOverTextFlicker());
    }

    IEnumerator GameOverRoutine(string msg)
    {
        yield return new WaitForSeconds(1.0f);
        WaitForSeconds letterDelay = new WaitForSeconds(_letterDisplayDelay);
        WaitForSeconds flashDelay = new WaitForSeconds(_flashDelay);

        for (int i = 0; i<msg.Length; i++)
        {
            _gameOverText.text += msg[i].ToString();
            yield return letterDelay;
        }

        bool flashGameOver = true;
        int flashCount = 0;

        while (flashGameOver)
        {
            yield return flashDelay;
            _gameOverText.enabled = false;
            yield return flashDelay;
            _gameOverText.enabled = true;

            flashCount++;
            if (flashCount >= _flashCount)
            {
                flashGameOver = false;
            }
        }
    }

    IEnumerator CountDownToDefend()
    {
        _playerBoxCollider2D.GetComponent<BoxCollider2D>().enabled = false;
        _spawnManager.OnPlayerReset();
  
        yield return new WaitForSeconds(0.25f);
        _readyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _readyText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        _setText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _setText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        _defendText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _defendText.gameObject.SetActive(false);

        _playerBoxCollider2D.GetComponent<BoxCollider2D>().enabled = true;
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

