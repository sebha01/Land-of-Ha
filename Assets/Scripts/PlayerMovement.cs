using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprRenderer;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    float horizontalMovement;
    bool lastFacing;

    [Header("Jumping")]
    public float jumpPower = 10.0f;
    public int maxJumps = 2;
    int jumpsRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    public float groundCheckOffsetX = 0.06f;

    [Header("Gravity")]
    public float baseGravity = 2.0f;
    public float maxFallSpeed = 18.0f;
    public float fallSpeedMultiplier = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        GroundCheck();
    }

    private void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        { 
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, - maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;   

        sprRenderer.flipX = horizontalMovement < 0 ? true : horizontalMovement > 0 ? false : lastFacing;
        
        if (horizontalMovement != 0)
        {
            lastFacing = horizontalMovement < 0;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
            }
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(groundCheckPos.position.x - groundCheckOffsetX, groundCheckPos.position.y, groundCheckPos.position.z), groundCheckSize);
    }
}
