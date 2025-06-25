using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridWorldManager : MonoBehaviour {

    public static GridWorldManager Instance { get; private set; }

    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private Tile grassTile;
    [SerializeField] private Tile yellowGrassTile;
    [SerializeField] private Tile MountainTile;

    [SerializeField] private new Transform camera;
    public List<Tile> selectedDestinationPath;
    public List<Tile> reachableTiles = new List<Tile>();

    public Dictionary<Vector2, Tile> tileDictionary { get; private set; }

    private Tile startNode;
    private Tile targetNode;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }


    /* Generador de mapas, basado en una grilla de tiles.
     * Recibe el tamaño de la grilla y el prefab de los tiles.
     * Por el momento los tiles son planos, pero en un futuro se pueden agregar tiles con diferentes alturas.
     * Promoviendo la generacion de biomas, basado en cercania. Por ejemplo si el tile es de tipo montaña con su correspondiente peso.
     * Los tiles cercanos a este seran de tipo montaña.
     * Por el momento es aleatorio.
     */
    public void GenerateGrid() {

        tileDictionary = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                float rand = Random.value;
                Tile prefabToUse;

                if (rand < 0.4f) {
                    prefabToUse = grassTile;
                } else if (rand < 0.8f) {
                    prefabToUse = yellowGrassTile;
                } else {
                    prefabToUse = MountainTile;
                }

                var tileInstance = Instantiate(prefabToUse, new Vector3(x, y), Quaternion.identity);
                tileInstance.name = $"Cell {x} {y}";

                tileInstance.Init(prefabToUse == MountainTile ? false : true, new SquareCoords {Pos = new Vector3(x, y)});

                tileDictionary.Add(new Vector2(x, y), tileInstance);
            }
        }

        camera.position = new Vector3((float)width / 2f - 0.5f, (float)height / 2f - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.SpawnPlayerTroops);

        foreach (var tile in tileDictionary.Values) tile.CacheNeighbors();

        Tile.OnTileClicked += OnTileClickedHandler;
    }

    private void OnDestroy() {
        Tile.OnTileClicked -= OnTileClickedHandler;
    }

    private void OnTileClickedHandler(Tile tile) {
        targetNode = tile;

        ClearSelectedDestinationPathTiles();

        var path = Pathfinding.FindPath(UnitWorldManager.Instance.selectedPlayerUnit.occupiedTile, targetNode, UnitWorldManager.Instance.selectedPlayerUnit);

        if (path == null) return;

        foreach (var tilePath in path) {
            tilePath.SetReachable();
        }

        selectedDestinationPath = path;
        foreach (var tilePath in selectedDestinationPath) {
            tilePath.walkeableIndicator.SetActive(true);

        }
    }

    /* Metodo encargado de seleccionar un tile aleatorio para el spawn del jugador.
     * Por el momento selecciona un tile aleatorio de la mitad izquierda del mapa.
     * En un futuro deberia solo elegir aquellos tiles marcados como playerSpawn o similar.
     */
    public Tile GetPlayerSpawnTile() {
        return tileDictionary.Where(tile => tile.Key.x < width / 2f && tile.Value.IsWalkable() && !tile.Value.unitOnTile)
            .OrderBy(Tile => Random.value)
            .First().Value;
    }

    public Tile GetEnemySpawnTile() {
        return tileDictionary.Where(tile => tile.Key.x > width / 2f && tile.Value.IsWalkable() && !tile.Value.unitOnTile)
            .OrderBy(Tile => Random.value)
        .First().Value;
    }

    public Tile GetTileAtPosition(Vector2 position) => tileDictionary.TryGetValue(position, out var tile) ? tile : null;

    public void ShowReachableTiles(BaseUnit unit) {
        ClearReachableTiles();

        reachableTiles = Pathfinding.GetReachableTiles(unit.occupiedTile, unit.currentMovementPoints);

        foreach (var tile in reachableTiles) {
            tile.SetReachable();
        }
    }

    public void ClearReachableTiles() {
        foreach (var tile in reachableTiles) {
            tile.RevertTile();
        }
        reachableTiles.Clear();
    }

    public void ClearSelectedDestinationPathTiles() {
        foreach (var tilePath in selectedDestinationPath) {
            tilePath.RevertTile();
        }
        selectedDestinationPath.Clear();
    }

    public void DeselectUnit(BasePlayerUnit unit) {
        if (unit == null) return;
        unit.HideActionIcons();
        UnitWorldManager.Instance.SetSelectedPlayerUnit(null);
        ClearReachableTiles();
        ClearSelectedDestinationPathTiles();
    }
}
