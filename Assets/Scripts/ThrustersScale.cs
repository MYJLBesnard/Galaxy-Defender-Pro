using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustersScale : MonoBehaviour
{
    [SerializeField] private PlayerScript _playerScript;
    //[SerializeField] private bool _canThrustersScale = false;

    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.63f, 0.5f);

        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (_playerScript == null)
        {
            Debug.LogError("The PlayerScript is null.");
        }
    }

    void Update()
    {
        if(_playerScript.canPlayerUseThrusters == true)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.localScale = new Vector3(0.3f, 0.63f, 0.5f);
            }
            else
            {
                transform.localScale = new Vector3(0.1f, 0.63f, 0.5f);
            }
        }

        if (_playerScript.canPlayerUseThrusters == false)
        {
            transform.localScale = new Vector3(0.1f, 0.63f, 0.5f); 
        }

    }
}
