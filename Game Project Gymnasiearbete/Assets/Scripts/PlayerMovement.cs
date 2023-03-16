using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider2D cc;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool grounded; //Sann om spelaren �r i kontakt med marken
    public bool wantSlide; //Sann om spelaren tryckt ned slide-knappen
    public bool sliding; //Sann om spelaren glider
    public bool jumping; //Sann om spelaren hoppar

    //V�rden f�r r�relse-variabler
    [Header("Movement")]
    public KeyCode JumpButton;
    public KeyCode LeftButton;
    public KeyCode RightButton;
    public KeyCode SlideButton;
    float horizontalInput = 0f;
    public float accForce = 0f;
    public float maxSpeed = 0f;

    //V�rden f�r hopp-variabler
    [Header("Jump")]
    public LayerMask ground;
    public float jumpStrength = 15f;
    public int doubleJump;
    public Vector2 groundBoxSize = new Vector2(1f, 1f);
    public Vector3 groundBoxOffset = new Vector3(0f, 0f, 0f);

    //V�rden f�r duck-variabler
    [Header("Slide")]
    public Vector2 ccDefaultSize = new Vector2(1f, 1f);
    public Vector2 ccDefaultOffset = new Vector3(0f, 0f);
    public Vector2 ccSlideSize = new Vector2(1f, 1f);
    public Vector2 ccSlideOffset = new Vector3(0f, 0f);
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
            //Kollar spelarens riktning och v�nder prefaben d�refter
            if (Input.GetKey(RightButton))
            {
                horizontalInput = 1f;
                sr.flipX = false; //V�nder spriten
            }
            else if (Input.GetKey(LeftButton))
            {
                horizontalInput = -1f;
                sr.flipX = true; //V�nder spriten
            }
            else
            {
                horizontalInput = 0f;
            }

            //Kollar om spelarens hastighet �r mindre �n den maximala
            if (Mathf.Abs(rb.velocity.x) <= maxSpeed)
            {
                Walk();
            }

            //Kollar om spelaren har tryckt ned hopp-knappen 
            if (jumping)
            {
                Jump();
            }

            //Kollar om duck-knappen har blivit nedtryckt, att spelaren �r p� marken samt att den inte redan glider
            if (grounded && wantSlide && !sliding)
            {
                StartCoroutine(Slide(cc));
            }
            //Kollar om duck-knappen har blivit nedtryckt samt att den inte redan glider
            else if (wantSlide && !sliding)
            {
                AirSlide(cc);
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.startTimer <= 0f)
        {
            //Animation f�r g�
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

            //Kollar om spelarens "grounded"-box nuddar marken samt om r�relsen i y-led �r 0
            if (grounded && rb.velocity.y <= 0)
            {
                doubleJump = 2;
            }
            //Kollar om spelaren kan hoppa samt om den trycker ned hopp-knappen
            if (doubleJump > 0 && Input.GetKeyDown(JumpButton))
            {
                jumping = true;
            }

            //Kollar om spelaren trycker ned slide-knappen
            if (Input.GetKeyDown(SlideButton))
            {
                wantSlide = true;
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
        FindObjectOfType<AudioManager>().Play("Jump");
        rb.velocity = new Vector3(rb.velocity.x, jumpStrength, 0f); //S�tter en hastighet riktad upp�t
        doubleJump--;
        animator.SetTrigger("jump"); //Triggar hoppanimationen
        jumping = false;
    }


    public IEnumerator Slide(CapsuleCollider2D cc)
    {
        wantSlide = false;
        sliding = true;
        animator.SetTrigger("slide");
        SlideHitBox(cc);
        Vector2 slideDir = new Vector2(horizontalInput * slideForce, 0f);
        rb.AddForce(slideDir, ForceMode2D.Impulse);
        yield return new WaitForSeconds(slideCD);
        DefaultHitBox(cc);
        sliding = false;
    }

    public void AirSlide(CapsuleCollider2D cc)
    {
        rb.AddForce(new Vector2(0f, airSlideForce), ForceMode2D.Impulse);
        StartCoroutine(Slide(cc));
    }

    public void SlideHitBox(CapsuleCollider2D cc)
    {
        cc.size = ccSlideSize;
        cc.offset = ccSlideOffset;
    }

    public void DefaultHitBox(CapsuleCollider2D capsuleCollider2D)
    {
        capsuleCollider2D.size = ccDefaultSize;
        capsuleCollider2D.offset = ccDefaultOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + groundBoxOffset, groundBoxSize);
    }
}
