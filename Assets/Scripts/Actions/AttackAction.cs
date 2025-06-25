using UnityEngine;

[CreateAssetMenu(menuName = "Actions/AttackAction")]
public class AttackAction : WeaponAction {
    public override void Execute(BasePlayerUnit unit) {
        unit.SetState(UnitState.Attacking);
        // Aquí puedes agregar lógica adicional si lo necesitas
        Debug.Log($"{unit.unitName} está en modo ataque.");
        // Por ejemplo: CombatHandler.ShowAttackRange(unit);
    }
}
