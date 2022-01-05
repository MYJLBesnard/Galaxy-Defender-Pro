using UnityEngine;

public class SceneManager_Credits : MonoBehaviour
{
    private GameManager _gameManager;
    private FadeEffect _fadeEffect;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _fadeEffect = GameObject.Find("CanvasFader").GetComponent<FadeEffect>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL.");
        }

        _gameManager.PlayMusic(0, 7.0f);
        _fadeEffect.FadeIn();
    }
}