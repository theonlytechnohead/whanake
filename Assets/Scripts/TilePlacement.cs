using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilePlacement : MonoBehaviour {

    private Tilemap tilemap;
    public TileBase sheep;
    public TileBase wood;
    public TileBase bricks;
    public TileBase stone;
    public TileBase wheat;
    public TileBase desert;

    private TileBase[] tiles = new TileBase[6];

    private int current = 0;

    void Start () {
        tilemap = GetComponent<Tilemap>();
        tiles[0] = sheep;
        tiles[1] = wood;
        tiles[2] = bricks;
        tiles[3] = stone;
        tiles[4] = wheat;
        tiles[5] = desert;
    }

    void Update () {
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0f;
        Vector3Int cellPosition = tilemap.WorldToCell(mouse);

        if (Input.GetButtonUp("Fire1")) {
            if (legalPlacement(cellPosition) && tilemap.GetTile(cellPosition) == null) {
                tilemap.SetTile(cellPosition, tiles[current++]);
                if (current == 6) current = 0;
            }
        }
    }

    public static bool legalPlacement (Vector3Int cell) {
        int rightLimit = Mathf.Abs(cell.y % 2) == 1 ? 8 : 9;
        if (-9 < cell.x && cell.x < rightLimit) {
            if (-4 < cell.y && cell.y < 16) {
                return true;
            }
        }
        return false;
    }
}
