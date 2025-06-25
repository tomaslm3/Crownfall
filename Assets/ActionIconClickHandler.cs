using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionIconClickHandler : MonoBehaviour, IPointerClickHandler {
    public WeaponAction action;
    private BasePlayerUnit owner;

    public void Setup(WeaponAction action, BasePlayerUnit owner) {
        this.action = action;
        this.owner = owner;
        var image = GetComponent<Image>();
        if (image != null)
            image.sprite = action.icon;
        else
            Debug.LogError("El prefab de acción no tiene un componente Image en el mismo GameObject.");
    }

    public void OnPointerClick(PointerEventData eventData) {
        action.Execute(owner);
    }
}