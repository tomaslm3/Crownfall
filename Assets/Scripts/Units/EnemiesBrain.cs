using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesBrain : MonoBehaviour {

    // Deberia simular la IA enemiga

    // En esta clase solo vamos a tener logica de la IA enemiga, acciones que llamen a los metodos de BaseUnit (A travez de su correspondiente unidad)
    // La Ia deberia permitirle a la unidad, analizar cuanto movimiento tiene restante, si puede atacar, cual es su rango de ataque
    // Que tan lejos esta una unidad enemiga.

    public void EnemyAIHandler(BaseUnit unit) {
        Debug.Log($"Enemy AI Handler for unit: {unit.name}");
        var closestPlayerUnit = FindClosestPlayerUnit(unit);

        if (closestPlayerUnit == null) {
            Debug.LogWarning("No player units found for enemy AI to target.");
            return;
        }

        var enemyTile = unit.occupiedTile;
        var playerTile = closestPlayerUnit.occupiedTile;

        // 1. Calcular la distancia en tiles 
        float distancia = enemyTile.GetDistance(playerTile);
        int rangoAtaque = unit.weapon1 != null ? unit.weapon1.AttackRange : unit.GetAttackRange();



        //// 2. Si está en rango de ataque, atacar
        if (distancia <= rangoAtaque * 14) {
            if (unit.CanAttack()) {
                Debug.Log($"{unit.name} ataca a {closestPlayerUnit.name}");
                CombatHandler.ResolveAttack(unit, closestPlayerUnit);
            }
            return;
        }


        // 3. Si no está en rango, intentar moverse hacia la unidad del jugador
        var path = Pathfinding.FindPath(enemyTile, playerTile, unit);

        if (path == null || path.Count == 0) {
            // No hay camino directo, buscar el tile alcanzable más cercano al objetivo
            var reachableTiles = Pathfinding.GetReachableTiles(enemyTile, unit.currentMovementPoints);
            if (reachableTiles == null || reachableTiles.Count == 0) {
                Debug.Log($"{unit.name} no puede moverse este turno.");
                return;
            }

            // Elegir el tile alcanzable más cercano al jugador
            Tile mejorTile = reachableTiles
                .OrderBy(t => t.GetDistance(playerTile))
                .First();

            // Obtener el path hasta ese tile
            var pathParcial = Pathfinding.FindPath(enemyTile, mejorTile, unit);
            if (pathParcial != null && pathParcial.Count > 0) {
                mejorTile.ConfirmMovementEnemyMovement(unit, pathParcial);
            }
        } else {
            // Hay camino directo, recortar el path para quedarse fuera del rango de ataque
            int tilesAntesDelObjetivo = Mathf.Clamp(rangoAtaque, 1, path.Count - 1);
            int indiceObjetivo = path.Count - tilesAntesDelObjetivo - 1;
            if (indiceObjetivo < 0) indiceObjetivo = 0;

            Tile objectiveTile = path[indiceObjetivo];
            List<Tile> pathRecortado = path.Take(indiceObjetivo + 1).ToList();

            objectiveTile.ConfirmMovementEnemyMovement(unit, pathRecortado);
        }

        // 4. Volver a comprobar si está en rango de ataque después de moverse
        if (unit.occupiedTile.GetDistance(playerTile) <= rangoAtaque * 14 && unit.CanAttack()) {
            CombatHandler.ResolveAttack(unit, closestPlayerUnit);
        }

    }

    private static BaseUnit FindClosestPlayerUnit(BaseUnit unit) {
        var playerUnits = UnitWorldManager.Instance.playerUnits;

        List<(BaseUnit playerUnit, float distancia)> unidadesConDistancia = new List<(BaseUnit, float)>();

        Vector2 posicionEnemigo = unit.occupiedTile.Coords.Pos;
        foreach (var playerUnit in playerUnits) {
            if (playerUnit.occupiedTile != null) {
                Vector2 posicionJugador = playerUnit.occupiedTile.Coords.Pos;
                float distancia = Vector2.Distance(posicionEnemigo, posicionJugador);
                unidadesConDistancia.Add((playerUnit, distancia));
            }
        }

        unidadesConDistancia.Sort((a, b) => a.distancia.CompareTo(b.distancia));

        return unidadesConDistancia.Count > 0 ? unidadesConDistancia[0].playerUnit : null;
    }
}