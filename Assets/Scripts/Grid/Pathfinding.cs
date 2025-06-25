using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    public static List<Tile> FindPath(Tile startTile, Tile targetTile, BaseUnit unit) {
        var toSearch = new List<Tile> { startTile };
        var processed = new HashSet<Tile>();

        startTile.SetGCost(0);
        startTile.SetHCost(startTile.GetDistance(targetTile));

        while (toSearch.Count > 0) {
            var current = toSearch[0];
            foreach (var tile in toSearch) {
                if (tile.FCost < current.FCost || (tile.FCost == current.FCost && tile.GCost < current.GCost)) {
                    current = tile;
                }
            }

            if (current == targetTile) {
                var path = new List<Tile>();
                var totalCost = 0;
                while (current != startTile) {
                    var previous = current.Connections;
                    totalCost += GetMoveCost(previous, current);
                    path.Add(current);
                    current = previous;
                }
                path.Reverse();

                if (totalCost > unit.currentMovementPoints)
                    return null;

                return path;
            }

            toSearch.Remove(current);
            processed.Add(current);

            foreach (var neighbor in current.Neightbors) {
                if (!neighbor.IsWalkable() || processed.Contains(neighbor)) continue;

                int moveCost = GetMoveCost(current, neighbor);
                if (moveCost == int.MaxValue) continue;

                int costToNeighbor = (int)(current.GCost + moveCost);
                bool isBetterPath = costToNeighbor < neighbor.GCost;

                if (isBetterPath || !toSearch.Contains(neighbor)) {
                    neighbor.SetGCost(costToNeighbor);
                    neighbor.SetHCost(neighbor.GetDistance(targetTile));
                    neighbor.SetConnections(current);

                    if (!toSearch.Contains(neighbor)) {
                        toSearch.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }


    public static List<Tile> GetReachableTiles(Tile startTile, int maxMovement) {
        var reachable = new List<Tile>();
        var toSearch = new Queue<Tile>();
        var visited = new Dictionary<Tile, int>();

        toSearch.Enqueue(startTile);
        visited[startTile] = 0;

        while (toSearch.Count > 0) {
            var current = toSearch.Dequeue();
            int currentCost = visited[current];

            foreach (var neighbor in current.Neightbors) {
                if (!neighbor.IsWalkable()) continue;

                int moveCost = GetMoveCost(current, neighbor);
                if (moveCost == int.MaxValue) continue;

                int totalCost = currentCost + moveCost;

                if (totalCost <= maxMovement) {
                    if (!visited.ContainsKey(neighbor) || totalCost < visited[neighbor]) {
                        visited[neighbor] = totalCost;
                        toSearch.Enqueue(neighbor);
                        if (!reachable.Contains(neighbor))
                            reachable.Add(neighbor);
                    }
                }
            }
        }

        return reachable;
    }


    public static int GetMoveCost(Tile from, Tile to) {
        int dx = Mathf.Abs((int)from.Coords.Pos.x - (int)to.Coords.Pos.x);
        int dy = Mathf.Abs((int)from.Coords.Pos.y - (int)to.Coords.Pos.y);

        if (dx + dy == 1) return 10;
        if (dx == 1 && dy == 1) return 14;

        return int.MaxValue;
    }


}
