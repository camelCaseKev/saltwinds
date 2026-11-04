using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class CargoRun : MonoBehaviour
{
    public float fleeSpeed;
    public float baseSpeed;
    float currentSpeed;
    public GameObject waypoint1; // CHECK
    public GameObject waypoint2; // CHECK
    public GameObject waypoint3; // CHECK
    //public float directionChangeInterval;
    //public bool followPlayer;
    //public float stoppingDistance = 3.0f;

    Coroutine moveCoroutine;
    Coroutine fireCoroutine;
    EnemyCannons cannons; 
    Rigidbody2D rb2d;
    Animator animator;
    Transform targetTransform = null;
    Transform playerTransform;
    Vector3 endPosition;
    //float currentAngle = 0;

    CircleCollider2D circleCollider; // Gizmo


    void Start()
    {
        animator = GetComponent<Animator>();
        cannons = GetComponent<EnemyCannons>();
        currentSpeed = baseSpeed;
        rb2d = GetComponent<Rigidbody2D>();

        /*if (transform.position.x < 0f)
        {
            waypoint1 = GameObject.Find("MerchantWaypointSouth1");
            waypoint2 = GameObject.Find("MerchantWaypointSouth2");
        }
        else
        {
            waypoint1 = GameObject.Find("MerchantWaypointSouth1");
            waypoint2 = GameObject.Find("MerchantWaypointSouth2");
        }*/

        targetTransform = waypoint1.transform; // CHECK
        moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

        circleCollider = GetComponent<CircleCollider2D>(); // Gizmo
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        while(true)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
            }

            Vector3 direction = endPosition - transform.position;
            float distanceToTarget = direction.magnitude;

            if(rigidBodyToMove != null)
            {
                animator.SetBool("isSailing", true);
                SetAnimationDir(direction.normalized);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                rb2d.MovePosition(newPosition);
            }

            yield return new WaitForFixedUpdate();
        }

        //animator.SetBool("isSailing", false); --> Currently unreachable
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            currentSpeed = fleeSpeed;
            playerTransform = collision.gameObject.transform;

            if(cannons != null)
            {
                cannons.target = playerTransform;
                fireCoroutine = StartCoroutine(cannons.FireAmmo());
            }

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
        }

        if(collision.gameObject == waypoint1) // CHECK
        {
            if(waypoint2 != null)
            {
                targetTransform = waypoint2.transform;
            }
            else
            {
                if(moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
            }
        }

        if(collision.gameObject == waypoint2) // CHECK
        {
            if(waypoint3 != null)
            {
                targetTransform = waypoint3.transform;
            }
            else
            {
                if(moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
            }
        }

        if(collision.gameObject == waypoint3) // CHECK
        {
            if(moveCoroutine != null) // Stop the ship, it has finished its journey
            {
                StopCoroutine(moveCoroutine);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isSailing", false);
            currentSpeed = baseSpeed;

            if(fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }
    }

    void OnDrawGizmos() // Gizmo
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    void SetAnimationDir(Vector3 direction)
    {
        if(direction.magnitude < 0.01f) // skip if barely moving
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Repeat(angle, 360f);

        if(angle > 337.5f || angle <= 22.5f) // EAST
        {
            animator.SetFloat("xDir", 1f);
            animator.SetFloat("yDir", 0f);
            animator.SetFloat("lastXdir", 1f);
            animator.SetFloat("lastYdir", 0f);
        }
        else if(angle > 22.5f && angle <= 67.5f) // northeast
        {
            animator.SetFloat("xDir", 1f);
            animator.SetFloat("yDir", 1f);
            animator.SetFloat("lastXdir", 1f);
            animator.SetFloat("lastYdir", 1f);
        }
        else if(angle > 67.5f && angle <= 112.5f) // NORTH
        {
            animator.SetFloat("xDir", 0f);
            animator.SetFloat("yDir", 1f);

            animator.SetFloat("lastXdir", 0f);
            animator.SetFloat("lastYdir", 1f);
        }
        else if(angle > 112.5f && angle <= 157.5f) // northwest
        {
            animator.SetFloat("xDir", -1f);
            animator.SetFloat("yDir", 1f);

            animator.SetFloat("lastXdir", -1f);
            animator.SetFloat("lastYdir", 1f);
        }
        else if(angle > 157.5f && angle <= 202.5f) // WEST
        {
            animator.SetFloat("xDir", -1f);
            animator.SetFloat("yDir", 0f);

            animator.SetFloat("lastXdir", -1f);
            animator.SetFloat("lastYdir", 0f);
        }
        else if(angle > 202.5f && angle <= 247.5f) // southwest
        {
            animator.SetFloat("xDir", -1f);
            animator.SetFloat("yDir", -1f);

            animator.SetFloat("lastXdir", -1f);
            animator.SetFloat("lastYdir", -1f);
        }
        else if(angle > 247.5f && angle <= 292.5f) // SOUTH
        {
            animator.SetFloat("xDir", 0f);
            animator.SetFloat("yDir", -1f);

            animator.SetFloat("lastXdir", 0f);
            animator.SetFloat("lastYdir", -1f);
        }
        else if(angle > 292.5f && angle <= 337.5f) // southeast
        {
            animator.SetFloat("xDir", 1f);
            animator.SetFloat("yDir", -1f);

            animator.SetFloat("lastXdir", 1f);
            animator.SetFloat("lastYdir", -1f);
        }
    }
}