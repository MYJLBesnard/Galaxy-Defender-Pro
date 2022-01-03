using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : MonoBehaviour    // moves the Boss turret laser shots towards the position of Player at that moment in time
                                            // Shot fired doesn't track player, they are only aligned to the line of sight between turret and Player
                                            // posiiton at time shot fired.
{
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private Vector3 normalizeDirection;
    public Transform _playerTransform;
    public float speed = 12f;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null.");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
        }

        if (!_playerTransform && _spawnManager.isPlayerDestroyed == false) _playerTransform = GameObject.Find("Player").transform; // turrets track Player transform

        normalizeDirection = (_playerTransform.position - transform.position).normalized;
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * normalizeDirection;

        if (transform.position.y < -10.00f || transform.position.y > 10.00f || transform.position.x < -15.00f || transform.position.x > 15.00f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);

                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        //_audioSource.Play();
    }
}