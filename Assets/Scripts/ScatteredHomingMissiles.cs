using UnityEngine;

public class ScatteredHomingMissiles : MonoBehaviour
{
    [SerializeField] private EndOfLevelDialogue _endOfLevelDialogue; 
    [SerializeField] private AudioClip _powerUpAudioClip = null;
    [SerializeField] private GameObject _explosionPrefab;

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
        if (other.tag == "EnemyLaser")
        {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
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

                player.PlayerHomingMissiles();
                Destroy(this.gameObject);
            }
        }
    }
}