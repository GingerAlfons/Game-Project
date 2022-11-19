using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public Rigidbody2D rb;
    public CapsuleCollider2D cc;
    public float speed = 2f;
    public KeyCode JumpButton;
    public KeyCode LeftButton;
    public KeyCode RightButton;
    public KeyCode DuckButton;
    float HorizontalInput = 0f;

    //Värden för hopp-variabler
    [Header("Jump")]
    public LayerMask ground;
    public float jumpStrength = 15f;
    public int doubleJump;
    public Vector2 boxSize = new Vector2(1f, 1f);
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    //Värden för duck-variabler
    [Header("Duck")]
    public Vector2 ccDefaultSize = new Vector2(1f, 1f);
    public Vector2 ccDefaultOffset = new Vector3(0f, 0f);
    public Vector2 ccDuckSize = new Vector2(1f, 1f);
    public Vector2 ccDuckOffset = new Vector3(0f, 0f);

    void Start()
    {
    }

    void Update()
    {
        //Animation
        if (HorizontalInput != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


        if (GameManager.Instance.startTimer > 0f)
            return;

        //Kollar spelarens riktning och vänder prefaben därefter
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (Input.GetKey(RightButton))
        {
            HorizontalInput = 1f;
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(LeftButton))
        {
            HorizontalInput = -1f;
            spriteRenderer.flipX = true;
        }
        else
        {
            HorizontalInput = 0f;
        }
        rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);
     

        //Kollar om spelaren nuddar marken och om den har tillåtelse att hoppa
        bool grounded = Physics2D.OverlapBox(transform.position + offset, boxSize, 0f, ground);
        if (grounded && rb.velocity.y <= 0)
        {
            doubleJump = 2;
        }

        if (doubleJump > 0 && Input.GetKeyDown(JumpButton))
        {
            Jump();
        }

        //Ducka
        CapsuleCollider2D capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        if (Input.GetKey(DuckButton))
        {
            Duck(capsuleCollider2D);
        }
        else
        {
            Stand(capsuleCollider2D);
        }
    }
    

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        doubleJump--;
    }
    public void Duck(CapsuleCollider2D capsuleCollider2D)
    {
        capsuleCollider2D.size = ccDuckSize;
        capsuleCollider2D.offset = ccDuckOffset;
    }
    public void Stand(CapsuleCollider2D capsuleCollider2D)
    {
        capsuleCollider2D.size = ccDefaultSize;
        capsuleCollider2D.offset = ccDefaultOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }

}
