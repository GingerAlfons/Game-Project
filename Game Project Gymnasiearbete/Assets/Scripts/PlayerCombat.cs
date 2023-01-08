using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health;

    [SerializeField] float damageInterval = 1f;
    float dmgTimer;
    [SerializeField] int damageAmount = 5;

    public KeyCode AttackButton;
    public KeyCode LeftButton;
    public KeyCode RightButton;
    public KeyCode InteractButton;
    public Vector2 attackBoxSize = new Vector2(2f, 1f);
    public Vector3 attackBoxOffset = new Vector3(0f, 1f, 0f);
    float HorizontalInput = 0f;
    bool isAttacking = false;

    public Weapon activeWeapon;

    public Healthbar healthbar;

    [SerializeField] public LayerMask player;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthbar.setmaxhealth(maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            
        }

        //Kollar om spelaren vill attackera
        if (Input.GetKeyDown(AttackButton))
        {
            StartAttack();
        }

        //Kollar spelarens riktning inför attack
        if (Input.GetKey(RightButton))
        {
            HorizontalInput = 1f;
            attackBoxOffset.x = 1;
        }
        else if (Input.GetKey(LeftButton))
        {
            HorizontalInput = -1f;
            attackBoxOffset.x = -1;
        }
    }

    //Damage systemet
    public void Damage(int dmg)
    {
        health -= dmg;
        healthbar.sethealth(health);
        if (health <= 0)
            Kill();
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

        AttackBox();

        yield return new WaitForSeconds(activeWeapon.attackCooldown);
        isAttacking = false;
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
                Debug.Log(cda[i].gameObject.name);

                /*Targeted Player, Weapon*/
                Knockback(cda[i].gameObject, activeWeapon.knockback); 
            }
        }
    }

    public void Knockback(GameObject enemy, float knockbackForce)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        
        Vector2 angle = new Vector2(enemy.transform.position.x - transform.position.x, enemy.transform.position.y - transform.position.y).normalized;
        Debug.Log(angle.x);
        
        rb.AddForce(angle * knockbackForce, ForceMode2D.Force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + attackBoxOffset, attackBoxSize);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
