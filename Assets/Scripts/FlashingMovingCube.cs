using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingMovingCube : MonoBehaviour
{
    [SerializeField]
    private int _speed = 5;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       // StartCoroutine(NameOfCoroutine());
        StartCoroutine(MoveThatCube());
    }

    IEnumerator NameOfCoroutine()
    {
        yield return null;
        Debug.Log("Waited for a frame.");

        yield return new WaitForSeconds(3.0f);
        Debug.Log("Waited for 3 seconds");

        yield return StartCoroutine(MoveThatCube());
    }

    IEnumerator MoveThatCube()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if (transform.position.x > 10)
        {
            _speed *= -1;
        }
        else if (transform.position.x < -10)
        {
            _speed *= -1;
        }
        

        yield return null;
    }
}
