using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour {

    public UnitState CurrentState { get; private set; } = UnitState.Idle;

    public string unitName;
    public Tile occupiedTile;
    public Faction faction;

    public SFXWorldManager SFXWorldManager;

    [Header("Movement")]
    [SerializeField] private int maxMovementPoints = 50;
    public int currentMovementPoints;

    [Header("Combat")]
    [SerializeField] private int attackRange = 1;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private int maxHealth = 30;
    public int currentHealth;

    [Header("Weapon Anchors")]
    [SerializeField] private Transform weapon1AnchorPoint;
    [SerializeField] private Transform weapon2AnchorPoint;

    [Header("Tipo de unidad y armas permitidas")]
    public UnitType unitType;
    public List<WeaponType> allowedWeaponTypes;

    public BaseWeapon weapon1 { get; private set; }
    public BaseWeapon weapon2 { get; private set; }

    public bool IsInAttackState() => CurrentState == UnitState.Attacking;
    public int GetAttackRange() => attackRange;
    public int GetAttackDamage() => attackDamage;
    public int GetMaxHealth() => maxHealth;
    public int GetMaxMovementPoints() => maxMovementPoints;

    public bool hasAttackedThisTurn { get; private set; } = false;

    private void Awake() {
        currentMovementPoints = maxMovementPoints;
        currentHealth = maxHealth;

        SFXWorldManager = FindObjectOfType<SFXWorldManager>();
    }


    public virtual void EquipWeapon(BaseWeapon weaponPrefab, int slot = 1) {
        if (weaponPrefab == null) return;


        if (weaponPrefab.weaponData == null || allowedWeaponTypes == null ||
            !allowedWeaponTypes.Contains(weaponPrefab.weaponData.weaponType)) {
            return;
        }

        if (unitType == UnitType.Archer && weaponPrefab.weaponData.weaponType == WeaponType.Bow) {
            if ((slot == 1 && weapon2 != null && weapon2.weaponData.weaponType == WeaponType.Bow) ||
                (slot == 2 && weapon1 != null && weapon1.weaponData.weaponType == WeaponType.Bow)) {
                return;
            }
        }

        Transform anchor = slot == 2 ? weapon2AnchorPoint : weapon1AnchorPoint;
        if (anchor == null) {
            return;
        }

        RemoveWeapon(slot);

        BaseWeapon weaponInstance = Instantiate(weaponPrefab, anchor.position, anchor.rotation, anchor);
        if (slot == 2)
            weapon2 = weaponInstance;
        else
            weapon1 = weaponInstance;

    }


    public void RemoveWeapon(int slot = 1) {
        BaseWeapon weaponToRemove = slot == 2 ? weapon2 : weapon1;
        if (weaponToRemove != null) {
            Destroy(weaponToRemove.gameObject);
            if (slot == 2)
                weapon2 = null;
            else
                weapon1 = null;
        }
    }

    public void RemoveAllWeapons() {
        RemoveWeapon(1);
        RemoveWeapon(2);
    }

    public void SetState(UnitState newState) {
        CurrentState = newState;
    }

    public void ResetState() {
        CurrentState = UnitState.Idle;
    }

    public void ResetMovement() {
        currentMovementPoints = maxMovementPoints;
        hasAttackedThisTurn = false;
    }

    public void MarkAsAttacked() {
        hasAttackedThisTurn = true;
    }

    public bool CanAttack() {
        return !hasAttackedThisTurn;
    }

    public bool CanMoveTo(List<Tile> path) {
        int cost = 0;
        Tile previous = occupiedTile;
        foreach (var tile in path) {
            cost += Pathfinding.GetMoveCost(previous, tile);
            previous = tile;
        }
        return cost <= currentMovementPoints;
    }

    public void ConsumeMovement(int cost) {
        currentMovementPoints -= cost;
    }

    public void ReceiveDamage(int damage) {
        SFXWorldManager.PlaySFX("unitDamaged");
        currentHealth -= damage;
        Debug.Log($"{unitName} recibió {damage} de daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0) {
            Die();
        }
    }

    public virtual void PerformAttack(BaseUnit target) {
        if (weapon1 != null) {
            weapon1.Attack(this, target);
        } else {
            int damage = GetAttackDamage();
            target.ReceiveDamage(damage);
        }
        MarkAsAttacked();
        CombatHandler.ClearAttackTiles();
    }

    private void Die() {
        if (occupiedTile != null) {
            occupiedTile.unitOnTile = null;
        }
        if (this.faction == Faction.Player) {
            UnitWorldManager.Instance.playerUnits.Remove(this);
        } else if (this.faction == Faction.Enemy) {
            UnitWorldManager.Instance.enemyUnits.Remove(this);
        }
        RemoveAllWeapons();
        Destroy(gameObject);
    }

    public void ResetTurn() {
        currentMovementPoints = maxMovementPoints;
        hasAttackedThisTurn = false;
    }
}

public enum UnitState {
    Idle,
    Moving,
    Attacking
}

