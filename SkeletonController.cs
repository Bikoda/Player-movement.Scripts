using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{

    public float skeletonSpeed = 1.0f;
    public float rotateSkele = 90.0f;
    public float rotationSpeed = 100.0f;
    public RuntimeAnimatorController walkSkele;
    public RuntimeAnimatorController idleSkele;
    //public RuntimeAnimatorController attackSkele;
    public BoxCollider skeleBox;
    public GameObject playerSkele;
    private Rigidbody skeletonRb;
    private Animator playerAnim;
    public bool isInCombat = false;

    private enum Direction
    {
        Forward,
        Right,
        Backward,
        Left
    }

    private Direction currentDirection = Direction.Forward;

    // Start is called before the first frame update
    void Start()
    {
        // Get necessary components
        GameObject enemyBox = GameObject.FindWithTag("Enemy");
        playerSkele = GameObject.Find("Skeleton");
        skeleBox = playerSkele.GetComponent<BoxCollider>();
        skeletonRb = playerSkele.GetComponent<Rigidbody>();
        playerAnim = playerSkele.GetComponent<Animator>();

        // Ensure RuntimeAnimatorController is correctly assigned in the Inspector
        if (walkSkele == null || idleSkele == null)
        {
            Debug.LogError("Animator controllers are not assigned in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calls the method to move skeleton
        SkeleMovement();
        HandleRotationInput();

    }

    // Moves the skeleton on X and Z
    void SkeleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = new Vector3(horizontalInput, 0.0f, verticalInput) * skeletonSpeed;

        // Update the Rigidbody velocity to move the player
        skeletonRb.velocity = new Vector3(movementInput.x, skeletonRb.velocity.y, movementInput.z);

        // Provides animation upon input switching between idle and walking
        if (horizontalInput != 0 || verticalInput != 0)
        {
            playerAnim.SetBool("isWalking", true);
            playerAnim.runtimeAnimatorController = walkSkele;

            // Rotates the character based on input direction
            Vector3 direction = new Vector3(horizontalInput, 0.0f, verticalInput);

            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                playerSkele.transform.rotation = Quaternion.RotateTowards(playerSkele.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            playerAnim.SetBool("isWalking", false);
            playerAnim.runtimeAnimatorController = idleSkele;
        }
    }

    // Adds the functionality of when entering the box collider of enemy bool isInCombat
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isInCombat = true;
        }
        else
        {
            isInCombat = false;
        }
    }

    void RotateCharacter(float angle)
    {
        // Rotate the character by the given angle around the y-axis
        playerSkele.transform.Rotate(0, angle, 0);
    }

    void UpdateDirection(bool isRightArrow)
    {
        switch (currentDirection)
        {
            case Direction.Forward:
                currentDirection = isRightArrow ? Direction.Right : Direction.Left;
                break;
            case Direction.Right:
                currentDirection = isRightArrow ? Direction.Backward : Direction.Forward;
                break;
            case Direction.Backward:
                currentDirection = isRightArrow ? Direction.Left : Direction.Right;
                break;
            case Direction.Left:
                currentDirection = isRightArrow ? Direction.Forward : Direction.Backward;
                break;
        }
    }

    void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateCharacter(90);
            UpdateDirection(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateCharacter(180);
            UpdateDirection(false);
        }
    }


}