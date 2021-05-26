using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfStatements : MonoBehaviour
{
    // create a program that when you hit the space key, you increment scrore value.  Once score value is greater
    // than 50.  You turn the cube Orange.

    [SerializeField]
    private int _score = 0;

    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        cube.GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _score += 10;

            if (_score == 50)
            {
                Debug.Log("Nice score of " + _score + ". I'm GREEN with envy!");
                cube.GetComponent<Renderer>().material.color = Color.green;
            }

            else if (_score >= 60)

            {
                Debug.Log("Your score is now: " + _score);
            }
        }
    }
}
