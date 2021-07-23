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

    [SerializeField]
    private bool _dodgingEnemyMovementOn;

    private float x;
    private float y;
    private float z;

    [SerializeField]
    private float _dodgingAmplitude = 1.0f;
    [SerializeField]
    private float _dodgingFrequency = 0.5f;

    private float _randomXStartPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        _enemyOneSpeed = Random.Range(2.5f, 4.5f);
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _animEnemyDestroyed = GetComponent<Animator>();

        _dodgingEnemyMovementOn = true;
        _dodgingAmplitude = Random.Range(1f, 5f);
        _randomXStartPos = Random.Range(-8.0f, 8.0f);


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
        if (_dodgingEnemyMovementOn == true)
        {
            {
                x = Mathf.Cos(_enemyOneSpeed * Time.time * _dodgingFrequency) * _dodgingAmplitude;
                y = transform.position.y;
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);
            }

            transform.Translate(Vector3.down * _enemyOneSpeed * Time.deltaTime);

        }
        else
        {
            transform.Translate(Vector3.down * _enemyOneSpeed * Time.deltaTime);

        }

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




