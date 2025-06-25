using UnityEngine;

[CreateAssetMenu(menuName = "Actions/AttackAction")]
public class AttackAction : WeaponAction {
    public override void Execute(BasePlayerUnit unit) {
        unit.SetState(UnitState.Attacking);
        // Aqu� puedes agregar l�gica adicional si lo necesitas
        Debug.Log($"{unit.unitName} est� en modo ataque.");
        // Por ejemplo: CombatHandler.ShowAttackRange(unit);
    }
}
