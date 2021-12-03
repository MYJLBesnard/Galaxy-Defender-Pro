using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : MonoBehaviour
{
    
    private Vector3 normalizeDirection;
    public Transform _playerTransform;
    public float speed = 5f;

    void Start()
    {
        if (!_playerTransform) _playerTransform = GameObject.Find("Player").transform;

        normalizeDirection = (_playerTransform.position - transform.position).normalized;
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * normalizeDirection;
    }
}