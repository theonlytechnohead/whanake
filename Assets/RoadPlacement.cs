using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class RoadPlacement : MonoBehaviour {
    private Tilemap tilemap;

    public Tile vertical;
    public Tile slopeUp;
    public Tile slopeDown;

    Vector3Int previous;

    void Start () {
        tilemap = GetComponent<Tilemap>();
    }

    void Update () {
        Vector3 mouse = Input.mousePosition;
        Vector3Int cell = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(mouse));
        cell.z = 0;
        if (previous != cell) {
            tilemap.SetTile(previous, null);
            previous = cell;
            Tile road = null;
            if (Mathf.Abs(cell.y) % 2 == 0) {
                // vertical
                if (Mathf.Abs(cell.y) % 4 == 2) {
                    if (Mathf.Abs(cell.x) % 2 == 0) {
                        road = vertical;
                    }
                } else if (Mathf.Abs(cell.y) % 4 == 0) {
                    if (Mathf.Abs(cell.x) % 2 == 1) {
                        road = vertical;
                    }
                }
            } else {
                // slope
                if (Mathf.Abs(cell.y) % 4 == 3) {
                    if (Mathf.Abs(cell.x) % 2 == 0) {
                        road = slopeUp;
                    } else {
                        road = slopeDown;
                    }
                } else if (Mathf.Abs(cell.y) % 4 == 1) {
                    if (Mathf.Abs(cell.x) % 2 == 0) {
                        road = slopeDown;
                    } else {
                        road = slopeUp;
                    }
                }
            }
            tilemap.SetTile(cell, road);
        }
    }
}
