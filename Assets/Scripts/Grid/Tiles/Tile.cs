using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public string tileName;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject highlightColor;
    [SerializeField] private GameObject reachableIndicator;
    [SerializeField] public GameObject walkeableIndicator;
    [SerializeField] private GameObject attackableIndicator;
    [SerializeField] private bool isWalkable;

    [SerializeField] private GameObject variant1;
    [SerializeField] private GameObject variant2;
    [SerializeField] private GameObject variant3;

    public BaseUnit unitOnTile;
    public ICoords Coords;
    private bool selected;

    private GameObject activeVariant;


    public float GetDistance(Tile other) => Coords.GetDistance(other.Coords);


    public virtual void Init(bool walkable, ICoords coords) {
        isWalkable = walkable;
        Coords = coords;
        transform.position = Coords.Pos;

        int variantIndex = (int)((coords.Pos.x + coords.Pos.y) % 3);
        SetVariant(variantIndex);
    }

    private void SetVariant(int index) {
        variant1.SetActive(false);
        variant2.SetActive(false);
        variant3.SetActive(false);

        switch (index) {
            case 0:
                variant1.SetActive(true);
                activeVariant = variant1;
                break;
            case 1:
                variant2.SetActive(true);
                activeVariant = variant2;
                break;
            case 2:
                variant3.SetActive(true);
                activeVariant = variant3;
                break;
        }
    }

    private void OnMouseEnter() {
        highlightColor.SetActive(true);
        //MenuManager.Instance.ShowSelectedTileInfoPanel(this);
    }

    private void OnMouseExit() {
        highlightColor.SetActive(false);
        //MenuManager.Instance.ShowSelectedTileInfoPanel(null);
    }

    public static event Action<Tile> OnTileClicked;

    private void OnEnable() {
        OnTileClicked += OnTileClickedHandler;
    }
    private void OnDisable() {
        OnTileClicked -= OnTileClickedHandler;
    }

    private void OnTileClickedHandler(Tile selectedTile) => selected = selectedTile == this;


    protected virtual void OnMouseDown() {
        if(GameManager.Instance.GameState != GameState.PlayerTurn || !isWalkable) return;
        if (unitOnTile != null) {
            if(unitOnTile.faction == Faction.Player) {
                // Unidad del jugador
                GridWorldManager.Instance.DeselectUnit(UnitWorldManager.Instance.selectedPlayerUnit);
                UnitWorldManager.Instance.SetSelectedPlayerUnit((BasePlayerUnit)unitOnTile);
                GridWorldManager.Instance.ShowReachableTiles((BasePlayerUnit)unitOnTile);
                SFXWorldManager.Instance.PlaySFX("unitSelected");
            } else {
                // Unidad enemiga con Unidad del jugador ya seleccionada
                if (UnitWorldManager.Instance.selectedPlayerUnit != null) {
                    if (CombatHandler.currentAttackableTiles.Contains(this)) {
                        if (this.unitOnTile != null && this.unitOnTile.faction != UnitWorldManager.Instance.selectedPlayerUnit.faction) {
                            // Atacar unidad enemiga
                            CombatHandler.ResolveAttack(UnitWorldManager.Instance.selectedPlayerUnit, this.unitOnTile);
                        }
                    }
                    return;
                } else {
                    // Unidad enemiga sin Unidad del jugador seleccionada
                }
            }
        } else {
            // No hay unidad en la casilla
            if (UnitWorldManager.Instance.selectedPlayerUnit != null && UnitWorldManager.Instance.selectedPlayerUnit.CurrentState != UnitState.Attacking) {
                // Casilla vacia con Unidad del jugador seleccionada
                OnTileClicked?.Invoke(this);
            } else {
                // Casilla vacia sin Unidad del jugador seleccionada
            }
        }
    }


    public void UnitSpawn(BaseUnit unit) {
        unit.transform.position = transform.position;
        unitOnTile = unit;
        unit.occupiedTile = this;

    }
    public void ConfirmMovement(BaseUnit unit, List<Tile> path) {
        if (unit == null || unitOnTile != null) return;

        if (unit.occupiedTile != null) {
            unit.occupiedTile.unitOnTile = null;
        }

        unit.transform.position = transform.position;
        unitOnTile = unit;
        unit.occupiedTile = this;

        int totalCost = 0;
        Tile previous = path.Count > 0 ? path[0].Connections : null;
        foreach (var tile in path) {
            if (previous != null)
                totalCost += Pathfinding.GetMoveCost(previous, tile);
            previous = tile;
        }

        unit.ConsumeMovement(totalCost);

        GridWorldManager.Instance.ClearReachableTiles();

        if (unit.currentMovementPoints > 0) {
            GridWorldManager.Instance.ShowReachableTiles(unit);
        }
    }

    public void ConfirmMovementEnemyMovement(BaseUnit unit, List<Tile> path) {
        if (unit == null || unitOnTile != null) return;

        if (unit.occupiedTile != null) {
            unit.occupiedTile.unitOnTile = null;
        }

        unit.transform.position = transform.position;
        unitOnTile = unit;
        unit.occupiedTile = this;

        int totalCost = 0;
        Tile previous = path.Count > 0 ? path[0].Connections : null;
        foreach (var tile in path) {
            if (previous != null)
                totalCost += Pathfinding.GetMoveCost(previous, tile);
            previous = tile;
        }

        unit.ConsumeMovement(totalCost);
    }

    public bool IsWalkable() {
        return isWalkable;
    }


    #region Pathfinding
    public List<Tile> Neightbors { get; protected set; }
    public Tile Connections { get; protected set; }
    public float GCost { get; private set; }
    public float HCost { get; private set; }
    public float FCost => GCost + HCost;

    private static readonly List<Vector2> Directions = new List<Vector2> {
        new Vector2(0, 1), // Up
        new Vector2(1, 0), // Right
        new Vector2(0, -1), // Down
        new Vector2(-1, 0), // Left
        new Vector2(1, 1), // Up-Right
        new Vector2(1, -1), // Down-Right
        new Vector2(-1, 1), // Up-Left
        new Vector2(-1, -1) // Down-Left
    };

    public void CacheNeighbors() {
        Neightbors = new List<Tile>();

        foreach (var tile in Directions.Select(direction => GridWorldManager.Instance.GetTileAtPosition(Coords.Pos + direction)).Where(tile => tile != null)) {
            Neightbors.Add(tile);
        }
    }

    public void SetConnections(Tile tile) {
        Connections = tile;
    }

    public void SetGCost(float gCost) {
        GCost = gCost;
    }
    public void SetHCost(float hCost) {
        HCost = hCost;
    }

    public void SetReachable() {
        reachableIndicator.SetActive(true);
    }

    public void SetAttackable() {
        attackableIndicator.SetActive(true);
    }


    public void RevertTile() {
            walkeableIndicator.SetActive(false);
            reachableIndicator.SetActive(false);
            attackableIndicator.SetActive(false);
    }
    #endregion
}

public struct SquareCoords : ICoords {
    public float GetDistance(ICoords other) {
        var dist = new Vector2Int(Mathf.Abs((int)Pos.x - (int)other.Pos.x), Mathf.Abs((int)Pos.y - (int)other.Pos.y));

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }
    public Vector2 Pos { get; set; }
}

public interface ICoords {
    public float GetDistance(ICoords other);
    public Vector2 Pos { get; set; }
}