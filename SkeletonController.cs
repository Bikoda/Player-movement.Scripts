using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SkeletonController : MonoBehaviour

{

    private float skeletonSpeed = 1.0f;
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

    // Start is called before the first frame update
    void Start()
    {
        GameObject enemyBox = GameObject.FindWithTag("Enemy");
        // Make sure to find the playerSkele GameObject
        playerSkele = GameObject.Find("Skeleton");
        skeleBox = playerSkele.AddComponent<BoxCollider>();


        // Get necessary components
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
        SkeleMovement();

        /*if (isInCombat == true) 
        {

            playerAnim.SetBool("isBeingAttacked", true);
            playerAnim.runtimeAnimatorController = attackSkele;
        }
        if (isInCombat == false)
        {

            playerAnim.SetBool("isBeingAttacked", false);
            playerAnim.runtimeAnimatorController = attackSkele;
        }*/
    }




    void SkeleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = new Vector3(horizontalInput, 0.0f, verticalInput) * skeletonSpeed;

        // Update the Rigidbody velocity to move the player
        skeletonRb.velocity = new Vector3(movementInput.x, skeletonRb.velocity.y, movementInput.z);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            playerAnim.SetBool("isWalking", true);
            playerAnim.runtimeAnimatorController = walkSkele;



            if (horizontalInput != 0)
            {
                float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
                skeletonRb.transform.Rotate(0, -rotationAmount, 0);
            }
            if (verticalInput != 0)
            {
                float rotationAmount = -verticalInput * rotationSpeed * Time.deltaTime;
                skeletonRb.transform.Rotate(0, -rotationAmount, 0);
            }

        }
        else
        {
            playerAnim.SetBool("isWalking", false);
            playerAnim.runtimeAnimatorController = idleSkele;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other = enemyBox)
        {

            isInCombat = true;
        }
        else
        {
            isInCombat = false;
        }
    }*/


}