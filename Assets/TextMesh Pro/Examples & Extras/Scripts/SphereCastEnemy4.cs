using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastEnemy4 : MonoBehaviour // Detects and Dodges Player Laser
{
    private GameManager _gameManager;
    public Enemy4 _enemy4Script;
    //public GameObject Enemy4;
   // [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;


    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _enemy4Script = GameObject.Find("Enemy4").GetComponent<Enemy4>();
    }

    void Update()
    {
        RaycastHit hit;
    
        float thickness = 1f; //<-- Desired thickness here.

        Vector3 origin = transform.position + new Vector3(0, 0.6f, -1.6f);
        Vector3 direction = transform.TransformDirection(Vector3.down);

        if (Physics.SphereCast(origin, thickness, direction, out hit))
        {
            //distanceToObstacle = hit.distance;
            if (hit.collider != null)
            {
                if (hit.collider.tag == "LaserPlayer")
                {
                    Debug.Log("Incoming Player Laser!");
                    RunEnemy4Dodge();
                }
            }
        }

        Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);

    }

    public void RunEnemy4Dodge()
    {
        _enemy4Script.randomNumber = Random.Range(-10, 10);
        _enemy4Script.StartCoroutine(_enemy4Script.DodgePlayerLaser());
    }
}

