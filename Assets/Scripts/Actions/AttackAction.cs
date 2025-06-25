using UnityEngine;

[CreateAssetMenu(menuName = "Actions/AttackAction")]
public class AttackAction : WeaponAction {
    public override void Execute(BasePlayerUnit unit) {
        unit.SetState(UnitState.Attacking);
        var selectedUnit = UnitWorldManager.Instance.selectedPlayerUnit;
        CombatHandler.ClearAttackTiles();
        GridWorldManager.Instance.ClearReachableTiles();
        GridWorldManager.Instance.ClearSelectedDestinationPathTiles();
        CombatHandler.ShowAttackRange(selectedUnit);
        unit.HideActionIcons();
    }
}
