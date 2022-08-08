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
        INITIAL_WORLD,
        INITIAL_BUILD,
        WORLD,
        BUILD
    }

    public static State state;
    public delegate void StateDelegate (State state);
    public static StateDelegate StateChanged;

    public static int tilesToPlace = 0;

    void Start () {
        state = State.INITIAL_WORLD;

        StateChanged += delegate (State s) {
            print(s);
        };

        StateChanged?.Invoke(state);
        //tilesToPlace = 49;
        tilesToPlace = 9;
    }

    void Update () {
        if (0 < tilesToPlace) {
            if (state != State.INITIAL_WORLD && state != State.WORLD) {
                StateChanged?.Invoke(State.WORLD);
                state = State.WORLD;
            }
        } else {
            if (state == State.INITIAL_WORLD) {
                StateChanged?.Invoke(State.INITIAL_BUILD);
                state = State.INITIAL_BUILD;
            } else if (state == State.WORLD) {
                StateChanged?.Invoke(State.BUILD);
                state = State.BUILD;
            }
        }
    }
}
