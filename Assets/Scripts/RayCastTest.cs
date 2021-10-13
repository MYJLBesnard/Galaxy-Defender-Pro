using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    private PlayerScript player;
    private GameManager gameManager;
    public Enemy3 enemy;
    private float _enemySpeed;


    [SerializeField] bool _isPlayer, _isEnemy, _isEnemy3;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        //enemy = GameObject.Find("Enemy3(Clone)").GetComponent<Enemy3>();
        enemy = GameObject.Find("Spawn Manager").GetComponentInChildren<Enemy3>();
        //enemy = GameObject.Find("Enemy3(Clone)").GetComponentInParent<EnemyTest>();

        if (enemy == null)
        {
            Debug.LogError("The enemy is null.");
        }
    }

    void FixedUpdate()
    {
        if (_isPlayer == true)
        {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, gameManager.currentEnemySensorRange);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Enemy")
            {
                player.PlayerFireLaser();
            }
        }

        Debug.DrawRay(transform.position, Vector2.up * gameManager.currentEnemySensorRange, Color.red);
        }

        if (_isEnemy == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, gameManager.currentEnemySensorRange);

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

                    //RunSpeedBurst();

                }
            }
        }

        if (_isEnemy3 == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, gameManager.currentEnemySensorRange);

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

            Debug.DrawRay(transform.position, Vector2.down * gameManager.currentEnemySensorRange, Color.red);
        }
      

    }

    public void RunSpeedBurst()
    {
        enemy.StartCoroutine(enemy.SpeedBurst());

    }



}
