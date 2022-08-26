using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 2f;

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
        bool grounded = Physics2D.OverlapBox(transform.position + offset, boxSize, 0f, ground);
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

        if (grounded && Input.GetButtonDown("Jump"))
            Jump();
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
