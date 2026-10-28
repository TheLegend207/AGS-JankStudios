using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    // Self-explainory
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float jumpPower = 7f;
    public float gravity = 15f;
    public float currentSpeed;
    public float lookSpeed = 2f;
    public float lookXLimit = 60f; // How far player can rotate up/down

    // Health related stuff
    public int maxHealth = 100;
    public int currentHealth;
    public TMP_Text healthText;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    private CharacterController characterController;

    private bool canMove = true;


    private void Start()
    {
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateHealthUI();
    }

    private void Update()
    {
        Movement();
        Look();
    }

    // Player movment 
    private void Movement()
    {
        // Gets player's "wasd" input 
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        // Gets player direction
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Allow player to combine verticle and horizontal input
        Vector3 movement = (forward * inputVertical) + (right * inputHorizontal);

        // Prevents diagonal movement from being faster than normal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Prevent player from moving
        if (!canMove)
        {
            currentSpeed = 0f;
        }

        // Shift-walk
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // Apply horizontal movement on z and x axis
        moveDirection.x = movement.x * currentSpeed;
        moveDirection.z = movement.z * currentSpeed;

        // Jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        if (characterController.isGrounded)
        {
            // Prevent player from continously falling 
            if (moveDirection.y < 0f)
            {
                moveDirection.y = -2f;
            }
        }
        else
        {
            // Apply gravity over time
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    //
    private void Look()
    {
        // Stops function when player cannot move
        if (!canMove)
        {
            return;
        }

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed; // Change verticle movement based on mouse speed
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit); // Prevenets player from looking too far up/down
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f); // Rotate camera up/down
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f); // Rotate player left/right
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "+" + currentHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

