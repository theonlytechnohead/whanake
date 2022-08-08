using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TownPlacement : MonoBehaviour {

    public int grid;

    private Tilemap tilemap;

    private Vector3Int current;

    public Tile town;

    void Start () {
        tilemap = GetComponent<Tilemap>();
    }

    void Update () {
        if (StateManager.state == StateManager.State.TOWN) {
            Vector3 mouse = Input.mousePosition;
            Vector3 world = Camera.main.ScreenToWorldPoint(mouse);
            Vector3Int cell = tilemap.WorldToCell(world);
            cell.z = 0;
            Tile tile = null;
            if (cell != current) {
                if (Mathf.Abs(cell.y) % 2 == 0) {
                    if (Mathf.Abs(cell.y) % 4 == 2) {
                        if (Mathf.Abs(cell.x) % 2 == 1) {
                            tile = town;
                        }
                    } else if (Mathf.Abs(cell.y) % 4 == 0) {
                        if (Mathf.Abs(cell.x) % 2 == 0) {
                            tile = town;
                        }
                    }
                }
                tilemap.SetTile(current, null);
                tilemap.SetTile(cell, tile);
                current = cell;
            }
        }
    }
}
