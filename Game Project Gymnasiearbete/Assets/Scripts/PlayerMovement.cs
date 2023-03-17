using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Spelarens komponenter
    public Animator animator;
    public CapsuleCollider2D cc;
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    //Rörelse-variabler
    [Header("Movement")]
    [SerializeField] private KeyCode JumpButton;
    [SerializeField] private KeyCode LeftButton;
    [SerializeField] private KeyCode RightButton;
    [SerializeField] private KeyCode SlideButton;
    private float horizontalInput = 0f;
    [SerializeField] private float accForce;
    [SerializeField] private float maxSpeed;
    public bool grounded; //Sann om spelaren är i kontakt med marken
    public bool wantSlide; //Sann om spelaren tryckt ned slide-knappen
    public bool sliding; //Sann om spelaren glider
    public bool jumping; //Sann om spelaren hoppar

    //Hopp-variabler
    [Header("Jump")]
    public LayerMask ground;
    [SerializeField] private float jumpStrength;
    [SerializeField] private int doubleJump;
    [SerializeField] private Vector2 groundBoxSize = new Vector2();
    [SerializeField] private Vector3 groundBoxOffset = new Vector3();

    //Slide-variabler
    [Header("Slide")]
    [SerializeField] private Vector2 ccDefaultSize = new Vector2();
    [SerializeField] private Vector2 ccDefaultOffset = new Vector3();
    [SerializeField] private Vector2 ccSlideSize = new Vector2();
    [SerializeField] private Vector2 ccSlideOffset = new Vector3();
    [SerializeField] private float slideForce;
    [SerializeField] private float airSlideForce;
    [SerializeField] private float slideCD;
    
    private void FixedUpdate()
    {
        if (GameManager.Instance.startTimer <= 0f)
        {
            

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

            //Kollar om duck-knappen har blivit nedtryckt, att spelaren är på marken samt att den inte redan glider
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
            //Spelarriktning
            Direction();

            //Gå-animation
            WalkAnimation();

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

            //Kollar om spelaren trycker ned slide-knappen
            if (Input.GetKeyDown(SlideButton))
            {
                wantSlide = true;
            }
        }
    }

    public void Direction()
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
    }
    
    //Gå-animation
    public void WalkAnimation()
    {
        if (horizontalInput != 0)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
    }
    //Skapar en horisontel kraft som rör spelaren
    public void Walk()
    {
        Vector2 walkDir = new Vector2(horizontalInput * accForce, 0f);
        rb.AddForce(walkDir, ForceMode2D.Force); //Skapar en kraft åt det hållet spelaren är riktad
    }

    //Skapar en hastighet riktad uppåt som rör spelaren + Triggar hopp-animation + Spelar hopp-ljud
    public void Jump()
    {
        FindObjectOfType<AudioManager>().Play("Jump");
        rb.velocity = new Vector3(rb.velocity.x, jumpStrength, 0f); //Sätter en hastighet riktad uppåt
        doubleJump--;
        animator.SetTrigger("jump"); //Triggar hoppanimationen
        jumping = false;
    }

    //Skapar en hastighet i sidled på spelaren + Triggar slide-animation
    public IEnumerator Slide(CapsuleCollider2D cc)
    {
        wantSlide = false;
        sliding = true;
        animator.SetTrigger("slide"); //Triggar slide-animation
        SlideHitBox(cc);
        Vector2 slideDir = new Vector2(horizontalInput * slideForce, 0f);
        rb.AddForce(slideDir, ForceMode2D.Impulse); //Skapar en kraft åt det hållet spelaren är riktad
        yield return new WaitForSeconds(slideCD);
        DefaultHitBox(cc);
        sliding = false;
    }

    //Skapar en kraft uppåt på spelaren
    public void AirSlide(CapsuleCollider2D cc)
    {
        rb.AddForce(new Vector2(0f, airSlideForce), ForceMode2D.Impulse); //Skapar en kraft u
        StartCoroutine(Slide(cc));
    }

    //Ändrar hitboxen
    public void SlideHitBox(CapsuleCollider2D cc)
    {
        cc.size = ccSlideSize;
        cc.offset = ccSlideOffset;
    }

    //Återställer hitboxen
    public void DefaultHitBox(CapsuleCollider2D capsuleCollider2D)
    {
        capsuleCollider2D.size = ccDefaultSize;
        capsuleCollider2D.offset = ccDefaultOffset;
    }

    //Ritar ut hitboxar
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + groundBoxOffset, groundBoxSize);
    }
}
