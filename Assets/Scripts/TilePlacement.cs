using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilePlacement : MonoBehaviour {

    private Tilemap tilemap;
    public TileBase placement;

    void Start () {
        tilemap = GetComponent<Tilemap>();
    }

    void Update () {
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0f;
        Vector3Int cellPosition = tilemap.WorldToCell(mouse);

        if (Input.GetButtonUp("Fire1")) {
            if (-9 < cellPosition.x && cellPosition.x < 9) {
                if (-6 < cellPosition.y && cellPosition.y < 16) {
                    tilemap.SetTile(cellPosition, placement);
                }
            }
        }
    }
}
