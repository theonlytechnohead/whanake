using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilePlacement : MonoBehaviour {

    private Tilemap tilemap;
    public Tile sheep;
    public Tile wood;
    public Tile bricks;
    public Tile stone;
    public Tile wheat;
    public Tile desert;

    [HideInInspector]
    public static Tile[] tiles = new Tile[6];

    [HideInInspector]
    public static int? current;
    [HideInInspector]
    public static int? next;

    private double startClick;
    private Vector3 startPosition;

    void Start () {
        tilemap = GetComponent<Tilemap>();
        tiles[0] = sheep;
        tiles[1] = wood;
        tiles[2] = bricks;
        tiles[3] = stone;
        tiles[4] = wheat;
        tiles[5] = desert;
        current = Mathf.FloorToInt(Random.Range(0, tiles.Length));
        next = Mathf.FloorToInt(Random.Range(0, tiles.Length));
    }

    void Update () {
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0f;
        Vector3Int cellPosition = tilemap.WorldToCell(mouse);

        if (Input.GetButtonDown("Fire1")) {
            startClick = Time.timeAsDouble;
            startPosition = Input.mousePosition;
        }

        if (Input.GetButtonUp("Fire1")) {
            if (Time.timeAsDouble - startClick < 0.1f || Vector3.Distance(Input.mousePosition, startPosition) < 5f) {
                if (Mathf.Abs(CameraController.coast) < 0.01f) {
                    if (StateManager.legalPlacement(cellPosition) && tilemap.GetTile(cellPosition) == null) {
                        if (0 < StateManager.tilesToPlace) {
                            StateManager.tilesToPlace--;
                            if (current.HasValue) {
                                tilemap.SetTile(cellPosition, tiles[current.Value]);
                                if (next.HasValue) current = next;
                                else current = null;
                                if (1 < StateManager.tilesToPlace) {
                                    next = Mathf.FloorToInt(Random.Range(0, tiles.Length));
                                } else {
                                    next = null;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
