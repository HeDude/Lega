using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //Properties
    private float speed;

    private float maxVelocityChange;

    private float jumpForce;
    private float jumpRaycastDistance;

    private Rigidbody rb;

    [SerializeField] private bool jumpEnabled;

    //Start is called before the first frame update
    private void Start()
    {
        //Sets movement values
        speed = 8f;
        maxVelocityChange = 10.0f;

        jumpForce = 5f;
        jumpRaycastDistance = 1.1f;

        //Gets the rigid body component
        rb = GetComponent<Rigidbody>();
    }

    //Update is called once per frame
    private void Update()
    {
        if(jumpEnabled)
            CheckToJump();
    }

    private void FixedUpdate()
    {
        CheckToMove();
    }

    //Checks if the player can move
    private void CheckToMove()
    {
        //Calculate how fast the player should be going
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        //Apply a force to reach the target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    //Checks if the player can jump
    private void CheckToJump()
    {
        //The player jumps when the player presses space and is on the ground
        if (Input.GetKeyDown(KeyCode.Space))
            if (IsGrounded())
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
    }

    //Checks whether player is grounded
    private bool IsGrounded()
    {
        //Sends a downward raycast to see whether the player is grounded
        return (Physics.Raycast(transform.position, Vector3.down, jumpRaycastDistance));
    }
}
