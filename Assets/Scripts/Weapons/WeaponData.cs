using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public int attackRange = 1;
    public int attackDamage = 10;
    public int durability = 10;
    public RuntimeAnimatorController animatorController;

    public AudioClip attackSFX;
}

public enum WeaponType {
    Sword,
    Bow,
    Shield
}

public enum UnitType {
    Warrior,
    Archer
}