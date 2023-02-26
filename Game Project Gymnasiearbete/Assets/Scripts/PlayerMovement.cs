using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider2D cc;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool grounded; //Sann om spelaren är i kontakt med marken
    public bool ducking; //Sann om spelaren duckar
    public bool sliding; //Sann om spelaren glider
    public bool jumping;

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
    public Vector2 groundBoxSize = new Vector2(1f, 1f);
    public Vector3 groundBoxOffset = new Vector3(0f, 0f, 0f);

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

            //Kollar om spelaren har tryckt ned hopp-knappen 
            if (jumping)
            {
                Jump();
            }

            //Kollar om duck-knappen har blivit nedtryckt samt att höger- eller vänster-knapp inte är nedtryckt
            if (ducking && horizontalInput == 0)
            {
                Duck(cc);
            }
            //Kollar om duck-knappen har blivit nedtryckt, att spelaren är på marken samt att den inte redan glider
            else if (ducking && grounded && !sliding)
            {
                StartCoroutine(Slide(cc));
            }
            //Kollar om duck-knappen har blivit nedtryckt samt att den inte redan glider
            else if (ducking && !sliding)
            {
                AirSlide(cc);
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
                animator.SetBool("walking", true);
            }
            else
            {
                animator.SetBool("walking", false);
            }


            //Kollar om spelaren nuddar marken
            grounded = Physics2D.OverlapBox(transform.position + groundBoxOffset, groundBoxSize, 0f, ground);

            //Kollar om spelarens "grounded"-box nuddar marken samt om rörelsen i y-led är 0
            if (grounded && rb.velocity.y <= 0)
            {
                doubleJump = 2;
            }
            //Kollar om spelaren kan hoppa samt om den trycker ned hopp-knappen
            if (doubleJump > 0 && Input.GetKeyDown(JumpButton))
            {
                jumping = true;
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
        rb.velocity = new Vector3(rb.velocity.x, jumpStrength, 0f);
        doubleJump--;
        jumping = false;
    }

    public void Duck(CapsuleCollider2D cc)
    {
        cc.size = ccDuckSize;
        cc.offset = ccDuckOffset;
    }

    public IEnumerator Slide(CapsuleCollider2D cc)
    {
        sliding = true;
        Duck(cc);
        Vector2 slideDir = new Vector2(horizontalInput * slideForce, 0f);
        rb.AddForce(slideDir, ForceMode2D.Impulse);
        yield return new WaitForSeconds(slideCD);
        sliding = false;
    }

    public void AirSlide(CapsuleCollider2D cc)
    {
        rb.AddForce(new Vector2(0f, airSlideForce), ForceMode2D.Impulse);
        StartCoroutine(Slide(cc));
    }

    public void Stand(CapsuleCollider2D capsuleCollider2D)
    {
        capsuleCollider2D.size = ccDefaultSize;
        capsuleCollider2D.offset = ccDefaultOffset;
        ducking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + groundBoxOffset, groundBoxSize);
    }
}
