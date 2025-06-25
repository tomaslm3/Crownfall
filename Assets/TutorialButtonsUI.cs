using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonsUI : MonoBehaviour {
    public BasePlayerUnit playerWarriorPrefab;
    public BasePlayerUnit playerArcherPrefab;
    public BaseEnemyUnit enemyWarriorPrefab;
    public BaseEnemyUnit enemyArcherPrefab;

    [SerializeField] private BaseWeapon swordWeaponPrefab;
    [SerializeField] private BaseWeapon shieldWeaponPrefab;
    [SerializeField] private BaseWeapon bowWeaponPrefab;

    public void AddWarriorUnit() {
        var spawnedUnit = Instantiate(playerWarriorPrefab);
        var spawnTile = GridWorldManager.Instance.GetPlayerSpawnTile();
        UnitWorldManager.Instance.playerUnits.Add(spawnedUnit);
        spawnTile.UnitSpawn(spawnedUnit);

        if (spawnedUnit.unitType == UnitType.Warrior) {
            if (swordWeaponPrefab != null)
                spawnedUnit.EquipWeapon(swordWeaponPrefab, 1);
            if (shieldWeaponPrefab != null)
                spawnedUnit.EquipWeapon(shieldWeaponPrefab, 2);
        } else if (spawnedUnit.unitType == UnitType.Archer) {
            if (bowWeaponPrefab != null)
                spawnedUnit.EquipWeapon(bowWeaponPrefab, 1);
        }
    }

    public void AddArcherUnit() {
        var spawnedUnit = Instantiate(playerArcherPrefab);
        var spawnTile = GridWorldManager.Instance.GetPlayerSpawnTile();
        UnitWorldManager.Instance.playerUnits.Add(spawnedUnit);
        spawnTile.UnitSpawn(spawnedUnit);

        if (spawnedUnit.unitType == UnitType.Warrior) {
            if (swordWeaponPrefab != null)
                spawnedUnit.EquipWeapon(swordWeaponPrefab, 1);
            if (shieldWeaponPrefab != null)
                spawnedUnit.EquipWeapon(shieldWeaponPrefab, 2);
        } else if (spawnedUnit.unitType == UnitType.Archer) {
            if (bowWeaponPrefab != null)
                spawnedUnit.EquipWeapon(bowWeaponPrefab, 1);
        }
    }

    public void AddEnemyArcherUnit() {
        var spawnedUnit = Instantiate(enemyArcherPrefab);
        var spawnTile = GridWorldManager.Instance.GetEnemySpawnTile();
        UnitWorldManager.Instance.enemyUnits.Add(spawnedUnit);
        spawnTile.UnitSpawn(spawnedUnit);

        if (spawnedUnit.unitType == UnitType.Warrior) {
            if (swordWeaponPrefab != null)
                spawnedUnit.EquipWeapon(swordWeaponPrefab, 1);
            if (shieldWeaponPrefab != null)
                spawnedUnit.EquipWeapon(shieldWeaponPrefab, 2);
        } else if (spawnedUnit.unitType == UnitType.Archer) {
            if (bowWeaponPrefab != null)
                spawnedUnit.EquipWeapon(bowWeaponPrefab, 1);
        }
    }

    public void AddEnemyWarriorUnit() {
        var spawnedUnit = Instantiate(enemyWarriorPrefab);
        var spawnTile = GridWorldManager.Instance.GetEnemySpawnTile();
        UnitWorldManager.Instance.enemyUnits.Add(spawnedUnit);
        spawnTile.UnitSpawn(spawnedUnit);

        if (spawnedUnit.unitType == UnitType.Warrior) {
            if (swordWeaponPrefab != null)
                spawnedUnit.EquipWeapon(swordWeaponPrefab, 1);
            if (shieldWeaponPrefab != null)
                spawnedUnit.EquipWeapon(shieldWeaponPrefab, 2);
        } else if (spawnedUnit.unitType == UnitType.Archer) {
            if (bowWeaponPrefab != null)
                spawnedUnit.EquipWeapon(bowWeaponPrefab, 1);
        }
    }
}