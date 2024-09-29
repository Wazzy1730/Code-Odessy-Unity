using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    private IInteractable interactable;  // Reference for interactable objects
    public float moveSpeed;
    private Rigidbody2D rb;
    private Vector2 input;  // Stores input vector
    private bool moving;

    [SerializeField] private InputActionReference moveActionToUse;  // Joystick input

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (anim == null)
        {
            anim = GetComponent<Animator>();  // Automatically assign Animator if it's not set
        }
    }

    private void Update()
    {
        // Handle input from both joystick and keyboard
        GetInput();

        // Animate player based on movement input
        Animate();
    }

    private void FixedUpdate()
    {
        // Use Rigidbody velocity for movement
        rb.velocity = input * moveSpeed;
    }

    private void GetInput()
    {
        // Get input from joystick
        if (moveActionToUse != null)
        {
            input = moveActionToUse.action.ReadValue<Vector2>();
        }
        else
        {
            Debug.LogError("moveActionToUse is not assigned.");
        }

        // Add keyboard movement input on top
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 keyboardInput = new Vector2(x, y);

        // Combine joystick and keyboard input
        if (keyboardInput.magnitude > 0.1f)
        {
            input = keyboardInput.normalized;  // Normalize to ensure consistent speed
        }
    }

    private void Animate()
    {
        // Check if the player is moving
        moving = input.magnitude > 0.1f;

        if (moving)
        {
            anim.SetFloat("X", input.x);  // Set animator X-axis
            anim.SetFloat("Y", input.y);  // Set animator Y-axis
        }

        anim.SetBool("Moving", moving);  // Set movement state
    }

    // Handle interaction with objects tagged "Interactable"
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            interactable = collision.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();  // Trigger interaction (e.g., open chest)
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (interactable != null)
            {
                interactable.StopInteract();  // Stop interaction (e.g., close chest)
                interactable = null;
            }
        }
    }
}
