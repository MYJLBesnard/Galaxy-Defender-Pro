using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldAlpha : MonoBehaviour
{

    [SerializeField] public Color playerShieldColor = new Color(0.004f, 0.74f, 1.0f, 1.0f);
    //[SerializeField] private float _shieldStrength = 255f;

    void Start()
    {
        playerShieldColor.a = 1.0f;
    }

    /*
    void Update()
    {
        if (_isPlayerShieldActive == true)
        {

            _shieldHits++;

            if (_shieldHits == 1)
            {
                playerShieldColor.a = 0.68f;
                return;

            }
            else if (_shieldHits == 2)
            {
                playerShieldColor.a = 0.39f;
                return;

            }
        }

           // var playerShieldColor = _playerShield.GetComponent<Renderer>();
        //playerShieldColor.material.SetColor("_color", new Color(0.004f, 0.74f, 1.0f, 1.0f));

        /*
         *  if (_isPlayerShieldActive == true)
        {
            _shieldHits++;

            if (_shieldHits == 1)
            {
                return;
            }
            else if (_shieldHits == 2)
            {
                return;
            }
            else if (_shieldHits == 3)
            {
                _isPlayerShieldActive = false;
                _playerShield.SetActive(false);
                _shieldHits = 0;
                return;
            }
        }
        */
}
