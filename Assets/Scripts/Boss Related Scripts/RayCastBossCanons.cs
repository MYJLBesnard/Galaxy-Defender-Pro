using System.Collections;
using UnityEngine;

public class RayCastBossCanons : MonoBehaviour // Laser Burst (targets Player and PowerUps)
{
    private GameManager _gameManager;
    public EnemyBoss _enemyBossScript;
    public GameObject EnemyBoss;
    public AudioSource audioSource;
    public AudioClip _enemyLaserCanonAudioClip;


    [SerializeField] private GameObject _enemyBossFixedWpnLaser;

    public float posAdjust = 2.0f;

    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemyBossScript = EnemyBoss.GetComponent<EnemyBoss>();
        audioSource = GetComponent<AudioSource>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null.");
        }

        if (_enemyBossScript == null)
        {
            Debug.LogError("The EnemyBoss Script is null.");
        }

        if (audioSource == null)
        {
            Debug.LogError("The audio source is null.");
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _gameManager.currentBossSensorRange);

        if (hit.collider != null)
            {
                if (hit.collider.tag == "Player" && _enemyBossScript.isEnemyBossActive == true) // stops miniguns from firing on Player when he is respawning and powerless
                {
                    RunLaserBurst();
                }

            if (hit.collider.tag == "PlayerPowerUps")
                {
                    RunLaserBurst();
                }
        }

        Debug.DrawRay(transform.position, Vector2.down * _gameManager.currentBossSensorRange, Color.red);
    }

    public void RunLaserBurst()
    {
        PlayClip(_enemyLaserCanonAudioClip);
        StartCoroutine(LaserBurst());
    }

    public IEnumerator LaserBurst()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + posAdjust, transform.position.z);
        GameObject enemyLaser = Instantiate(_enemyBossFixedWpnLaser, position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }

        yield return new WaitForSeconds(0.1f);
    }

    public void PlayClip(AudioClip soundEffectClip)
    {
        if (soundEffectClip != null)
        {
            audioSource.PlayOneShot(soundEffectClip);
        }
    }
}
