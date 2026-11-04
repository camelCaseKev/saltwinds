using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;
    public float directionChangeInterval;
    public bool followPlayer;
    public float stoppingDistance = 3.0f;

    Coroutine moveCoroutine;
    Coroutine fireCoroutine; // CHECK
    EnemyCannons cannons; // CHECK
    Rigidbody2D rb2d;
    Animator animator;
    Transform targetTransform = null;
    Vector3 endPosition;
    float currentAngle = 0;

    CircleCollider2D circleCollider; // Gizmo


    void Start()
    {
        animator = GetComponent<Animator>();
        cannons = GetComponent<EnemyCannons>(); // CHECK
        currentSpeed = wanderSpeed;
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());

        circleCollider = GetComponent<CircleCollider2D>(); // Gizmo
    }

    void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }

    public IEnumerator WanderRoutine()
    {
        while(true)
        {
            ChooseNewEndpoint();
            
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
    }

    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
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

            if(targetTransform != null && distanceToTarget <= stoppingDistance) // Stop early if close enough to player
                break;

            if(rigidBodyToMove != null)
            {
                animator.SetBool("enemyIsSailing", true);
                SetAnimationDir(direction.normalized);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                rb2d.MovePosition(newPosition);
            }

            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("enemyIsSailing", false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;
            targetTransform = collision.gameObject.transform;

            cannons.target = targetTransform; // CHECK
            fireCoroutine = StartCoroutine(cannons.FireAmmo()); // CHECK

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("enemyIsSailing", false);
            currentSpeed = wanderSpeed;

            if(fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
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

    void SetAnimationDir(Vector3 direction) 
    {
        if(direction.magnitude < 0.01f) // skip if barely moving
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Repeat(angle, 360f);

        if(angle > 337.5f || angle <= 22.5f) // EAST
        {
            animator.SetFloat("enemyXdir", 1f);
            animator.SetFloat("enemyYdir", 0f);
            animator.SetFloat("enemyLastXdir", 1f);
            animator.SetFloat("enemyLastYdir", 0f);
        }
        else if(angle > 22.5f && angle <= 67.5f) // northeast
        {
            animator.SetFloat("enemyXdir", 1f);
            animator.SetFloat("enemyYdir", 1f);
            animator.SetFloat("enemyLastXdir", 1f);
            animator.SetFloat("enemyLastYdir", 1f);
        }
        else if(angle > 67.5f && angle <= 112.5f) // NORTH
        {
            animator.SetFloat("enemyXdir", 0f);
            animator.SetFloat("enemyYdir", 1f);

            animator.SetFloat("enemyLastXdir", 0f);
            animator.SetFloat("enemyLastYdir", 1f);
        }
        else if(angle > 112.5f && angle <= 157.5f) // northwest
        {
            animator.SetFloat("enemyXdir", -1f);
            animator.SetFloat("enemyYdir", 1f);

            animator.SetFloat("enemyLastXdir", -1f);
            animator.SetFloat("enemyLastYdir", 1f);
        }
        else if(angle > 157.5f && angle <= 202.5f) // WEST
        {
            animator.SetFloat("enemyXdir", -1f);
            animator.SetFloat("enemyYdir", 0f);

            animator.SetFloat("enemyLastXdir", -1f);
            animator.SetFloat("enemyLastYdir", 0f);
        }
        else if(angle > 202.5f && angle <= 247.5f) // southwest
        {
            animator.SetFloat("enemyXdir", -1f);
            animator.SetFloat("enemyYdir", -1f);

            animator.SetFloat("enemyLastXdir", -1f);
            animator.SetFloat("enemyLastYdir", -1f);
        }
        else if(angle > 247.5f && angle <= 292.5f) // SOUTH
        {
            animator.SetFloat("enemyXdir", 0f);
            animator.SetFloat("enemyYdir", -1f);

            animator.SetFloat("enemyLastXdir", 0f);
            animator.SetFloat("enemyLastYdir", -1f);
        }
        else if(angle > 292.5f && angle <= 337.5f) // southeast
        {
            animator.SetFloat("enemyXdir", 1f);
            animator.SetFloat("enemyYdir", -1f);

            animator.SetFloat("enemyLastXdir", 1f);
            animator.SetFloat("enemyLastYdir", -1f);
        }
    }

    /*public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while(remainingDistance > float.Epsilon)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
                remainingDistance = (transform.position - endPosition).sqrMagnitude - stoppingDistance; // CHECK
            }
            
            if(rigidBodyToMove != null)
            {
                animator.SetBool("enemyIsSailing", true);
                Vector3 direction = (endPosition - transform.position).normalized; // 4/20/25
                SetAnimationDir(direction); // 4/20/25

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                rb2d.MovePosition(newPosition);
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("enemyIsSailing", false);
    }*/
}
