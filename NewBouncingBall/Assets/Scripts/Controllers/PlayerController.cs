using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 6f;

    [Header("Hold Jump (subtle)")]
    public float holdBoost = 15f;   
    public float holdTime = 0.12f;

    [Header("Flip In Air")]
    public float flipSpeed = 720f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    Rigidbody2D rb;
    bool isGrounded;

    bool holdingJump;
    float holdCounter;

    bool isFlipping;

    float jumpCooldown;

    public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (jumpAction != null) jumpAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (jumpAction != null) jumpAction.action.Disable();
    }

    void Update()
    {
        if (jumpCooldown > 0f)
            jumpCooldown -= Time.deltaTime;

        // Ground check 
        if (groundCheck != null && jumpCooldown <= 0f)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Move input
        Vector2 moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        float x = moveInput.x;

        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        if (anim) anim.SetFloat("MoveX", x);

        // Jump input
        bool jumpDown = jumpAction != null && jumpAction.action.WasPressedThisFrame();
        bool jumpHeld = jumpAction != null && jumpAction.action.IsPressed();
        bool jumpUp   = jumpAction != null && jumpAction.action.WasReleasedThisFrame();

        // Start jump
        if (jumpDown && isGrounded)
        {
            rb.linearVelocity = new Vector2(x * moveSpeed, jumpForce);

            holdingJump  = true;
            holdCounter  = holdTime;
            isFlipping   = true;
            isGrounded   = false;

            jumpCooldown = 0.1f; 
        }

        // Hold boost 
        if (jumpHeld && holdingJump && holdCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,
                                            rb.linearVelocity.y + holdBoost * Time.deltaTime);
            holdCounter -= Time.deltaTime;
        }

        if (jumpUp)
            holdingJump = false;

        // Flip in air
        if (!isGrounded && isFlipping)
            transform.Rotate(0f, 0f, -flipSpeed * Time.deltaTime);

        // Land on feet
        if (isGrounded && isFlipping && rb.linearVelocity.y <= 0f)
        {
            isFlipping = false;
            transform.rotation = Quaternion.identity;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}