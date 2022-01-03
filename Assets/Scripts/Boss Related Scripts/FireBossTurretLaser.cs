using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBossTurretLaser : MonoBehaviour
{
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    [SerializeField] private EnemyBoss _enemyBoss;
    public GameObject explosionEffect;
    public GameObject turretLaserPrefab;
    //public AudioClip _enemyLaserShotAudioClip;
    //[SerializeField] private bool _stopUpdating = false;
    public  bool bossStartShooting = false;

    private float _enemyRateOfFire;
    private float _enemyCanFire;

    void Start()
    {
        if (_enemyBoss.isEnemyBossActive == true)
        {
            StartCoroutine(BossStartShooting());
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _enemyBoss = GameObject.Find("EnemyBoss(Clone)").GetComponent<EnemyBoss>();

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

                //PlayClip(_enemyLaserShotAudioClip);
            }
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

            //_audioSource.Play();
    }

    public void CountdownActiveTurrets()
    {
        StartCoroutine(BossStartShooting());
    }

    IEnumerator BossStartShooting()
    {
        yield return new WaitForSeconds(600.0f); // sets the delay before the Boss starts firing from his main turrets
        bossStartShooting = true;
    }
}


