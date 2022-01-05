using System.Collections;
using UnityEngine;

public class FireBossTurretLaser : MonoBehaviour
{
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private EnemyBoss _enemyBoss;
    public GameObject explosionEffect;
    public GameObject turretLaserPrefab;
    public AudioSource audioSource;
    public AudioClip _enemyLaserShotAudioClip;
    public  bool bossStartShooting = false;

    private float _enemyRateOfFire;
    private float _enemyCanFire;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _enemyBoss = GameObject.Find("EnemyBoss(Clone)").GetComponent<EnemyBoss>();
        audioSource = GetComponent<AudioSource>();


        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null.");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
        }

        if (_enemyBoss == null)
        {
            Debug.LogError("The Enemy Boss script is null.");
        }

        if (audioSource == null)
        {
            Debug.LogError("The audio source is null.");
        }

        if (_enemyBoss.isEnemyBossActive == true)
        {
            StartCoroutine(BossStartShooting());
        }
    }

    private void Update()
    {
        if (_spawnManager.isPlayerDestroyed == false)
        {
            if (Time.time > _enemyCanFire && bossStartShooting == true && _enemyBoss.isEnemyBossActive == true)

            {
                _enemyRateOfFire = Random.Range(1.5f, 3.5f);
                _enemyCanFire = Time.time + _enemyRateOfFire;

                if (_gameManager.currentEnemyRateOfFire != 0)
                {
                    if (_spawnManager.isPlayerDestroyed == false)
                    {
                        GameObject bossTurretLaser = Instantiate(turretLaserPrefab, transform.position, transform.rotation);
                        bossTurretLaser.transform.parent = _spawnManager._enemyContainer.transform;
                    }
                }

                PlayClip(_enemyLaserShotAudioClip);
            }
        }
    }

    public void PlayClip(AudioClip soundEffectClip)
    {
        if (soundEffectClip != null)
        {
            audioSource.PlayOneShot(soundEffectClip);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }

    public void CountdownActiveTurrets()
    {
        StartCoroutine(BossStartShooting());
    }

    IEnumerator BossStartShooting()
    {
        yield return new WaitForSeconds(6.0f); // sets the delay before the Boss starts firing from his main turrets
        bossStartShooting = true;
    }
}


