using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public Rigidbody2D rbWeaponDrop;
    public SpriteRenderer weaponSprite;
    public LayerMask ground;
    public Vector2 boxSize;
    public Vector3 offset;

    public Weapon weaponValue;

    // Start is called before the first frame update
    void Start()
    {
        //S�tter spriten p� WeaponDrop objektet till weaponArtwork f�r den slumpade vapentypen
        weaponSprite.sprite = weaponValue.weaponArtwork;

        //S�tter en start hastighet i negativ y-led
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
