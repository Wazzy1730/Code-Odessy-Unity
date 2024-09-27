using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;

    private IInteractable interactable; // Added interactable reference

    public float moveSpeed;
    private Rigidbody2D rb;
    private float x;
    private float y;
    private Vector2 input;
    private bool moving;

    [SerializeField] private InputActionReference moveActionToUse;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (anim == null)
        {
            anim = GetComponent<Animator>(); // Automatically assign Animator if it's not set in the Inspector
        }
    }


    private void Update()
    {
        if (moveActionToUse != null)
        {
            Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogError("moveActionToUse is not assigned.");
        }

        GetInput();
        Animate();
    }



    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed; // Assign velocity properly
    }

    private void GetInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        input = new Vector2(x, y).normalized; // Normalize for consistent speed in all directions
    }

    private void Animate()
    {
        moving = input.magnitude > 0.1f;

        if (moving)
        {
            anim.SetFloat("X", x);
            anim.SetFloat("Y", y);
        }

        anim.SetBool("Moving", moving);
    }

    // Detect interaction with objects tagged as "Interactable"
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            interactable = collision.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(); // Trigger open chest animation
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (interactable != null)
            {
                interactable.StopInteract(); // Trigger close chest animation
                interactable = null;
            }
        }
    }
}
