using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DialogueTestScript : MonoBehaviour
{
    private AudioSource _audioSource;
    public bool playOnAwake = false;
    public bool loop = false;

    public bool lastLevel = false;
    public bool playerAcceptsMsn = true;
    public bool rookie = true;
    public bool spaceCadet = false;
    public bool spaceCaptain = false;
    public bool galaxyDefender = false;

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
    public List<AudioClip> audioMsgNoThreatExists = new List<AudioClip>();

    void Start()
    {
        List<AudioClip> audioMsgThreatStillExists = new List<AudioClip>();
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

        for (int i = (audioMsgNoThreatExists.Count - 1); i >= 0; i--)
        {
            if (audioMsgNoThreatExists[i] == null)
            {
                audioMsgNoThreatExists.RemoveAt(i);
            }
        }

        RookieState(); // sets default rank and difficulty level to "Rookie"
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

    public void SpaceCadetState()
    {
        spaceCadet = true;
        rookie = false;
        spaceCaptain = false;
        galaxyDefender = false;
        _introRank = ranks[1];
        _promoRank = ranks[2];
        lastLevel = false;
    }

    public void SpaceCaptainState()
    {
        spaceCaptain = true;
        rookie = false;
        spaceCadet = false;
        galaxyDefender = false;
        _introRank = ranks[2];
        _promoRank = ranks[3];
        lastLevel = false;
    }

    public void GalaxyDefenderState()
    {
        galaxyDefender = true;
        rookie = false;
        spaceCadet = false;
        spaceCaptain = false;
        _introRank = ranks[3];
        lastLevel = true;
    }

    public void GenerateRandomDialogue()
    {
        audioMsgThreatStillExists.Clear();
        audioMsgNoThreatExists.Clear();

        if (lastLevel == false)
        {
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

            if (playerAcceptsMsn == true)
            {
                int randomDecisionYes = Random.Range(0, decisionYes.Length);
                _decision = decisionYes[randomDecisionYes];
                audioMsgThreatStillExists.Add(_decision);

                _promo = promoted[0];
                audioMsgThreatStillExists.Add(_promo);
                audioMsgThreatStillExists.Add(_promoRank);
            }
            else if (playerAcceptsMsn == false)
            {
                _decision = decisionNo[0];
                audioMsgThreatStillExists.Add(_decision);
                audioMsgThreatStillExists.Add(_introRank);

                int randomRTB = Random.Range(0, returnToBase.Length);
                _rtb = returnToBase[randomRTB];
                audioMsgThreatStillExists.Add(_rtb);


            }
        }
        else if (lastLevel == true)
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
    }

    public void PlaySequentialSounds()
    {
        if (lastLevel == false)
        {
            audioMsgThreatStillExists.Clear();
            GenerateRandomDialogue();
            StartCoroutine(PlayThreatPresent());
        }
        else if (lastLevel == true)
        {
            audioMsgNoThreatExists.Clear();
            GenerateRandomDialogue();
            StartCoroutine(PlayThreatGone());
        }
    }

    IEnumerator PlayThreatPresent()
    {
        if (audioMsgThreatStillExists.Count > 0)
        {
            for (int i = 0; i < audioMsgThreatStillExists.Count; i++)
            {
                _audioSource.PlayOneShot(audioMsgThreatStillExists[i]);

                // wait for the lenght of the clip to finish playing
                yield return new WaitForSeconds(audioMsgThreatStillExists[i].length + 0.15f);
            }
        }
        yield return null;
    }

    IEnumerator PlayThreatGone()
    {
        if (audioMsgNoThreatExists.Count > 0)
        {
            for (int i = 0; i < audioMsgNoThreatExists.Count; i++)
            {
                _audioSource.PlayOneShot(audioMsgNoThreatExists[i]);

                // wait for the lenght of the clip to finish playing
                yield return new WaitForSeconds(audioMsgNoThreatExists[i].length + 0.15f);
            }
        }
        yield return null;
    }

    public void Back()
    {
        SceneManager.LoadScene("Main Menu"); // Game Scene
    }
}

