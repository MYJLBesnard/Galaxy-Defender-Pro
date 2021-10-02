using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float _speedOfPowerUps = .5f;
    [SerializeField] private int _powerUpID;  // ID for PwrUp:
                                              // 0 = Triple Shot,
                                              // 1 = Speed Boost,
                                              // 2 = Shields,
                                              // 3 = Health,
                                              // 4 = Ammo,
                                              // 5 = Homing Missiles,
                                              // 6 = Lateral Laser Canon.
    [SerializeField] private AudioClip _powerUpAudioClip = null;

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
                player.PlayClip(_powerUpAudioClip);
                switch (_powerUpID)
                {
                    case 0:
                        player.TripleShotActivate();
                        break;
                    case 1:
                        player.SpeedBoostActivate();
                        break;
                    case 2:
                        player.ShieldActivate();
                        break;
                    case 3:
                        player.HealthBoostActivate();
                        break;
                    case 4:
                        player.PlayerRegularAmmo();
                        break;
                    case 5:
                        player.PlayerHomingMissiles();
                        break;
                    case 6:
                        player.LateralLaserShotActive();
                        break;
                }
            }

            AudioSource.PlayClipAtPoint(_powerUpAudioClip, transform.position);
            Destroy(this.gameObject);
        }
    }
}
