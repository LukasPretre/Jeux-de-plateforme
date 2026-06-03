using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    public float groundCheckRadius = 0.42f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float coyoteTime = 0.2f;
    public float jumpContinuesForce = 1.0f;
    public float dashLentgth = 5f;


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private Animator animator;
    private bool isRunning = false;
    private float coyoteTimeCounter;
    private bool hasDash = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(rb.linearVelocityX != 0) 
        { 
            if (rb.linearVelocityX < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            hasDash = true;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;    
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (coyoteTimeCounter > 0f)
            { 
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f;
            }
        }

        if (Input.GetKey(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.AddForce(Vector2.up * jumpContinuesForce);
        }

        if (Input.GetKeyDown(KeyCode.L) && hasDash)
        {
            Vector2 dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            rb.AddForce(dashDirection * dashLentgth, ForceMode2D.Impulse);
            hasDash = false;
        }

        SetAnimation(moveInput);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded)
        {
            bool shouldRun = moveInput != 0;

            // Utiliser des bools pour Idle/Run (pas de triggers)
            if (shouldRun && !isRunning)
            {
                animator.SetBool("IsRunning", true);
                isRunning = true;
            }
            else if (!shouldRun && isRunning)
            {
                animator.SetBool("IsRunning", false);
                isRunning = false;
            }
        }
        else
        {
            animator.SetBool("IsGrounded", false);
            if (rb.linearVelocityY > 0)
            {
                animator.SetTrigger("Jump");
            }
            else
            {
                animator.SetTrigger("Fall");
            }
        }
    }
}
 