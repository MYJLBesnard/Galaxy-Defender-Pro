using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEnemy3 : MonoBehaviour // Speed Burst
{
    private GameManager _gameManager;
    public Enemy3 _enemy3Script;
    public GameObject Enemy3;

    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemy3Script = Enemy3.GetComponent<Enemy3>();
    }

    void FixedUpdate()
    {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _gameManager.currentEnemySensorRange);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    RunSpeedBurst();
                }
            }

            Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);
    }

    public void RunSpeedBurst()
    {
        _enemy3Script.StartCoroutine(_enemy3Script.SpeedBurst());
    }
}
