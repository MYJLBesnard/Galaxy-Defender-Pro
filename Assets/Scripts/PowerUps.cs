using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]
    private float _speedOfPowerUps = 3.0f;

    [SerializeField]
    private int _powerUpID;  // ID for PwrUp: 0 = Triple Shot, 1 = Speed Boost, 2 = Shields.

    void Update()
    {
        transform.Translate(Vector3.down * _speedOfPowerUps * Time.deltaTime);

        if (transform.position.y < -7.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerScript player = other.transform.GetComponent<PlayerScript>();

            if (player != null)
            {
                if (_powerUpID == 0)
                {
                    player.TripleShotActivate();
                }
                else if (_powerUpID == 1)
                {
                    player.SpeedBoostActivate();
                }
                else if (_powerUpID == 2)
                {
                    player.ShieldActivate();
                }
            }

            Destroy(this.gameObject);
        }
    }
}
