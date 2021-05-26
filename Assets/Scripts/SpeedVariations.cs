using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedVariations : MonoBehaviour
{

    // write a program that increases or decreases the speed of an object
    // use A to decrease, S to increase
    // when speed is gretaer than 20, debug.log "slow down"
    // when speed is 0 print "speed up"
    // can't go below zero

    public GameObject cube;
    public float _speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            _speed += 1;
        }
        else if (Input.GetKeyDown("a"))
        {
            _speed -= 1;
        }

        if (_speed <= 0)
        {
            _speed = 0;
            Debug.Log("Speed up!");
        }
        else if(_speed > 0 && _speed <= 20)
        {
            Debug.Log("Your speed is within limitations.");
        }
        else
        {
            Debug.Log("You are going too fast!  Slow down!");
        }


            cube.transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (cube.transform.position.x > 11.0f)
        {
            cube.transform.position = new Vector3(-11.0f, 0, 0);
        }
        
    }
}
