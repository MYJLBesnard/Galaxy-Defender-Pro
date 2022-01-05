using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private EndOfLevelDialogue _endOfLevelDialogue;
    [SerializeField] private int _powerUpID;  // ID for PwrUp:
                                              // 0 = Triple Shot
                                              // 1 = Speed Boost
                                              // 2 = Shields
                                              // 3 = Ammo
                                              // 4 = Homing Missiles
                                              // 5 = Lateral Laser Canon
                                              // 6 = Health
                                              // 7 = Negative PowerUp
    [SerializeField] private AudioClip _powerUpAudioClip = null;
    [SerializeField] private GameObject _explosionPrefab;

    void Update()
    {
        transform.Translate(Vector3.down * _gameManager.currentPowerUpSpeed * Time.deltaTime);

        if (transform.position.y < -9.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _endOfLevelDialogue = GameObject.Find("DialoguePlayer").GetComponent<EndOfLevelDialogue>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager Player is NULL.");
        }

        if (_endOfLevelDialogue == null)
        {
            Debug.Log("Dialogue Player is NULL.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            if (_powerUpID != 7)
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

        if (other.tag == "Player")
        {
            PlayerScript player = other.transform.GetComponent<PlayerScript>();

            if (player != null)
            {
                if (_endOfLevelDialogue.powerUpAudioIsBossDefeated == false)
                {
                    _endOfLevelDialogue.PlayPowerUpDialogue(_powerUpAudioClip);
                }

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
                        player.PlayerRegularAmmo();
                        break;
                    case 4:
                        player.PlayerHomingMissiles();
                        break;
                    case 5:
                        player.LateralLaserShotActive();
                        break;
                    case 6:
                        player.HealthBoostActivate();
                        break;
                    case 7:
                        player.NegativePowerUpCollision();
                        break;
                }
            }

            AudioSource.PlayClipAtPoint(_powerUpAudioClip, transform.position);

            if (_powerUpID != 7)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
