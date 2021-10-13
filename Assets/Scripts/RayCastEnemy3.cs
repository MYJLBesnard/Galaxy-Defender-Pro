using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEnemy3 : MonoBehaviour
{
    //private PlayerScript _player;
    private GameManager _gameManager;
    //private SpawnManager _spawnManager;
    public Enemy3 _enemy3Script;
    public GameObject Enemy3;
    //private float _enemySpeed;


    //[SerializeField] bool _isPlayer, _isEnemy, _isEnemy3;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;

    private void Start()
    {
        //_player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        //_spawnManager = GameObject.Find("Spawn Manager").GetComponentInChildren<SpawnManager>();
        //_enemy3Script = GameObject.Find("Enemy3(Clone)").GetComponent<Enemy3>();
        _enemy3Script = Enemy3.GetComponent<Enemy3>();
    }

    void FixedUpdate()
    {
        //if (_isEnemy3 == true)
        //{
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _gameManager.currentEnemySensorRange);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player")
                {
                    /*
                    Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                    GameObject enemyLaser = Instantiate(_enemyDoubleShotLaserPrefab, position, Quaternion.identity);
                    Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                    for (int i = 0; i < lasers.Length; i++)
                    {
                        lasers[i].AssignEnemyLaser();
                    }
                    */

                    RunSpeedBurst();

                }
            }

            Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);
        //}
    }

    public void RunSpeedBurst()
    {
        _enemy3Script.StartCoroutine(_enemy3Script.SpeedBurst());
    }
}
