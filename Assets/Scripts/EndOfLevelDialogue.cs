using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelDialogue : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerScript _playerScript;

    private AudioSource _audioSource;
    public bool playOnAwake = false;
    public bool loop = false;

    public bool lastLevel = false;
    public bool playerAcceptsMsn = true;
    public bool rookie = true;
    public bool spaceCadet = false;
    public bool spaceCaptain = false;
    public bool galaxyDefender = false;
    public bool msgDonePlaying = false;
    public bool powerUpAudioIsBossDefeated = false;

    public AudioClip[] congratulations;
    public AudioClip[] threatPresent;
    public AudioClip[] threatCleared;
    public AudioClip[] newMsn;
    public AudioClip[] mayNotSurvive;
    public AudioClip[] pleaseAccept;
    public AudioClip[] decisionYes;
    public AudioClip[] decisionNo;
    public AudioClip[] ranks;
    public AudioClip[] returnToBase;
    public AudioClip[] promoted;

    private AudioClip _congrats;
    private AudioClip _introRank;
    private AudioClip _threat;
    private AudioClip _msn;
    private AudioClip _risk;
    private AudioClip _plea;
    private AudioClip _decision;
    private AudioClip _rtb;
    private AudioClip _promo;
    private AudioClip _promoRank;

    public List<AudioClip> audioMsgThreatStillExists = new List<AudioClip>();
    public List<AudioClip> audioMsgPlayerDecision = new List<AudioClip>();
    public List<AudioClip> audioMsgNoThreatExists = new List<AudioClip>();

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();


        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL.");
        }

        if (_playerScript == null)
        {
            Debug.Log("PlayerScript is NULL.");
        }

        List<AudioClip> audioMsgThreatStillExists = new List<AudioClip>();
        List<AudioClip> audioMsgPlayerDecision = new List<AudioClip>();
        List<AudioClip> audioMsgNoThreatExists = new List<AudioClip>();

        // make sure we have a reference to the AudioSource
        if (_audioSource == null)
        {
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                _audioSource = gameObject.GetComponent<AudioSource>();
            }
        }
        _audioSource.playOnAwake = playOnAwake;
        _audioSource.loop = loop;
        
        // this loop just make sure that alll the AudioClips in the soundList are valid
        for (int i = (audioMsgThreatStillExists.Count - 1); i >= 0; i--)
        {
            if (audioMsgThreatStillExists[i] == null)
            {
                audioMsgThreatStillExists.RemoveAt(i);
            }
        }

        for (int i = (audioMsgPlayerDecision.Count - 1); i >= 0; i--)
        {
            if (audioMsgPlayerDecision[i] == null)
            {
                audioMsgPlayerDecision.RemoveAt(i);
            }
        }

        for (int i = (audioMsgNoThreatExists.Count - 1); i >= 0; i--)
        {
            if (audioMsgNoThreatExists[i] == null)
            {
                audioMsgNoThreatExists.RemoveAt(i);
            }
        }

        RookieState(); // sets default rank and difficulty level to "Rookie"
        DetermineDifficultyLevel();
    }

    public void DetermineDifficultyLevel()
    {
        switch (_gameManager.difficultyLevel)
        {
            case 1:
                rookie = true;
                spaceCadet = false;
                spaceCaptain = false;
                galaxyDefender = false;
                _introRank = ranks[0];
                _promoRank = ranks[1];
                lastLevel = false;
                break;
            case 2:
                spaceCadet = true;
                rookie = false;
                spaceCaptain = false;
                galaxyDefender = false;
                _introRank = ranks[1];
                _promoRank = ranks[2];
                lastLevel = false;
                break;
            case 3:
                spaceCaptain = true;
                rookie = false;
                spaceCadet = false;
                galaxyDefender = false;
                _introRank = ranks[2];
                _promoRank = ranks[3];
                lastLevel = false;
                break;
            case 4:
                galaxyDefender = true;
                rookie = false;
                spaceCadet = false;
                spaceCaptain = false;
                _introRank = ranks[3];
                lastLevel = true;
                break;
        }
    }

    public void RookieState()
    {
        rookie = true;
        spaceCadet = false;
        spaceCaptain = false;
        galaxyDefender = false;
        _introRank = ranks[0];
        _promoRank = ranks[1];
        lastLevel = false;
    }

    public void PlaySequentialSounds()
    {
        if (lastLevel == false)
        {
            audioMsgThreatStillExists.Clear();
            GenerateThreatDialogue();
            StartCoroutine(PlayThreatPresent());
        }
        else if (lastLevel == true)
        {
            audioMsgNoThreatExists.Clear();
            GenerateNoThreatDialogue();
            StartCoroutine(PlayThreatGone());
            StartCoroutine(QuitToCredits());
        }
    }

    public void GenerateThreatDialogue()
    {
        audioMsgThreatStillExists.Clear();
        audioMsgPlayerDecision.Clear();
        audioMsgNoThreatExists.Clear();

        int randomCongrats = Random.Range(0, congratulations.Length);
        _congrats = congratulations[randomCongrats];

        // intro rank audio clip played next

        int randomThreatPresent = Random.Range(0, threatPresent.Length);
        _threat = threatPresent[randomThreatPresent];

        int randomMsn = Random.Range(0, newMsn.Length);
        _msn = newMsn[randomMsn];

        int randomRisk = Random.Range(0, mayNotSurvive.Length);
        _risk = mayNotSurvive[randomRisk];

        int randomPlea = Random.Range(0, pleaseAccept.Length);
        _plea = pleaseAccept[randomPlea];

        audioMsgThreatStillExists.Add(_congrats);
        audioMsgThreatStillExists.Add(_introRank);
        audioMsgThreatStillExists.Add(_threat);
        audioMsgThreatStillExists.Add(_msn);
        audioMsgThreatStillExists.Add(_risk);
        audioMsgThreatStillExists.Add(_plea);
    }

    public void GenerateNoThreatDialogue()
    {
        int randomCongrats = Random.Range(0, congratulations.Length);
        _congrats = congratulations[randomCongrats];

        // intro rank audio clip played next

        int randomThreatCleared = Random.Range(0, threatCleared.Length);
        _threat = threatCleared[randomThreatCleared];

        int randomRTB = Random.Range(0, returnToBase.Length);
        _rtb = returnToBase[randomRTB];

        audioMsgNoThreatExists.Add(_congrats);
        audioMsgNoThreatExists.Add(_introRank);
        audioMsgNoThreatExists.Add(_threat);
        audioMsgNoThreatExists.Add(_rtb);
    }

    public void PlayerAccepts()
    {
        playerAcceptsMsn = true;
        GeneratePlayerDecision();
    }

    public void PlayerRefuses()
    {
        playerAcceptsMsn = false;
        GeneratePlayerDecision();
    }

    public void GeneratePlayerDecision()
    {
        if (playerAcceptsMsn == true)
        {
            int randomDecisionYes = Random.Range(0, decisionYes.Length);
            _decision = decisionYes[randomDecisionYes];
            audioMsgPlayerDecision.Add(_decision);

            _promo = promoted[0];
            audioMsgPlayerDecision.Add(_promo);
            audioMsgPlayerDecision.Add(_promoRank);
            _gameManager.difficultyLevel++;
            _gameManager.TurnOffContinueOptionText();
            DetermineDifficultyLevel();
            StartCoroutine(PlayerGivesDecision());
            StartCoroutine(ContinueToNextLevel());
        }
        else if (playerAcceptsMsn == false)
        {
            _decision = decisionNo[0];
            audioMsgPlayerDecision.Add(_decision);
            audioMsgPlayerDecision.Add(_introRank);

            int randomRTB = Random.Range(0, returnToBase.Length);
            _rtb = returnToBase[randomRTB];
            audioMsgPlayerDecision.Add(_rtb);

            _gameManager.TurnOffContinueOptionText();
            //StartCoroutine(PlayerGivesDecision());
            StartCoroutine(PlayerChicken());
        }
    }
    
    public void PlayDialogueClip(AudioClip dialogueClip)
    {
        if (dialogueClip != null)
        {
            _audioSource.PlayOneShot(dialogueClip);
        }
    }

    public void PlayPowerUpDialogue(AudioClip powerUpDialogueClip)
    {
        if (powerUpDialogueClip != null)
        {
            _audioSource.PlayOneShot(powerUpDialogueClip);
        }
    }

    public void StopDialogueAudio()
    {
        _audioSource.Stop();
    }
   
    IEnumerator PlayThreatPresent()
    {
        yield return new WaitForSeconds(3.0f);

        if (audioMsgThreatStillExists.Count > 0)
        {
            for (int i = 0; i < audioMsgThreatStillExists.Count; i++)
            {
                PlayDialogueClip(audioMsgThreatStillExists[i]);

                // wait for the lenght of the clip to finish playing
                yield return new WaitForSeconds(audioMsgThreatStillExists[i].length + 0.15f);
            }
        }

        msgDonePlaying = true;
        _gameManager.DisplayContinueOptionText();

        yield return null;
    }

    IEnumerator PlayerGivesDecision()
    {
        if (audioMsgPlayerDecision.Count > 0)
        {
            for (int i = 0; i < audioMsgPlayerDecision.Count; i++)
            {
                PlayDialogueClip(audioMsgPlayerDecision[i]);

                // wait for the lenght of the clip to finish playing
                yield return new WaitForSeconds(audioMsgPlayerDecision[i].length + 0.15f);
            }
        }

        yield return null;
    }

    IEnumerator PlayThreatGone()
    {
        yield return new WaitForSeconds(3.0f);

        if (audioMsgNoThreatExists.Count > 0)
        {
            for (int i = 0; i < audioMsgNoThreatExists.Count; i++)
            {
                PlayDialogueClip(audioMsgNoThreatExists[i]);

                // wait for the lenght of the clip to finish playing
                yield return new WaitForSeconds(audioMsgNoThreatExists[i].length + 0.15f);
            }
        }
        yield return null;
    }

    IEnumerator ContinueToNextLevel()
    {
        powerUpAudioIsBossDefeated = false; // resets value
        playerAcceptsMsn = false; // resets value
        yield return new WaitForSeconds(7.0f);
        _gameManager.NextLevel();
    }

    IEnumerator QuitToCredits()
    {
        yield return new WaitForSeconds(12.5f);
        RunCreditsDelayCoroutine();
    }

    IEnumerator PlayerChicken()
    {
        if (audioMsgPlayerDecision.Count > 0)
        {
            for (int i = 0; i < audioMsgPlayerDecision.Count; i++)
            {
                PlayDialogueClip(audioMsgPlayerDecision[i]);

                // wait for the lenght of the clip to finish playing
                yield return new WaitForSeconds(audioMsgPlayerDecision[i].length + 0.15f);
            }
        }

        yield return new WaitForSeconds(6.0f);
        RunCreditsDelayCoroutine();
    }

    public void RunCreditsDelayCoroutine()
    {
        _playerScript.FadeOut();
        StartCoroutine(LoadCreditsDelay());
    }

    IEnumerator LoadCreditsDelay() // Loads a new game
    {
        yield return new WaitForEndOfFrame();
        FadeMusicOutToCredits();
    }

    public void FadeMusicOutToCredits() // Quits and fades out to the credits scene
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

