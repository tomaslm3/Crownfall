using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUIWorldManager : MonoBehaviour
{
    public static InfoUIWorldManager Instance { get; private set; }


    [SerializeField] private GameObject selectedPlayerUnitPanel;
    [SerializeField] private GameObject selectedTileInfoPanel;
    [SerializeField] private GameObject selectedTileUnitInfoPanel;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        var selected = UnitWorldManager.Instance.selectedPlayerUnit;

        if (selected != null && selectedPlayerUnitPanel.activeSelf) {
            ShowSelectedPLayerUnitPanel(selected);
        }
    }

    //public void ShowSelectedTileInfoPanel(Tile tile) {
    //    TextMeshProUGUI[] textComponents = selectedTileInfoPanel.GetComponentsInChildren<TextMeshProUGUI>();
    //    TextMeshProUGUI[] textComponentsUnitOnTile = selectedTileUnitInfoPanel.GetComponentsInChildren<TextMeshProUGUI>();
    //    if (tile == null) {
    //        selectedTileInfoPanel.SetActive(false);
    //        selectedTileUnitInfoPanel.SetActive(false);
    //        return;
    //    }
    //    if (textComponents.Length > 0) {
    //        textComponents[0].text = tile.tileName;
    //    }

    //    if (tile.unitOnTile != null) {
    //        selectedTileUnitInfoPanel.SetActive(true);
    //        if (textComponentsUnitOnTile.Length > 0) {
    //            textComponentsUnitOnTile[0].text = tile.unitOnTile.unitName;
    //        }
    //    } else {
    //        selectedTileUnitInfoPanel.SetActive(false);
    //    }

    //    selectedTileInfoPanel.SetActive(true);
    //}

    public void ShowSelectedPLayerUnitPanel(BasePlayerUnit playerUnit) {
        TextMeshProUGUI[] textComponents = selectedPlayerUnitPanel.GetComponentsInChildren<TextMeshProUGUI>();

        if (playerUnit == null) {
            selectedPlayerUnitPanel.SetActive(false);
            return;
        }

        if (textComponents.Length > 0) {
            textComponents[0].text = playerUnit.unitName;
            textComponents[1].text = "Movimiento restante: " + playerUnit.currentMovementPoints.ToString();
            textComponents[2].text = "Vida: " + playerUnit.currentHealth.ToString();
            textComponents[3].text = "Puede atacar: " + playerUnit.CanAttack();
        }
        selectedPlayerUnitPanel.SetActive(true);
    }
}
