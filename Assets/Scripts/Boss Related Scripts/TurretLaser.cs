using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : MonoBehaviour    // moves the Boss turret laser shots towards the position of Player at that moment in time
                                            // Shot fired doesn't track player.
{
    private Vector3 normalizeDirection;
    public Transform _playerTransform;
    public float speed = 12f;

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