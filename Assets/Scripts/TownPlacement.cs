using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TownPlacement : MonoBehaviour {

    public int grid;

    private static Tilemap tilemap;

    private Vector3Int current;

    void Start () {
        tilemap = GetComponent<Tilemap>();
    }

    void Update () {
        Vector3 mouse = Input.mousePosition;
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);
        Vector3Int cell = tilemap.WorldToCell(world);
        cell.z = 0;
        if (cell != current) {
            current = cell;
            bool valid = false;
            if (Mathf.Abs(cell.y) % 2 == grid) {
                valid = true;
                print($"{grid} {cell}: {valid}");
            }
        }
    }
}
