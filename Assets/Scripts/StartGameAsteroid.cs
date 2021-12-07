using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameAsteroid : MonoBehaviour
{
    [SerializeField] private float _asteroidSpeed;
    [SerializeField] private bool _startOfGame = true;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionSoundEffect;
    private SpawnManager _spawnManager;
    private PlayerScript _playerScript;
    //[SerializeField] private EnemyBoss _enemyBoss;


    private void Start()
    {
        _asteroidSpeed = 2.0f;

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
       // _enemyBoss = GameObject.Find("EnemyBoss").GetComponent<EnemyBoss>();

        if (_spawnManager == null)
        {
            Debug.Log("The SpawnManager is null.");
        }

        if (_playerScript == null)
        {
            Debug.Log("The PlayerScript is null.");
        }

        /*
        if (_enemyBoss == null)
        {
            Debug.LogError("The Enemy Boss script is null.");
        }
        */
    }

    void Update()
    {
        transform.Translate(Vector3.down * _asteroidSpeed * Time.deltaTime);

        if (transform.position.y <= 7.0f)
        {
            transform.position = new Vector3(transform.position.x, 7.0f, 0);
        }

        if (transform.position.y <= 7.0f && _startOfGame == true)
        {
            SpaceCommandDestroyAsteroid();
        }
    }

    public void SpaceCommandDestroyAsteroid()
    {
        _startOfGame = false;
        _playerScript.AsteroidBlockingSensors();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LaserPlayer" || other.tag == "PlayerHomingMissile")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            _playerScript.PlayClip(_explosionSoundEffect);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.5f);
            _playerScript.AsteroidDestroyed();
            //_enemyBoss.isEnemyBossActive = true;
        }
    }
}
