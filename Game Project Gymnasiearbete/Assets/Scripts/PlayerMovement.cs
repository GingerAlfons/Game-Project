using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 2f;
    public KeyCode JumpButton;
    public KeyCode LeftButton;
    public KeyCode RightButton;
    float HorizontalInput = 0f;

    [Header("Jump")]
    public LayerMask ground;
    public float jumpStrength = 15f;
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

        if (grounded && Input.GetKeyDown(JumpButton))
            Jump();

        //Flippar spriten beroende på vilket håll man kollar åt

    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }
}
