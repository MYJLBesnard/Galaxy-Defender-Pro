using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEnemy5 : MonoBehaviour // Fires Rear Burst
{
    private GameManager _gameManager;
    public Enemy5 _enemy5Script;
    public GameObject Enemy5;
    [SerializeField] private GameObject _enemyRearShotLaserPrefab;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemy5Script = Enemy5.GetComponent<Enemy5>();
    }

    void FixedUpdate()
    {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * _gameManager.currentEnemySensorRange);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player" && hit.distance <= _gameManager.currentEnemySensorRange)
            {
                    RunRearLaserBurst();
                }
            }

            Debug.DrawRay(transform.position, Vector2.up * _gameManager.currentEnemySensorRange, Color.red);
    }

    public void RunRearLaserBurst()
    {
        _enemy5Script.StartCoroutine(_enemy5Script.RearLaserBurst());
    }
}
