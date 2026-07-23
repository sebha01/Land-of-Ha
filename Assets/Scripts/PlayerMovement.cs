using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprRenderer;
    public float moveSpeed = 5.0f;

    float horizontalMovement;
    bool lastFacing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
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
}
