using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileHighlight : MonoBehaviour {

    private Tilemap tilemap;
    public TileBase legalHighlight;
    public TileBase illegalHighlight;

    private Vector3Int? previous;

    private static bool requireTouch = false;

    void Start () {
        tilemap = GetComponent<Tilemap>();
    }

    void Update () {
        if (requireTouch) {
            if (0 < Input.touchCount) {
                DoHighlight();
            } else {
                ClearHighlight();
            }
        } else {
            DoHighlight();
        }
    }

    Vector3Int GetCellUnderMouse () {
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0f;
        return tilemap.WorldToCell(mouse);
    }

    void DoHighlight () {
        Vector3Int cellPosition = GetCellUnderMouse();
        if (cellPosition != previous) {
            if (TilePlacement.legalPlacement(cellPosition)) tilemap.SetTile(cellPosition, legalHighlight);
            else tilemap.SetTile(cellPosition, illegalHighlight);
            if (previous.HasValue) tilemap.SetTile(previous.Value, null);
            previous = cellPosition;
        }
    }

    void ClearHighlight () {
        tilemap.SetTile(GetCellUnderMouse(), null);
    }

    public static void OnlyHighlightWithTouch (bool value) {
        requireTouch = value;
    }
}
