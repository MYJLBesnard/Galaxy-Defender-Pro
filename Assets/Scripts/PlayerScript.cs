using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _playerLaserPrefab;

    [SerializeField]
    private bool _hasPlayerLaserCooledDown = true;

    [SerializeField]
    private float _playerRateOfFire = 0.5f;


    void Start()
    {
        transform.position = new Vector3(-5, 0, 0);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _hasPlayerLaserCooledDown)
        {
            PlayerFireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void PlayerFireLaser()
    {
        Instantiate(_playerLaserPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
        _hasPlayerLaserCooledDown = false;
        StartCoroutine(PlayerLaserCoolDownTimer());
    }

    IEnumerator PlayerLaserCoolDownTimer()
    {
        yield return new WaitForSeconds(_playerRateOfFire);
        _hasPlayerLaserCooledDown = true;
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
