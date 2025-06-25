using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitWorldManager : MonoBehaviour
{
    public static UnitWorldManager Instance { get; private set; }

    private List<ScriptableUnit> units;

    [SerializeField] private BaseWeapon swordWeaponPrefab;
    [SerializeField] private BaseWeapon shieldWeaponPrefab;
    [SerializeField] private BaseWeapon bowWeaponPrefab;

    public List<BaseUnit> playerUnits = new();
    public List<BaseUnit> enemyUnits = new();

    public BasePlayerUnit selectedPlayerUnit;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }


    public void SpawnPlayerTroops(List<BasePlayerUnit> unitsToSpawn) {
        playerUnits.Clear();

        foreach (var unitPrefab in unitsToSpawn) {
            var spawnedUnit = Instantiate(unitPrefab);
            var randomSpawnTile = GridWorldManager.Instance.GetPlayerSpawnTile();
            playerUnits.Add(spawnedUnit);

            randomSpawnTile.UnitSpawn(spawnedUnit);

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

        GameManager.Instance.ChangeState(GameState.SpawnEnemyTroops);
    }


    public void SpawnEnemyTroops(List<BaseEnemyUnit> unitsToSpawn) {
        enemyUnits.Clear();

        foreach (var unitPrefab in unitsToSpawn) {
            var spawnedUnit = Instantiate(unitPrefab);
            var randomSpawnTile = GridWorldManager.Instance.GetEnemySpawnTile();
            enemyUnits.Add(spawnedUnit);

            randomSpawnTile.UnitSpawn(spawnedUnit);

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

        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

    /* Metodo para Debug encargado de seleccionar una unidad aleatoria
     * Dependiendo la faccion.
     */
    //private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
    //    return (T)units.Where(units => units.faction == faction)
    //        .OrderBy(x => Random.value)
    //        .First().unitPrefab;
    //}

    public void SetSelectedPlayerUnit(BasePlayerUnit unit) {
        if (unit != null)
            unit.ShowActionIcons();
        selectedPlayerUnit = unit;
        InfoUIWorldManager.Instance.ShowSelectedPLayerUnitPanel(unit);
    }

    public void ResetPlayerUnits() {
        foreach (var unit in playerUnits) {
            unit.ResetTurn();
        }
    }

    public void ResetEnemyUnits() {
        foreach (var unit in enemyUnits) {
            unit.ResetTurn();
        }
    }
}
