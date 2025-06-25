using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoUIWorldManager : MonoBehaviour
{
    public static InfoUIWorldManager Instance { get; private set; }


    [SerializeField] private GameObject selectedPlayerUnitPanel;
    [SerializeField] private GameObject selectedTileInfoPanel;
    [SerializeField] private GameObject selectedTileUnitInfoPanel;

    // Referencias a las barras y textos
    [Header("Referencias UI de Unidad Seleccionada")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthBarText;
    [SerializeField] private Image movementBar;
    [SerializeField] private TextMeshProUGUI movementBarText;
    [SerializeField] private TextMeshProUGUI unitNameText;

    [Header("Panel dinámico de unidades del jugador")]
    [SerializeField] private GameObject playerUnitsGridPanel;
    [SerializeField] private GameObject playerUnitPanelPrefab;

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

    public void SkipTurn() {
        if (GameManager.Instance.GameState == GameState.PlayerTurn) {
            TurnWorldManager.Instance.EndPlayerTurn();
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
        if (playerUnit == null) {
            selectedPlayerUnitPanel.SetActive(false);
            return;
        }

        // Nombre de la unidad
        if (unitNameText != null)
            unitNameText.text = playerUnit.unitName;

        // Vida
        if (healthBar != null && healthBarText != null) {
            int maxHealth = playerUnit.GetComponent<BaseUnit>().GetMaxHealth();
            int currentHealth = playerUnit.currentHealth;
            healthBar.fillAmount = (float)currentHealth / maxHealth;
            healthBarText.text = $"{currentHealth}/{maxHealth}";
        }

        // Movimiento
        if (movementBar != null && movementBarText != null) {
            int maxMovement = playerUnit.GetComponent<BaseUnit>().GetMaxMovementPoints();
            int currentMovement = playerUnit.currentMovementPoints;
            movementBar.fillAmount = (float)currentMovement / maxMovement;
            movementBarText.text = $"{currentMovement}/{maxMovement}";
        }

        selectedPlayerUnitPanel.SetActive(true);
    }

    //public void UpdatePlayerUnitsGrid(List<BaseUnit> playerUnits, BaseUnit selectedUnit) {
    //    foreach (Transform child in playerUnitsGridPanel.transform) {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (var unit in playerUnits) {
    //        if (unit == selectedUnit)
    //            continue;

    //        GameObject panel = Instantiate(playerUnitPanelPrefab, playerUnitsGridPanel.transform);
    //        var nameText = panel.transform.Find("SelectedPlayerUnit Name").GetComponent<TextMeshProUGUI>();
    //        var healthBarImage = panel.transform.Find("HealthBar Fill").GetComponent<Image>();
    //        var healthBarText = panel.transform.Find("HealthBarText").GetComponent<TextMeshProUGUI>();
    //        var movementBarImage = panel.transform.Find("MovementBar Fill").GetComponent<Image>();
    //        var movementBarText = panel.transform.Find("MovementBarText").GetComponent<TextMeshProUGUI>();
    //        var unitImage = panel.transform.Find("UnitImage").GetComponent<Image>();

    //        nameText.text = unit.unitName;

    //        int maxHealth = unit.GetMaxHealth();
    //        int currentHealth = unit.currentHealth;
    //        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    //        healthBarText.text = $"{currentHealth}/{maxHealth}";

    //        int maxMovement = unit.GetMaxMovementPoints();
    //        int currentMovement = unit.currentMovementPoints;
    //        movementBarImage.fillAmount = (float)currentMovement / maxMovement;
    //        movementBarText.text = $"{currentMovement}/{maxMovement}";

    //    }
    //}
}
