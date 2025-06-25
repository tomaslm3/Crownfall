using UnityEngine;

[CreateAssetMenu(menuName = "Actions/DefenseAction")]
public class DefenseAction : WeaponAction {
    public int healAmount = 10;

    public override void Execute(BasePlayerUnit unit) {
        int maxHealth = unit.GetMaxHealth();
        unit.currentHealth = Mathf.Min(unit.currentHealth + healAmount, maxHealth);
        Debug.Log($"{unit.unitName} se curó {healAmount} puntos de vida. Vida actual: {unit.currentHealth}/{maxHealth}");
    }
}
