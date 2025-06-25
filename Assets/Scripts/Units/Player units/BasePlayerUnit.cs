using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerUnit : BaseUnit
{
    public Transform actionIconsAnchor;
    public GameObject actionIconPrefab;
    private List<GameObject> currentActionIcons = new List<GameObject>();

    public void ShowActionIcons() {
        foreach (var icon in currentActionIcons)
            Destroy(icon);
        currentActionIcons.Clear();
        
        var allActions = new List<WeaponAction>();
        if (weapon1 != null) allActions.AddRange(weapon1.actions);
        if (weapon2 != null) allActions.AddRange(weapon2.actions);

        foreach (var action in allActions) {
            var iconGO = Instantiate(actionIconPrefab, actionIconsAnchor);
            var handler = iconGO.GetComponent<ActionIconClickHandler>();
            handler.Setup(action, this);
            currentActionIcons.Add(iconGO);
        }
    }
    public void HideActionIcons() {
        foreach (var icon in currentActionIcons)
            Destroy(icon);
        currentActionIcons.Clear();
    }

    public override void PerformAttack(BaseUnit target) {
        base.PerformAttack(target);
        HideActionIcons();
    }

}


