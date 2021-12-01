using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrackPlayer : MonoBehaviour
{
    private Rigidbody2D _rb;
  //  public GameObject homingMissileExplosionEffect;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private float _missileSpeed = 12.0f;
    [SerializeField] private float _rotateSpeed = 350f;
   // [SerializeField] private GameObject _selfdestructExplosion;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
            MoveTowardsEnemy();
    }

    private void MoveTowardsEnemy()
    {
        Vector2 direction = (Vector2)_playerGameObject.transform.position - _rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -rotateAmount * _rotateSpeed;
        //_rb.velocity = transform.up * _missileSpeed;
    }
}


