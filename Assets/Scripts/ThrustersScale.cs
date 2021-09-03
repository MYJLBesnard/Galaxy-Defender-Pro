using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustersScale : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.63f, 0.5f);
    }

    void Update()
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
}
