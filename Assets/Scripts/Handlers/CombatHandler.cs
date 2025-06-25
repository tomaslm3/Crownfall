using System.Collections.Generic;
using UnityEngine;

public static class CombatHandler {
    public static List<Tile> currentAttackableTiles = new();

    public static List<Tile> GetAttackableTiles(Tile origin, int range) {
        var reachable = new List<Tile>();
        var visited = new HashSet<Tile>();
        var queue = new Queue<(Tile tile, int distance)>();

        queue.Enqueue((origin, 0));
        visited.Add(origin);

        while (queue.Count > 0) {
            var (currentTile, currentDistance) = queue.Dequeue();

            if (currentTile != origin && currentDistance <= range)
                reachable.Add(currentTile);

            if (currentDistance >= range)
                continue;

            foreach (var neighbor in currentTile.Neightbors) {
                if (!visited.Contains(neighbor)) {
                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, currentDistance + 1));
                }
            }
        }

        return reachable;
    }

    public static List<Tile> ShowAttackRange(BaseUnit unit) {
        var origin = unit.occupiedTile;
        int range = unit.weapon1 != null ? unit.weapon1.AttackRange : unit.GetAttackRange();
        Debug.Log($"Unit {unit.unitName} attack range: {range}");
        currentAttackableTiles = GetAttackableTiles(origin, range);

        foreach (var tile in currentAttackableTiles) {
            tile.SetAttackable();
        }

        return currentAttackableTiles;
    }

    public static void ClearAttackTiles() {
        foreach (var tile in currentAttackableTiles) {
            tile.RevertTile();
        }
        currentAttackableTiles.Clear();
    }

    public static void ResolveAttack(BaseUnit attacker, BaseUnit target) {
        if (attacker == null || target == null) return;
        if (attacker.faction == target.faction) return;
        if (!attacker.CanAttack()) {
            return;
        }

        attacker.PerformAttack(target);
    }
}
