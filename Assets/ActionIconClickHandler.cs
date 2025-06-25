using UnityEngine;
using UnityEngine.EventSystems;

public class ActionIconClickHandler : MonoBehaviour, IPointerClickHandler {
    public WeaponAction action;
    private BasePlayerUnit owner;

    public void Setup(WeaponAction action, BasePlayerUnit owner) {
        this.action = action;
        this.owner = owner;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sprite = action.icon;
        else
            Debug.LogError("El prefab de acción no tiene un componente SpriteRenderer en el mismo GameObject.");
    }

    public void OnPointerClick(PointerEventData eventData) {
        action.Execute(owner);
    }
}