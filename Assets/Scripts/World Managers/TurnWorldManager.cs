using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWorldManager : MonoBehaviour {
    public static TurnWorldManager Instance { get; private set; }

    public bool IsPlayerTurn => GameManager.Instance.GameState == GameState.PlayerTurn;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void EndPlayerTurn() {
        GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void HandleEnemyTurn() {
        if (GameManager.Instance.GameState == GameState.EnemyTurn) {
            Debug.Log("Enemies Turn Begins");

            foreach (BaseUnit unit in UnitWorldManager.Instance.enemyUnits) {
                if (unit == null) {
                    Debug.LogWarning("Se encontró una unidad enemiga null en la lista.");
                    continue;
                }
                unit.GetComponent<EnemiesBrain>().EnemyAIHandler(unit);
            }

            Debug.Log("Enemies Turn end");
            GameManager.Instance.ChangeState(GameState.PlayerTurn);
        }
    }
}
