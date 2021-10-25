using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMagnet : MonoBehaviour
{
    private Vector3 _scaleChange; // scale of Tractor Beam
    [SerializeField] private GameObject _tractorBeam;


    private void Start()
    {
        _scaleChange = new Vector3(4.0f, 4.0f, 4.0f);

    }

    void uodate()
    {
        if (_tractorBeam == true)
        {
            _tractorBeam.transform.localScale += _scaleChange * 5f;

            if (_tractorBeam.transform.localScale.x < 4.0f || _tractorBeam.transform.localScale.x > 40.0f)
            {
                _scaleChange = -_scaleChange * 5f;
            }
        }
    }
}
