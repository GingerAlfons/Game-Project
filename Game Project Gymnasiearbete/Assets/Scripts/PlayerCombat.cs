using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //Spelarens komponenter
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    //Vapnets komponenter
    public SpriteRenderer WeaponSpriteRenderer;
    public Animator weaponAnimator;

    //Liv-variabler
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health;
    public Healthbar healthbar;

    //Kamera-skada variabler
    [SerializeField] float damageInterval = 1f;
    float dmgTimer;
    [SerializeField] int damageAmount = 5;

    //Attack-variabler
    [SerializeField] private KeyCode AttackButton;
    [SerializeField] private KeyCode LeftButton;
    [SerializeField] private KeyCode RightButton;
    [SerializeField] private KeyCode InteractButton;
    [SerializeField] private Vector2 attackBoxSize = new Vector2();
    [SerializeField] private Vector3 attackBoxOffset = new Vector3();
    private bool isAttacking = false;

    //Vapen-variabler
    public Weapon activeWeapon;
    public Vector2 interactBox = new Vector2(2f,2f);

    [SerializeField] public LayerMask player;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //Start timer till spelet
        if (GameManager.Instance.startTimer > 0f)
            return; 

        
        if (!spriteRenderer.isVisible)
        {
            if (dmgTimer > 0)
                dmgTimer -= Time.deltaTime;
            else
            {
                dmgTimer = damageInterval;
                Damage(damageAmount);
            }
        }

        //Kollar om spelaren vill ta upp ett nytt vapen
        if (Input.GetKeyDown(InteractButton))
        {
            CheckForWeapon();
        }

        //Kollar om spelaren vill attackera
        if (Input.GetKeyDown(AttackButton))
        {
            StartAttack();
        }

        //Kollar spelarens riktning inför attack
        if (Input.GetKey(RightButton))
        {
            attackBoxOffset.x = 1;
            WeaponSpriteRenderer.flipX = false; //Vänder tillbaka vapenspriten
            weaponAnimator.SetBool("facingRight", true); //Ändrar animationsparametern
        }
        else if (Input.GetKey(LeftButton))
        {
            attackBoxOffset.x = -1;
            WeaponSpriteRenderer.flipX = true; //Vänder vapenspriten
            weaponAnimator.SetBool("facingRight", false);
        }
    }

    //Damage systemet
    public void Damage(int dmg)
    {
        health -= dmg;
        healthbar.SetHealth(health);
        if (health <= 0)
            GameManager.Instance.EndGame(gameObject);
    }

    public void StartAttack()
    {
        if (isAttacking == false)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        //Triggar slåanimation
        TriggerHitAnimation();

        yield return new WaitForSeconds(activeWeapon.attackDelay);

        AttackBox();

        yield return new WaitForSeconds(activeWeapon.attackCooldown);
        isAttacking = false;

        //Återställer slåanimation
        ResetHitAnimation();
    }

    public void AttackBox()
    {
        Collider2D[] cda = Physics2D.OverlapBoxAll(transform.position + attackBoxOffset, attackBoxSize, 0f, player);
        for (int i = 0; i < cda.Length; i++)
        {
            if (cda[i].gameObject == gameObject)
            {
                continue;
            }    

            PlayerCombat pc = cda[i].GetComponent<PlayerCombat>();
            if (pc)
            {
                //Targeted Player, Weapon
                Knockback(cda[i].gameObject, activeWeapon.knockback); 
            }
        }
    }

    public void CheckForWeapon()
    {
        Collider2D[] cda = Physics2D.OverlapBoxAll(transform.position, interactBox, 0f);
        for (int i = 0; i < cda.Length; i++)
        {
            WeaponDrop wp = cda[i].GetComponent<WeaponDrop>();
            if (wp)
            {
                //Återställer vapenanimationen
                weaponAnimator.ResetTrigger("holding" + activeWeapon.name);
                //Sätter spelarens aktiva vapen till vapnet på objektet
                activeWeapon = wp.weaponValue;
                //Raderar WeaponDrop objektet
                Destroy(wp.gameObject);
                WeaponHoldAnimation();
            }
        }
    }

    public void Knockback(GameObject enemy, float knockbackForce)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        
        Vector2 angle = new Vector2(enemy.transform.position.x - transform.position.x, enemy.transform.position.y - transform.position.y).normalized;
        FindObjectOfType<AudioManager>().Play("Hit");
        rb.AddForce(angle * knockbackForce, ForceMode2D.Force);
    }

    //Kollar vilket vapen spelaren håller i och spelar upp respektive animation
    public void WeaponHoldAnimation()
    {
        //Kollar vilket vapen som är aktivt och startar en animation beroende på det
        if (activeWeapon.name != "fists")
        {
            FindObjectOfType<AudioManager>().Play("Pickup");
            weaponAnimator.SetTrigger("holding" + activeWeapon.name);
        }
    }

    //Startar slåanimation
    public void TriggerHitAnimation()
    {
        if (activeWeapon.name == "Fists")
        {
            animator.SetTrigger("hit" + activeWeapon.name);
        }
        else
        {
            weaponAnimator.SetTrigger("hit" + activeWeapon.name);
        }
    }

    //Återställer slåanimation
    public void ResetHitAnimation()
    {
        if (activeWeapon.name == "Fists")
        {
            animator.ResetTrigger("hit" + activeWeapon.name);
        }
        else
        {
            weaponAnimator.ResetTrigger("hit" + activeWeapon.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + attackBoxOffset, attackBoxSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, interactBox);
    }
}
