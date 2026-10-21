using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float jumpPower = 7f;
    public float gravity = 15f;
    public float currentSpeed;
    public float lookSpeed = 2f;
    public float lookXLimit = 60f;

    public int maxHealth = 100;
    public int currentHealth; //Do health bar later

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    private CharacterController characterController;

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        
    }

    private void Movement()
    {
        //Get player's wasd movement
        float inputVerticle = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        //Get player's direction
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //Allow player to combine verticle and horizontal movement
        Vector3 movement = (forward * inputVerticle) + (right * inputHorizontal);

        //Apply horizontal movement on z and x axis
        moveDirection.x = movement.x * currentSpeed;
        moveDirection.z = movement.z * currentSpeed;

        // Prevents diagonal movement from being faster than normal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        //Jump
        if (Input.GetButton("Jump") & characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        if (characterController.isGrounded)
        {
            if (moveDirection.y < 0)
            {
                moveDirection.y = -2f;
            }

            else
            {
                moveDirection.y -= gravity * Time.deltaTime;    
            }
        }
    }

    private void Look()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
    }
}


