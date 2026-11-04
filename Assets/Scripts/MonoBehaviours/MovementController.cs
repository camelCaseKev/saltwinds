using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    Vector2 movement = new Vector2();
    Animator animator;
    Rigidbody2D rb2D;


    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateState();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        rb2D.velocity = movement * movementSpeed;
    }

    void UpdateState()
    {
        if(Mathf.Approximately(movement.x, 0) && Mathf.Approximately(movement.y, 0))
        {
            animator.SetBool("isSailing", false);
        }
        else
        {
            animator.SetBool("isSailing", true);

            animator.SetFloat("xLastDir", movement.x);
            animator.SetFloat("yLastDir", movement.y);
        }

        animator.SetFloat("xDir", movement.x);
        animator.SetFloat("yDir", movement.y);
    }





    /*public class MovementController : MonoBehaviour
    {
        public float movementSpeed = 3.0f;
        Vector2 movement = new Vector2();
        Animator animator;
        string animationState = "AnimationState";
        Rigidbody2D rb2D;

        enum CharStates
        {
            sailEast = 1,
            sailSouth = 2,
            sailWest = 3,
            sailNorth = 4,
            sailNortheast = 5,
            sailSoutheast = 6,
            sailSouthwest = 7,
            sailNorthwest = 8,
            idle = 9
        }

        // Start is called before the first frame update
        private void Start()
        {
            animator = GetComponent<Animator>();
            rb2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateState();
        }

        void FixedUpdate()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
            rb2D.velocity = movement * movementSpeed;
        }

        private void UpdateState()
        {
            if (movement.x > 0 && movement.y > 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailNortheast);
            }
            else if (movement.x < 0 && movement.y > 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailNorthwest);
            }
            else if (movement.x > 0 && movement.y < 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailSoutheast);
            }
            else if (movement.x < 0 && movement.y < 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailSouthwest);
            }
            else if (movement.x > 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailEast);
            }
            else if (movement.x < 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailWest);
            }
            else if (movement.y > 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailNorth);
            }
            else if (movement.y < 0)
            {
                animator.SetInteger(animationState, (int)CharStates.sailSouth);
            }
            else
            {
                animator.SetInteger(animationState, (int)CharStates.idle);
            }
        }*/
}
