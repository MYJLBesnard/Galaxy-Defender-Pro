using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEnemy4 : MonoBehaviour // Laser Burst (targets Player and PowerUps) & Dodges Player Laser
{
    private GameManager _gameManager;
    public Enemy4 _enemy4Script;
    public GameObject Enemy4;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemy4Script = Enemy4.GetComponent<Enemy4>();
    }

    void FixedUpdate()
    {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _gameManager.currentEnemySensorRange);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    RunLaserBurst();
                }

                if (hit.collider.tag == "PlayerPowerUps")
                {
                    RunLaserBurst();
                }

                if (hit.collider.tag == "LaserPlayer")
                {
                    Debug.Log("Incoming Player Laser! - RayCast 4");
                    RunEnemy4Dodge();
                }
            }

            Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);
    }

    public void RunLaserBurst()
    {
        _enemy4Script.StartCoroutine(_enemy4Script.LaserBurst());
    }

    public void RunEnemy4Dodge()
    {
        _enemy4Script._randomNumber = Random.Range(-10, 10);
        _enemy4Script.StartCoroutine(_enemy4Script.DodgePlayerLaser());
    }
}
