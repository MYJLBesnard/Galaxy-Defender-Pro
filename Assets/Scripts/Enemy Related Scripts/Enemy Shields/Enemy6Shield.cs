using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6Shield : MonoBehaviour
{
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private bool _isEnemyShieldActive = true;
    [SerializeField] public int _shield6Hits = 0;
    [SerializeField] private float _enemyShieldAlpha = 1.0f;

    private Enemy6 _enemy6;

    void Start()
    {
        _enemy6 = GameObject.Find("Enemy6").GetComponent<Enemy6>();

        if (_enemy6 == null)
        {
            Debug.LogError("The Enemy6 script is null.");
        }
    }

    public void Enemy6Damage()
    {
        if (_isEnemyShieldActive == true)
        {
            _shield6Hits++;

            switch (_shield6Hits)
            {
                case 1:
                    _enemyShieldAlpha = 0.75f;
                    _enemyShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _enemyShieldAlpha);
                    break;
                case 2:
                    _enemyShieldAlpha = 0.40f;
                    _enemyShield.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _enemyShieldAlpha);
                    break;
                case 3:
                    _isEnemyShieldActive = false;
                    _enemyShield.SetActive(false);
                    break;
            }
            return;
        }

        _enemy6.DestroyEnemyShip();
    }
}