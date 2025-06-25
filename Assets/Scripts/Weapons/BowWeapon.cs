using Unity.VisualScripting;
using UnityEngine;

public class BowWeapon : BaseWeapon {
    [Header("Prefab de flecha")]
    [SerializeField] private ArrowProjectile arrowPrefab;

    public override void Attack(BaseUnit attacker, BaseUnit target) {
        base.Attack(attacker, target);

    }

    /// <summary>
    /// Llamado por Animation Event en el frame correcto.
    /// </summary>
    public void ShootArrow() {
        var arrow = Instantiate(arrowPrefab);
        SFXWorldManager.PlaySFX(weaponData.attackSFX);
        arrow.Init(transform.position, pendingTarget, AttackDamage);
    }
}