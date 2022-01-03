using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneManager_Instructions : MonoBehaviour
{
    private GameManager _gameManager;
    private FadeEffect _fadeEffect;
    public Button defaultMsg;
    public Button introMsg;
    public Button backToMainMenu;

    public GameObject defaultBtnData;
    public GameObject introBtnData;
    public GameObject playerInfoData;
    public GameObject HUDInfoData;
    public GameObject scoreInfoData;
    public GameObject thrusterInfoData;
    public GameObject tractorBeamInfoData;
    public GameObject basicPwrUpInfoData;
    public GameObject healthInfoData;
    public GameObject wpnsInfoData;
    public GameObject negPwrUpInfoData;
    public GameObject basicEnemiesData;
    public GameObject advancedEnemiesData;
    public GameObject mineLayerInfoData;
    public GameObject bossInfoData;
    public GameObject currentlyActiveBtn;

    public TMP_Text Default_Message_txt;
    public TMP_Text Intro_message_txt;
    public TMP_Text Player_txt;
    public TMP_Text HUD_txt;
    public TMP_Text Score_txt;
    public TMP_Text Thrusters_txt;
    public TMP_Text Tractor_Beam_txt;
    public TMP_Text Basic_Power_Up_txt;
    public TMP_Text Health_txt;
    public TMP_Text Player_Weapons_txt;
    public TMP_Text Negative_Power_Up_txt;
    public TMP_Text Basic_Enemies_txt;
    public TMP_Text Advanced_Enemies_txt;
    public TMP_Text Mine_Layer_txt;
    public TMP_Text Boss_txt;

    [Header("Computer Display")]
    [SerializeField] private TMP_Text _ComputerScreenText;
    [SerializeField] private float _letterDisplayDelay;

    public bool msgDone = true;
    public bool defaultInstructionComplete = false;
    public bool firstTimePlayingDefault = true;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _fadeEffect = GameObject.Find("CanvasFader").GetComponent<FadeEffect>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL (Instructions).");
        }

        if (_fadeEffect == null)
        {
            Debug.Log("FadeEffect is NULL.");
        }

        _gameManager.PlayMusic(4, 7.0f);
       
        DefaultMsgSelected();
        _letterDisplayDelay = 0.025f;

        _fadeEffect.FadeIn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // returns to Main Menu scene
        {
            _fadeEffect.FadeOut();
            FadeMusicToMainMenuScene();
        }
    }

    public void FadeBackToMainMenu()
    {
        _fadeEffect.FadeOut();
        FadeMusicToMainMenuScene();
    }

    public void FadeMusicToMainMenuScene() // Fades out and loads the Main Menu scene
    {
        _gameManager.StopMusic(2.0f);
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu() // Loads the Main Menu
    {
        yield return new WaitForSeconds(2.0f);
        _gameManager.comingFromInstructionsScene = true;
        _gameManager.LoadMainMenu();
    }

    IEnumerator ComputerScreenSequentialText(string msg)
    {
        msgDone = false;

        yield return new WaitForSeconds(1.0f);
        WaitForSeconds letterDelay = new WaitForSeconds(_letterDisplayDelay);

        for (int i = 0; i < msg.Length; i++)
        {
            _ComputerScreenText.text += msg[i].ToString();
            yield return letterDelay;
        }

        msgDone = true;

        if (defaultInstructionComplete == true && firstTimePlayingDefault == true)
        {
            firstTimePlayingDefault = false;
            introMsg.Select();
        }
    }

    public void DefaultMsgSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            defaultBtnData.SetActive(true);

            _ComputerScreenText = Default_Message_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }

        defaultInstructionComplete = true;
    }

    public void IntroductionSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            introBtnData.SetActive(true);
            currentlyActiveBtn = introBtnData;

            _ComputerScreenText = Intro_message_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void PlayerInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            playerInfoData.SetActive(true);
            currentlyActiveBtn = playerInfoData;

            _ComputerScreenText = Player_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void HUDInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            HUDInfoData.SetActive(true);
            currentlyActiveBtn = HUDInfoData;

            _ComputerScreenText = HUD_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void ScoreInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            scoreInfoData.SetActive(true);
            currentlyActiveBtn = scoreInfoData;

            _ComputerScreenText = Score_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void ThrusterInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            thrusterInfoData.SetActive(true);
            currentlyActiveBtn = thrusterInfoData;

            _ComputerScreenText = Thrusters_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void TractorBeamInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            tractorBeamInfoData.SetActive(true);
            currentlyActiveBtn = tractorBeamInfoData;

            _ComputerScreenText = Tractor_Beam_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void BasicPwrUpInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            basicPwrUpInfoData.SetActive(true);
            currentlyActiveBtn = basicPwrUpInfoData;

            _ComputerScreenText = Basic_Power_Up_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void HealthInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            healthInfoData.SetActive(true);
            currentlyActiveBtn = healthInfoData;

            _ComputerScreenText = Health_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void WpnsInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            wpnsInfoData.SetActive(true);
            currentlyActiveBtn = wpnsInfoData;

            _ComputerScreenText = Player_Weapons_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void NegPwrUpInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            negPwrUpInfoData.SetActive(true);
            currentlyActiveBtn = negPwrUpInfoData;

            _ComputerScreenText = Negative_Power_Up_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void BasicEnemiesInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            basicEnemiesData.SetActive(true);
            currentlyActiveBtn = basicEnemiesData;

            _ComputerScreenText = Basic_Enemies_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void AdvEnemiesInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            advancedEnemiesData.SetActive(true);
            currentlyActiveBtn = advancedEnemiesData;

            _ComputerScreenText = Advanced_Enemies_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void MineLayerInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            mineLayerInfoData.SetActive(true);
            currentlyActiveBtn = mineLayerInfoData;

            _ComputerScreenText = Mine_Layer_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }

    public void BossInfoSelected()
    {
        if (msgDone == true)
        {
            currentlyActiveBtn.SetActive(false);
            bossInfoData.SetActive(true);
            currentlyActiveBtn = bossInfoData;

            _ComputerScreenText = Boss_txt;
            string msg = _ComputerScreenText.text;
            _ComputerScreenText.text = null;
            StartCoroutine(ComputerScreenSequentialText(msg));
        }
    }
}
