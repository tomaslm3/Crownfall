using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour {
    [Header("Datos del arma")]
    public WeaponData weaponData;

    [Header("Animación")]
    [SerializeField] protected Animator weaponAnimator;

    [Header("Acciones del arma")]
    public List<WeaponAction> actions = new List<WeaponAction>();

    private int currentDurability;

    public BaseUnit pendingTarget;

    public SFXWorldManager SFXWorldManager;

    public int AttackRange => weaponData != null ? weaponData.attackRange : 1;
    public int AttackDamage => weaponData != null ? weaponData.attackDamage : 10;
    public int Durability => currentDurability;

    protected virtual void Awake() {
        if (weaponData != null)
            currentDurability = weaponData.durability;
        else
            currentDurability = 10;
        if (weaponAnimator != null && weaponData != null && weaponData.animatorController != null)
            weaponAnimator.runtimeAnimatorController = weaponData.animatorController;

        SFXWorldManager = FindObjectOfType<SFXWorldManager>();
    }

    /// <summary>
    /// Inicia el ataque, dispara la animación y guarda el objetivo.
    /// El daño se aplicará mediante Animation Event.
    /// </summary>
    public virtual void Attack(BaseUnit attacker, BaseUnit target) {
        if (currentDurability <= 0) {
            Debug.LogWarning($"{name}: El arma está rota y no puede atacar.");
            return;
        }

        if (weaponAnimator != null)
            weaponAnimator.SetTrigger("Attack");

        pendingTarget = target;
        currentDurability--;
    }

    /// <summary>
    /// Llamado por Animation Event en el frame correcto.
    /// </summary>
    public void ApplyDamage() {
        if (pendingTarget != null) {
            SFXWorldManager.PlaySFX(weaponData.attackSFX);
            pendingTarget.ReceiveDamage(AttackDamage);
            pendingTarget = null;
        }
    }


}
