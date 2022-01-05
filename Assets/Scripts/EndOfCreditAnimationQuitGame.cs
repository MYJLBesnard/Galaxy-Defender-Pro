using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndOfCreditAnimationQuitGame : MonoBehaviour
{
    private GameManager _gameManager;
    public Image fadedScreen;
    public GameObject fader;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void QuitTheGame(string message)
    {
        if (message.Equals("CreditsAreFinished"))
        {
            FadeOut();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        fader.SetActive(true);
        fadedScreen.color = new Color(0, 0, 0, 0);

        float duration = 2.0f;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            fadedScreen.color = new Color(fadedScreen.color.r, fadedScreen.color.g, fadedScreen.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(LoadCreditsDelay());
    }

    IEnumerator LoadCreditsDelay() // Loads a new game
    {
        yield return new WaitForSeconds(2.0f);
        FadeOutMusicToCredits();
    }

    public void FadeOutMusicToCredits() // Quits and fades out to the credits scene
    {
        _gameManager.StopMusic(2.0f);
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame() // Loads credit scene
    {
        yield return new WaitForSeconds(2.0f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
