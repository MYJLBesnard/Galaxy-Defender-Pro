using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _laserSpeed = 8.0f;
    [SerializeField] private bool _isPlayerLaser = false, _isEnemyLaser = false, _isPlayerLateralLaser = false, _isEnemyRearShootingLaser = false, _isEnemyArcLaser = false;


    void Update()
    {
        if (_isPlayerLaser == true)
        {
            LaserMoveUp();
        }

        if (_isEnemyLaser == true)
        {
            LaserMoveDown();
        }

        if (_isPlayerLateralLaser == true)
        {
            LaserMoveLateral();
        }

        if (_isEnemyRearShootingLaser == true)
        {
            LaserMoveUp();
        }
    }

    void LaserMoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);


        if (transform.position.y > 6.00f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void LaserShootRear()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);


        if (transform.position.y > 6.00f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void LaserMoveDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -6.00f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void LaserMoveLateral()
    {
        transform.Translate(Vector3.left * _laserSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * _laserSpeed * Time.deltaTime);
        if (transform.position.x < -12.0f || transform.position.x > 12.0f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }


    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;

        if (_isEnemyRearShootingLaser == true)
        {
            _isEnemyLaser = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true || other.tag == "Player" && _isEnemyRearShootingLaser == true)
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }

        if (other.tag == "Player" && _isEnemyArcLaser == true)
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
