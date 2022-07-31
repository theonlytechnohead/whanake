using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    public int maxY = 10;
    public int minY = 0;

    private Camera cameraComponent;

    private float scrollwheel = 0;
    private float touchpad = 0;
    [HideInInspector]
    public static float coast = 0;
    private System.Collections.Generic.List<float> prevMoves = new System.Collections.Generic.List<float>();

    void Start () {
        cameraComponent = GetComponent<Camera>();
    }

    void Update () {
        Vector3 pos = transform.position;

        if (scrollwheel != 0) coast = scrollwheel / cameraComponent.orthographicSize * cameraComponent.aspect;
        if (touchpad != 0) {
            pos.y -= touchpad / cameraComponent.orthographicSize * cameraComponent.aspect;
            coast = 0;
        }

        if (0 < Input.touchCount) {
            TileHighlight.OnlyHighlightWithTouch(true);
            Touch touch = Input.GetTouch(0);
            if (touch.deltaPosition.y != float.NaN) {
                float moved = touch.deltaPosition.y / Screen.height;
                moved *= cameraComponent.orthographicSize * 2f;
                pos.y -= moved;
                if (touch.phase == TouchPhase.Moved) {
                    prevMoves.Add(moved);
                }
                coast = 0;
            }
            if (touch.phase == TouchPhase.Ended && 5 < prevMoves.Count) {
                float average = 0;
                for (int i = prevMoves.Count - 5; i < prevMoves.Count; i++) {
                    average += prevMoves[i];
                }
                average /= 5;
                coast = average;
                prevMoves.Clear();
            }
        } else if (Input.GetButton("Fire1")) {
            TileHighlight.OnlyHighlightWithTouch(false);
            float speed = Input.GetAxis("Mouse Y") / cameraComponent.orthographicSize * cameraComponent.aspect;
            pos.y -= speed;
            coast = speed;
        } else {
            coast = Mathf.Lerp(coast, 0, 10f * 0.75f * Time.deltaTime);
            coast = Mathf.Clamp(coast, -0.1f, 0.1f);
            pos.y -= coast;
            if (maxY < pos.y) {
                pos.y = Mathf.Lerp(pos.y, maxY + (coast * 10f), 10f * 0.75f * Time.deltaTime);
            }
            if (pos.y < minY) {
                pos.y = Mathf.Lerp(pos.y, minY - (coast * 10f), 10f * 0.75f * Time.deltaTime);
            }
        }

        transform.position = pos;
    }

    private void OnGUI () {
        float delta = Event.current.delta.y;
        if (delta != 0) {
            TileHighlight.OnlyHighlightWithTouch(false);
            if (delta == Mathf.Floor(delta)) {
                scrollwheel = Mathf.Clamp(delta, -0.15f, 0.15f);
            } else {
                touchpad = Mathf.Clamp(delta, -2f, 2f);
            }
        } else {
            scrollwheel = 0;
            touchpad = 0;
        }
    }
}
