using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy1;

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(SpawnRoutine());

    }

    // Update is called once per frame
    void Update()
    {
    }

    // spawn game objects every 5 seconds
    // Create a coroutine of type IEnumerator -- Yield Events
    // use a While loop

    IEnumerator SpawnRoutine()
    {
        // while loop (infinite loop)
        // instantiate enemy prefab
        // yield wait for 5 seconds

        while (true)
        {
            transform.position = new Vector3(-5, 0, 0);
            Instantiate(_enemy1, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

}
