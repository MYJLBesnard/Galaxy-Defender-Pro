using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameAsteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private PlayerScript _playerScript;

    private void Start()
    {
        _rotationSpeed = Random.Range(3.5f, 12f);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (_spawnManager == null)
        {
            Debug.Log("The SpawnManager is null.");
        }

        if (_playerScript == null)
        {
            Debug.Log("The PlayerScript is null.");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LaserPlayer")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);

            _playerScript.AsteroidDestroyed();

            _spawnManager.StartSpawning();
        }
    }
}
