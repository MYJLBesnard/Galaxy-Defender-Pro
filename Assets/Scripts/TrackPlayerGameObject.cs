using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrackPlayerGameObject : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private float _rotateSpeed = 350f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        TrackPlayerShip();
    }

    private void TrackPlayerShip()
    {
        Vector2 direction = (Vector2)_playerGameObject.transform.position - _rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -rotateAmount * _rotateSpeed;
    }
}
