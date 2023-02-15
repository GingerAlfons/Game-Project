using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider2D cc;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool grounded;
    public bool ducking;

    //Värden för rörelse-variabler
    [Header("Movement")]
    public KeyCode JumpButton;
    public KeyCode LeftButton;
    public KeyCode RightButton;
    public KeyCode DuckButton;
    float horizontalInput = 0f;
    public float accForce = 0f;
    public float maxSpeed = 0f;

    //Värden för hopp-variabler
    [Header("Jump")]
    public LayerMask ground;
    public float jumpStrength = 15f;
    public int doubleJump;
    public Vector2 boxSize = new Vector2(1f, 1f);
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    //Värden för duck-variabler
    [Header("Duck & Slide")]
    public Vector2 ccDefaultSize = new Vector2(1f, 1f);
    public Vector2 ccDefaultOffset = new Vector3(0f, 0f);
    public Vector2 ccDuckSize = new Vector2(1f, 1f);
    public Vector2 ccDuckOffset = new Vector3(0f, 0f);
    public float slideForce = 10f;
    public float airSlideForce = 10f;
    public float slideCD = 0.5f;
    void Start()
    {
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.startTimer <= 0f)
        {
            //Kollar spelarens riktning och vänder prefaben därefter
            if (Input.GetKey(RightButton))
            {
                horizontalInput = 1f;
                sr.flipX = false;
            }
            else if (Input.GetKey(LeftButton))
            {
                horizontalInput = -1f;
                sr.flipX = true;
            }
            else
            {
                horizontalInput = 0f;
            }

            //Kollar om spelarens hastighet är mindre än den maximala
            if (Mathf.Abs(rb.velocity.x) <= maxSpeed)
            {
                Walk();
            }

            //Kollar om spelaren har tryckt ned duck-knappen
            if (ducking)
            {
                if (horizontalInput == 0f)
                {
                    Duck(cc);
                }
                else
                {
                    //Kollar om spelaren är på marken
                    if (grounded)
                    {
                        Slide(cc);
                    }
                    else
                    {
                        AirSlide(cc);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.startTimer <= 0f)
        {
            //Animation
            if (horizontalInput != 0)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }


            //Kollar om spelaren nuddar marken
            grounded = Physics2D.OverlapBox(transform.position + offset, boxSize, 0f, ground);

            //Kollar om spelarens "grounded"-box nuddar marken och om rörelsen i y-led är 0
            if (grounded && rb.velocity.y <= 0)
            {
                doubleJump = 2;
            }

            if (doubleJump > 0 && Input.GetKeyDown(JumpButton))
            {
                Jump();
            }

            //Kollar om spelaren trycker ned duck-knappen
            if (Input.GetKeyDown(DuckButton))
            {
                ducking = true;
            }

            //Kollar om spelaren släpper duck-knappen
            if (Input.GetKeyUp(DuckButton))
            {
                Stand(cc);
            }
        }
    }
    

    public void Walk()
    {
        Vector2 walkDir = new Vector2(horizontalInput * accForce, 0f);
        rb.AddForce(walkDir, ForceMode2D.Force);
    }
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        doubleJump--;
    }
    public void Duck(CapsuleCollider2D cc)
    {
        cc.size = ccDuckSize;
        cc.offset = ccDuckOffset;
    }
    public void Slide(CapsuleCollider2D cc)
    {
        Duck(cc);
        Vector2 walkDir = new Vector2(horizontalInput * slideForce, 0f);
        rb.AddForce(walkDir, ForceMode2D.Impulse);
        ducking = false;
    }
    public void AirSlide(CapsuleCollider2D cc)
    {
        rb.AddForce(new Vector2(0f, airSlideForce), ForceMode2D.Impulse);
        Slide(cc);
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
