using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public Rigidbody2D rbWeaponDrop;
    public LayerMask ground;
    public Vector2 boxSize;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //Sätter en start hastighet i negativ y-led
        rbWeaponDrop.velocity = new Vector2(0f,-2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Kollar om objektet nuddar ground
        if (Physics2D.OverlapBox(transform.position + offset, boxSize, 0f, ground))
        {
            //Stannar objektet
            rbWeaponDrop.velocity = new Vector2(0f,0f);
        }
    }
}
