
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShootTest : MonoBehaviour
{
    public GameObject bulletPrefab;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) == true)
            transform.rotation *= Quaternion.Euler(0f, 0f, 360.0f * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow) == true)
            transform.rotation *= Quaternion.Euler(0f, 0f, -360.0f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            //CreateBullet(-30f);
            CreateBullet(-15f);
            CreateBullet(0f);
            CreateBullet(15f);
            //CreateBullet(30f);
        }
    }

    private void CreateBullet(float angleOffset = 0f)
    {
        GameObject bullet = Instantiate<GameObject>(bulletPrefab);
        bullet.transform.position = transform.position;

        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * transform.right * 80f);
    }
}
