using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class SceneManager_Bootstrapper : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private TMP_Text _introText1;
    [SerializeField] private TMP_Text _introText2;
    [SerializeField] private TMP_Text _introText3;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL (Bootstrapper).");
        }

        StartCoroutine(FadeInOutIntroText());
        Invoke("LoadMainMenu", 10.0f);

        if (GameManager.instance) GameManager.instance.PlayMusic(3, 2.5f);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    IEnumerator FadeInOutIntroText()
    {
        var pause = new WaitForSeconds(1.75f);

        float duration = 4.0f; //4.0 secs
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            float fov = Mathf.Lerp(150f, 60f, currentTime / duration);
            _introText1.color = new Color(_introText1.color.r, _introText1.color.g, _introText1.color.b, alpha);
            _introText2.color = new Color(_introText2.color.r, _introText2.color.g, _introText2.color.b, alpha);
            _introText3.color = new Color(_introText3.color.r, _introText3.color.g, _introText3.color.b, alpha);

            // Makes the actual change to Field Of View
            Camera.main.fieldOfView = fov;

            currentTime += Time.deltaTime;
            yield return null;
        }

        yield return pause;

        duration = 4.0f; //4.0 secs
        currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            float fov = Mathf.Lerp(60f, 10.0f, currentTime / duration);
            _introText1.color = new Color(_introText1.color.r, _introText1.color.g, _introText1.color.b, alpha);
            _introText2.color = new Color(_introText2.color.r, _introText2.color.g, _introText2.color.b, alpha);
            _introText3.color = new Color(_introText3.color.r, _introText3.color.g, _introText3.color.b, alpha);
            Camera.main.fieldOfView = fov;
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
