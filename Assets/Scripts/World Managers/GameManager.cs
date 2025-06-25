using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState GameState;

    public BasePlayerUnit playerWarriorPrefab;
    public BasePlayerUnit playerArcherPrefab;
    public BaseEnemyUnit enemyWarriorPrefab;
    public BaseEnemyUnit enemyArcherPrefab;

    List<BasePlayerUnit> playerTroops;
    List<BaseEnemyUnit> enemyTroops;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {

        playerTroops = new List<BasePlayerUnit> {
            playerWarriorPrefab,
            playerWarriorPrefab,
            playerArcherPrefab
        };

        enemyTroops = new List<BaseEnemyUnit> {
            enemyWarriorPrefab,
            enemyWarriorPrefab,
            enemyArcherPrefab
        };

        ChangeState(GameState.GenerateGrid);
    }

    private void Update() {

        if ((GameState == GameState.PlayerTurn || GameState == GameState.EnemyTurn) && UnitWorldManager.Instance.playerUnits.Count == 0) {
            Debug.Log("Derrota!");
            GameManager.Instance.ChangeState(GameState.GameOver_Defeat);
        }
        if ((GameState == GameState.PlayerTurn || GameState == GameState.EnemyTurn) && UnitWorldManager.Instance.enemyUnits.Count == 0) {
            Debug.Log("Victoria!");
            GameManager.Instance.ChangeState(GameState.GameOver_Victory);
        }

        if (TurnWorldManager.Instance.IsPlayerTurn) {

            if (Input.GetKeyDown(KeyCode.E) && GameState == GameState.PlayerTurn) {
                if (GameState == GameState.PlayerTurn) {
                    TurnWorldManager.Instance.EndPlayerTurn();
                }
            }

            if (UnitWorldManager.Instance.selectedPlayerUnit != null) {


                if (Input.GetKeyDown(KeyCode.A) && UnitWorldManager.Instance.selectedPlayerUnit != null) {
                    var selectedUnit = UnitWorldManager.Instance.selectedPlayerUnit;
                    selectedUnit.SetState(UnitState.Attacking);
                    CombatHandler.ClearAttackTiles();
                    GridWorldManager.Instance.ClearReachableTiles();
                    GridWorldManager.Instance.ClearSelectedDestinationPathTiles();
                    CombatHandler.ShowAttackRange(selectedUnit);
                }

                if(UnitWorldManager.Instance.selectedPlayerUnit.CurrentState == UnitState.Idle) {
                        if (Input.GetKeyDown(KeyCode.Space)) {
                            if (GridWorldManager.Instance.selectedDestinationPath != null) {
                                foreach (var tile in GridWorldManager.Instance.selectedDestinationPath) {
                                    tile.RevertTile();
                                }
                                GridWorldManager.Instance.selectedDestinationPath.Last().ConfirmMovement(UnitWorldManager.Instance.selectedPlayerUnit, GridWorldManager.Instance.selectedDestinationPath);

                            }
                        }
                }

                if (Input.GetMouseButtonDown(1)) {
                    if (GameState == GameState.PlayerTurn) {
                        if(UnitWorldManager.Instance.selectedPlayerUnit.CurrentState == UnitState.Attacking) {
                            UnitWorldManager.Instance.selectedPlayerUnit.ResetState();
                            CombatHandler.ClearAttackTiles();
                        }
                        GridWorldManager.Instance.DeselectUnit();
                    }
                }
            }
        }

        if (!TurnWorldManager.Instance.IsPlayerTurn) {
            TurnWorldManager.Instance.HandleEnemyTurn();
        }
    }

    public void ChangeState(GameState newGameState) {
        string sceneName;
        GameState = newGameState;
        switch (GameState) {
            case GameState.GenerateGrid:
                Debug.Log("Generating Grid");
                GridWorldManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnPlayerTroops:
                Debug.Log("Spawning Player Troops");
                UnitWorldManager.Instance.SpawnPlayerTroops(playerTroops);
                break;
            case GameState.SpawnEnemyTroops:
                Debug.Log("Spawning Enemy Troops");
                UnitWorldManager.Instance.SpawnEnemyTroops(enemyTroops);
                break;
            case GameState.PlayerTurn:
                Debug.Log("Player Turn Begins");
                UnitWorldManager.Instance.ResetPlayerUnits();
                break;
            case GameState.EnemyTurn:
                Debug.Log("Enemy Turn Begins");
                UnitWorldManager.Instance.ResetEnemyUnits();
                break;
            case GameState.GameOver_Defeat:
                sceneName = "Defeat_Scene_01";
                HandleGameOver(sceneName);
                break;
            case GameState.GameOver_Victory:
                sceneName = "Victory_Scene_01";
                HandleGameOver(sceneName);
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }
    }

    public void HandleGameOver(string sceneName) {
        StartCoroutine(WaitAndChangeScene(sceneName));
    }

    private IEnumerator WaitAndChangeScene(string sceneName) {

        Debug.Log("Game Over...");
        yield return new WaitForSeconds(3f);


        SceneManager.LoadScene(sceneName);
    }


    //public void HandleGameOver() {
    //    StartCoroutine(WaitAndRestartGame());
    //}

    //private IEnumerator WaitAndRestartGame() {

    //    Debug.Log("Game Over. Reiniciando juego en 3 segundos...");
    //    yield return new WaitForSeconds(3f);

    //    foreach (var unit in UnitWorldManager.Instance.playerUnits) {
    //        Destroy(unit.gameObject);
    //    }
    //    UnitWorldManager.Instance.playerUnits.Clear();

    //    foreach (var unit in UnitWorldManager.Instance.enemyUnits) {
    //        Destroy(unit.gameObject);
    //    }
    //    UnitWorldManager.Instance.enemyUnits.Clear();

    //    ChangeState(GameState.GenerateGrid);
    //}
}


public enum GameState {
    GenerateGrid,
    SpawnPlayerTroops,
    SpawnEnemyTroops,
    PlayerTurn,
    EnemyTurn,
    GameOver,
    GameOver_Defeat,
    GameOver_Victory
}