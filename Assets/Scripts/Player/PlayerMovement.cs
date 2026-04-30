using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = Vector3.zero;



    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // reset velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //movement controller
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // check if player is moving
        if (lastPosition != gameObject.transform.position && isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastPosition = gameObject.transform.position;
    }
}
