using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireBossTurretLaser : MonoBehaviour
{
    private GameManager _gameManager;
    public GameObject ExplosionEffect;
    public GameObject _turretLaserPrefab;
    [SerializeField] private bool _stopUpdating = false;
    [SerializeField] private bool _bossStartShooting = false;

    private float _enemyRateOfFire = 3.0f;
    private float _enemyCanFire = -1.0f;

    void Start()
    {

        StartCoroutine(BossStartShooting());

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null.");
        }

    }

    private void Update()
    {
        if (Time.time > _enemyCanFire && _stopUpdating == false && _bossStartShooting == true)
        {
            //_enemyRateOfFire = _gameManager.currentEnemyRateOfFire;
            //_enemyRateOfFire = 1.5f;
            _enemyRateOfFire = Random.Range(0.5f, 1.5f);

            _enemyCanFire = Time.time + _enemyRateOfFire;

            TurretLaserShot();

            Laser[] lasers = _turretLaserPrefab.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            //PlayClip(_enemyLaserShotAudioClip);
        }
    }

    IEnumerator BossStartShooting()
    {
        yield return new WaitForSeconds(6.0f);
        _bossStartShooting = true;
    }

    public void TurretLaserShot()
    {
        Instantiate(_turretLaserPrefab, transform.position, transform.rotation);
    }
}


