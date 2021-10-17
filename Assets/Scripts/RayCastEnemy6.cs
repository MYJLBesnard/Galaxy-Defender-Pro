﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEnemy6 : MonoBehaviour
{
    private GameManager _gameManager;
    public Enemy6 _enemy6Script;
    public GameObject Enemy6;
  //  [SerializeField] private GameObject _enemyArcPrefab;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _enemy6Script = Enemy6.GetComponent<Enemy6>();
    }

    void FixedUpdate()
    {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * _gameManager.currentEnemySensorRange);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player" && hit.distance <= _gameManager.currentEnemySensorRange)
                {
                    RunArcLaserBurst();
                }
            }

            Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentEnemySensorRange, Color.red);
    }

    public void RunArcLaserBurst()
    {
        _enemy6Script.StartCoroutine(_enemy6Script.ArcBurst());
    }
}
