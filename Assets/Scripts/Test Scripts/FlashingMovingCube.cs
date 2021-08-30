using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingMovingCube : MonoBehaviour
{
    [SerializeField]
    private int _speed = 5;

    public bool isCoroutineRunning = false;

    void Start()
    {

    }

    void Update()
    {
        MoveThatCube();

        if (Input.GetKeyDown(KeyCode.Space) && isCoroutineRunning == false)
        {
            StartCoroutine(FirstCoroutine());
        }
    }

    public void MoveThatCube()
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
    }

    IEnumerator FirstCoroutine()
    {
        isCoroutineRunning = true;
        Debug.Log("User pressed the Space bar. I'm starting the first coroutine.");

        yield return null;
        Debug.Log("Waited for a frame.");

        yield return new WaitForSeconds(3.0f);
        Debug.Log("Waited for 3 seconds");

        yield return StartCoroutine(PrintAMessage());
        Debug.Log("Back in the original Coroutine.  Ready for the Space bar again...");
        isCoroutineRunning = false;
    }

    IEnumerator PrintAMessage()
    {
        Debug.Log("Began the PrintAMessage() coroutine.");
        yield return null;
        Debug.Log("Hi World!");
        yield return new WaitForSeconds(5f);
        Debug.Log("Waited for 5 seconds. Leaving the PrintAMessage() coroutine.");
    }

   
}
