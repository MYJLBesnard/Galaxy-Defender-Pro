using UnityEngine;

public class NewGamePowerUp : MonoBehaviour
{
    [SerializeField] private EndOfLevelDialogue _endOfLevelDialogue; 
    [SerializeField] private AudioClip _powerUpAudioClip = null;
    [SerializeField] private bool _isMissiles = false;
    [SerializeField] private bool _isAmmo = false;

    private void Start()
    {
        _endOfLevelDialogue = GameObject.Find("DialoguePlayer").GetComponent<EndOfLevelDialogue>();

        if (_endOfLevelDialogue == null)
        {
            Debug.Log("Dialogue Player is NULL.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerScript player = other.transform.GetComponent<PlayerScript>();

            if (player != null)
            {
                if (_endOfLevelDialogue.powerUpAudioIsBossDefeated == false)
                {
                    _endOfLevelDialogue.PlayPowerUpDialogue(_powerUpAudioClip);
                }

                player.newGamePowerUpCollected++;
                if (player.newGamePowerUpCollected == 2)
                {
                    player.LaserIsWpnsFree();
                }

                if (_isMissiles == true)
                {
                    player.PlayerHomingMissiles();
                }

                if (_isAmmo == true)
                {
                    player.PlayerRegularAmmo();
                }

                Destroy(this.gameObject);
            }
        }
    }
}