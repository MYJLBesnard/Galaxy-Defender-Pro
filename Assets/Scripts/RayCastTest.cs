using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{

    void FixedUpdate()
    {
        float distanceToTarget = 2.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, distanceToTarget);


        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
        }

        Debug.DrawRay(transform.position, Vector2.up * distanceToTarget, Color.red);

    }


}
