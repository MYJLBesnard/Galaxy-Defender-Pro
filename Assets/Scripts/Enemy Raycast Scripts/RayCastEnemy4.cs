using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEnemy4 : MonoBehaviour
{
    private GameManager _gameManager;
    public Enemy4 _enemy4Script;
    public GameObject Enemy4;
    [SerializeField] private GameObject _enemyDoubleShotLaserPrefab;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _enemy4Script = Enemy4.GetComponent<Enemy4>();
    }

    void FixedUpdate()
    {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _gameManager.currentEnemySensorRange);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player")
                {
                    RunLaserBurst();
                }
            }

            Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);
    }

    public void RunLaserBurst()
    {
        _enemy4Script.StartCoroutine(_enemy4Script.LaserBurst());
    }
}
