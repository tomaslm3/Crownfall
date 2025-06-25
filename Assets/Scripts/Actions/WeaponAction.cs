using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponAction", menuName = "Actions/Weapon Action")]
public class WeaponAction : ScriptableObject
{
    public string actionName;
    public Sprite icon;
    public virtual void Execute(BasePlayerUnit unit) { }
}
