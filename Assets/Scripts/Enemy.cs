using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemyOneSpeed;

    [SerializeField]
    private GameObject _enemyOnePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _enemyOneSpeed = Random.Range(2.5f, 4.5f);
    }

    // Update is called once per frame
    void Update()
    { 
        transform.Translate(Vector3.down * _enemyOneSpeed * Time.deltaTime);

        if (transform.position.y < -7.0f)
        { 
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7.0f, 0);
            _enemyOneSpeed = Random.Range(2.5f, 4.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
