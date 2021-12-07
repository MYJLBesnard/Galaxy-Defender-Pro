using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
public class FireBossTurretLaser : MonoBehaviour
{
    private GameManager _gameManager;
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
            Debug.Log("going to coroutine now...");
            StartCoroutine(BossStartShooting());
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemyBoss = GameObject.Find("EnemyBoss").GetComponent<EnemyBoss>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null.");
        }

        if (_enemyBoss == null)
        {
            Debug.LogError("The Enemy Boss script is null.");
        }
    }

    private void Update()
    {
        if (Time.time > _enemyCanFire && bossStartShooting == true) /*_stopUpdating == false && */ 
        {

            _enemyRateOfFire = Random.Range(0.5f, 1.5f);
            _enemyCanFire = Time.time + _enemyRateOfFire;

            TurretLaserShot();

            Laser[] lasers = turretLaserPrefab.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            //PlayClip(_enemyLaserShotAudioClip);
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
        Debug.Log("counting 6 seconds");
        yield return new WaitForSeconds(6.0f);
        bossStartShooting = true;
        Debug.Log("finished counting.  bool now: " + bossStartShooting);
    }

    public void TurretLaserShot()
    {
        Instantiate(turretLaserPrefab, transform.position, transform.rotation);
    }
}


