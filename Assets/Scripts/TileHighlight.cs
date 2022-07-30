using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileHighlight : MonoBehaviour {

    private Tilemap tilemap;
    public TileBase legalHighlight;
    public TileBase illegalHighlight;

    private Vector3Int? previous;

    void Start () {
        tilemap = GetComponent<Tilemap>();
    }

    void Update () {
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0f;
        Vector3Int cellPosition = tilemap.WorldToCell(mouse);
        if (cellPosition != previous) {
            if (TilePlacement.legalPlacement(cellPosition)) tilemap.SetTile(cellPosition, legalHighlight);
            else tilemap.SetTile(cellPosition, illegalHighlight);
            if (previous.HasValue) tilemap.SetTile(previous.Value, null);
            previous = cellPosition;
        }
    }
}
