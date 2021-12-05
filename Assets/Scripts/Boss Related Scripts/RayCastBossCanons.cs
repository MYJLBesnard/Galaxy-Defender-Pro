using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBossCanons : MonoBehaviour // Laser Burst (targets Player and PowerUps)
{
    //private GameManager _gameManager;
    public EnemyBoss _enemyBossScript;
    public GameObject EnemyBoss;
    [SerializeField] private GameObject _enemyBossFixedWpnLaser;

    public float posAdjust = 2.0f;

    private void Start()
    {
        //_gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
                Debug.Log("Raycast from Boss hit Player");

                    RunLaserBurst();
                }

                if (hit.collider.tag == "PlayerPowerUps")
                {
                Debug.Log("Raycast from Boss hit PowerUp");

                    RunLaserBurst();
            }
        }

           // Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 5, Color.red);

    }

    public void RunLaserBurst()
    {
        Debug.Log("Firing Boss laser weapons straight down");
        StartCoroutine(LaserBurst());

    }

    public IEnumerator LaserBurst()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + posAdjust, transform.position.z);
        GameObject enemyLaser = Instantiate(_enemyBossFixedWpnLaser, position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }

        yield return new WaitForSeconds(0.1f);
    }
}
