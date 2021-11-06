using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingPlayerLaserDetection : MonoBehaviour // Laser Burst (targets Player and PowerUps) & Dodges Player Laser
{
    [SerializeField] public int randomNumber;


    //private GameManager _gameManager;
    public Enemy4 _enemy4Script;
    public GameObject Enemy4;
    //public GameObject PlayerLaserDetector;
    //[SerializeField] private GameObject _enemyDoubleShotLaserPrefab;

    private void Start()
    {
        //_gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _enemy4Script = Enemy4.GetComponent<Enemy4>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerLaserDetectable")
        {
                Debug.Log("Incoming Player Laser!");

            RunEnemy4Dodge();
        }
    }

    public void RunEnemy4Dodge()
    {
        randomNumber = Random.Range(-10, 10); // used to randomly pick left or right dodge
        _enemy4Script._randomNumber = randomNumber;
        _enemy4Script.StartCoroutine(_enemy4Script.DodgePlayerLaser());

    }
}
