using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBoss : MonoBehaviour // Laser Burst (targets Player and PowerUps) & Dodges Player Laser
{
    private GameManager _gameManager;
    public EnemyBoss _enemyBossScript;
    public GameObject EnemyBoss;
    [SerializeField] private GameObject _enemyBossTurretLaser;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemyBossScript = EnemyBoss.GetComponent<EnemyBoss>();
    }

    void FixedUpdate()
    {
           // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _gameManager.currentEnemySensorRange);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5);


        if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
 //                   RunLaserBurst();
                }

                if (hit.collider.tag == "PlayerPowerUps")
                {
 //                   RunLaserBurst();
                }
            }

           // Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 5, Color.red);

    }

    public void RunLaserBurst()
    {
 //       _enemyBossScript.StartCoroutine(_enemyBossScript.LaserBurst());
    }
}
