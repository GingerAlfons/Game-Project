using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public CapsuleCollider2D cc;
    public float speed = 2f;
    public KeyCode JumpButton;
    public KeyCode LeftButton;
    public KeyCode RightButton;
    public KeyCode DuckButton;
    float HorizontalInput = 0f;

    [Header("Jump")]
    public LayerMask ground;
    public float jumpStrength = 15f;
    public int doubleJump;
    public Vector2 boxSize = new Vector2(1f, 1f);
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    void Start()
    {

    }

    void Update()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (GameManager.Instance.startTimer > 0f)
            return;

        if (Input.GetKey(RightButton))
        {
            HorizontalInput = 1f;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(LeftButton))
        {
            HorizontalInput = -1f;
            spriteRenderer.flipX = false;
        }
        else
        {
            HorizontalInput = 0f;
        }
     
        bool grounded = Physics2D.OverlapBox(transform.position + offset, boxSize, 0f, ground);
        rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);

        if (grounded && rb.velocity.y <= 0)
        {
            doubleJump = 2;
        }

        if (doubleJump > 0 && Input.GetKeyDown(JumpButton))
        {
            Debug.Log(doubleJump);
            Jump();
        }
    }
    

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        doubleJump--;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }

    private void Duck()
    {

    }
}
