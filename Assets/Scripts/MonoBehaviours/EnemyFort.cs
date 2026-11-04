using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class EnemyFort : Enemy
{
    Coroutine fireCoroutine; // CHECK
    EnemyCannons cannons; // CHECK
    //Rigidbody2D rb2d;
    Transform targetTransform = null;

    CircleCollider2D circleCollider; // Gizmo


    void Start()
    {
        cannons = GetComponent<EnemyCannons>();

        circleCollider = GetComponent<CircleCollider2D>(); // Gizmo
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            targetTransform = collision.gameObject.transform;

            cannons.target = targetTransform;
            fireCoroutine = StartCoroutine(cannons.FireAmmo());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            if(fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }

            targetTransform = null;
        }
    }

    void OnDrawGizmos() // Gizmo
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}