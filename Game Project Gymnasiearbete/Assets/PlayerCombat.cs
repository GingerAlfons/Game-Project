using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health;

    [SerializeField] float damageInterval = .1f;
    float dmgTimer;
    [SerializeField] int damageAmount = 5;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void Damage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            Kill();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
