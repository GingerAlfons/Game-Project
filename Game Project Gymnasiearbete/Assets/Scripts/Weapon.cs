using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public Sprite weaponArtwork;
    public float knockback;
    public float attackCooldown;
    public float attackDelay;
    public Vector2 attackBoxSize;
    public Vector3 attackBoxOffset;
}
