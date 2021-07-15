using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemyOneSpeed;

    private PlayerScript _player;
    private Animator _animEnemyDestroyed;

    [SerializeField]
    private GameObject _enemyOnePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _enemyOneSpeed = Random.Range(2.5f, 4.5f);
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _animEnemyDestroyed = GetComponent<Animator>();

        if (_player == null)
        {
            Debug.Log("The PlayerScript is null.");
        }

        if (_animEnemyDestroyed == null)
        {
            Debug.Log("The Enemy Dstroyed anim is null.");
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerScript player = other.transform.GetComponent<PlayerScript>();

            if (player != null)
            {
                player.Damage();
            }

            DestroyEnemyShip();
        }

        if (other.tag == "LaserPlayer")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10); // calls the AddScore() method in the PlayerScript to add 10 points to the score
                                      // the value of 10 is set to this type of enemy, but we could expand later with a
                                      // Switch statement to attribute different values to "points"
            }

            DestroyEnemyShip();
        }
    }

    private void DestroyEnemyShip()
    {
        _animEnemyDestroyed.SetTrigger("OnEnemyDeath");
        _enemyOneSpeed = 0;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this.gameObject, 2.8f);
    }
}




