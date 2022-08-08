using UnityEngine;

public class StateManager : MonoBehaviour {

    public static Vector2Int heightBounds = new(-4, 16);
    public static Vector2Int widthBounds = new(-9, 9);

    public static bool legalPlacement (Vector3Int cell) {
        int rightLimit = Mathf.Abs(cell.y % 2) == 1 ? widthBounds.y - 1 : widthBounds.y;
        if (widthBounds.x < cell.x && cell.x < rightLimit) {
            if (heightBounds.x < cell.y && cell.y < heightBounds.y) {
                return true;
            }
        }
        return false;
    }

    public enum State {
        NONE,
        TILE,
        ROAD,
        TOWN
    }

    public static State state;
    public delegate void StateDelegate (State state);
    public static StateDelegate StateChanged;

    public static int tilesToPlace = 0;
    public static int roadsToPlace = 0;
    public static int townsToPlace = 0;

    void Start () {
        state = State.TILE;

        StateChanged += delegate (State s) {
            print(s);
        };

        StateChanged?.Invoke(state);
        tilesToPlace = 9;
        roadsToPlace = 5;
        townsToPlace = 2;
    }

    void Update () {
        if (0 < tilesToPlace) {
            if (state != State.TILE) {
                StateChanged?.Invoke(State.TILE);
                state = State.TILE;
            }
        } else {
            if (0 < roadsToPlace) {
                if (state != State.ROAD) {
                    StateChanged?.Invoke(State.ROAD);
                    state = State.ROAD;
                }
            } else if (0 < townsToPlace) {
                if (state != State.TOWN) {
                    StateChanged?.Invoke(State.TOWN);
                    state = State.TOWN;
                }
            }
        }
    }
}
