using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrackPlayerGameObject : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private EnemyBoss _enemyBoss;
    private Rigidbody2D _rb;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private GameObject _enemyBossDefaultAimPoint;
    [SerializeField] private float _rotateSpeed = 500f;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
        }

        _enemyBoss = GameObject.Find("EnemyBoss(Clone)").GetComponent<EnemyBoss>();

        if (_enemyBoss == null)
        {
            Debug.Log("The FireBossTurretLaser is null.");
        }

        _rb = GetComponent<Rigidbody2D>();

        _playerGameObject = GameObject.Find("Player");

        _enemyBossDefaultAimPoint = GameObject.Find("EnemyBossDefaultAimPoint");
    }

    private void Update()
    {
        if (_playerGameObject != null)  // if Player still alive, start tracking

        {
            TrackPlayerShip();
        }

        if (_spawnManager.isPlayerDestroyed == true) // heeps turrets aiming to the bottom of the screen
                                                     // if Player is destroyed.

        {
            TrackDefaultAimPoint();
        }
    }

    private void TrackPlayerShip()
    {
        Vector2 direction = (Vector2)_playerGameObject.transform.position - _rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        _rb.angularVelocity = -rotateAmount * _rotateSpeed;

        // The following code checks to see if there is a large angular difference between the Player location and the axis of the Turrets.
        // If the angle exceed +/- 100, then the EnemyBoss is deactivated until the Turrets are pointing closer to the Player's position.
        // This ensure that the laser shots from the Turrets are aligned properly and not moving through space sideways.
        if (_rb.angularVelocity > 100f || _rb.angularVelocity < -100f)
        {
            _enemyBoss.isEnemyBossActive = false;
        }
        else
        {
            _enemyBoss.isEnemyBossActive = true;
        }
    }

    private void TrackDefaultAimPoint()
    {
        Vector2 direction = (Vector2)_enemyBossDefaultAimPoint.transform.position - _rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -rotateAmount * _rotateSpeed;
    }
}
