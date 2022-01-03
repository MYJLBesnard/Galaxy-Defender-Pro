using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameAsteroid : MonoBehaviour
{
    [SerializeField] private float _asteroidSpeed;
    [SerializeField] private bool _startOfGame = true;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionSoundEffect;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private PlayerScript _playerScript;
    //private EndOfLevelDialogue _endOfLevelDialogue;


    private void Start()
    {
        _asteroidSpeed = 2.0f;

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        //_endOfLevelDialogue = GameObject.Find("DialoguePlayer").GetComponent<EndOfLevelDialogue>();  //************************************

        if (_gameManager == null)
        {
            Debug.Log("The GameManager is null.");
        }

        if (_spawnManager == null)
        {
            Debug.Log("The SpawnManager is null.");
        }

        if (_playerScript == null)
        {
            Debug.Log("The PlayerScript is null.");
        }

        /*
        if (_endOfLevelDialogue == null)
        {
            Debug.Log("Dialogue Player is NULL.");
        }
        */

        _gameManager.CachePlayerScript();
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
        }
    }
}
