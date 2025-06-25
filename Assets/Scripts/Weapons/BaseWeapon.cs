using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour {
    [Header("Datos del arma")]
    public WeaponData weaponData;

    [Header("Animación")]
    [SerializeField] protected Animator weaponAnimator;

    private int currentDurability;

    public BaseUnit pendingTarget;

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
            weaponAnimator.SetTrigger("Attack"); // Cambiado a Trigger

        pendingTarget = target;
        currentDurability--;
    }

    /// <summary>
    /// Llamado por Animation Event en el frame correcto.
    /// </summary>
    public void ApplyDamage() {
        if (pendingTarget != null) {
            pendingTarget.ReceiveDamage(AttackDamage);
            pendingTarget = null;
        }
    }


}
