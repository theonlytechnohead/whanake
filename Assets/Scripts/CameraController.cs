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
    private System.Collections.Generic.List<float> prevMoves = new();

    public enum InputMethod {
        MOUSE,
        SCROLLWHEEL,
        TOUCHPAD,
        TOUCH
    }
    public static InputMethod lastInputMethod;
    public delegate void InputMethodChanged (InputMethod inputMethod);
    public static InputMethodChanged InputChanged;

    void Start () {
        cameraComponent = GetComponent<Camera>();
        SetInputMethod(InputMethod.MOUSE);
        InputChanged?.Invoke(InputMethod.MOUSE);
    }

    void Update () {
        Vector3 pos = transform.position;

        if (scrollwheel != 0) coast = scrollwheel / cameraComponent.orthographicSize * cameraComponent.aspect;
        if (touchpad != 0) {
            pos.y -= touchpad / cameraComponent.orthographicSize * cameraComponent.aspect;
            coast = 0;
        }

        if (0 < Input.touchCount) {
            SetInputMethod(InputMethod.TOUCH);
            Touch touch = Input.GetTouch(0);
            if (touch.deltaPosition.y != float.NaN) {
                if (touch.phase == TouchPhase.Moved) {
                    float moved = touch.deltaPosition.y / Screen.height;
                    moved *= cameraComponent.orthographicSize * 2f;
                    if (0 < prevMoves.Count && 2f < Mathf.Abs(touch.deltaPosition.y)) {
                        pos.y -= moved;
                    }
                    prevMoves.Add(moved);
                }
                coast = 0;

            }
            if (touch.phase == TouchPhase.Ended && 5 < prevMoves.Count) {
                float average = 0;
                for (int i = prevMoves.Count - 5; i < prevMoves.Count; i++) average += prevMoves[i];
                coast = average / 5f;
                prevMoves.Clear();
            }
        } else if (Input.GetButton("Fire1")) {
            SetInputMethod(InputMethod.MOUSE);
            float speed = Input.GetAxis("Mouse Y") / cameraComponent.orthographicSize * cameraComponent.aspect;
            pos.y -= speed;
            coast = speed;
        } else {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) SetInputMethod(InputMethod.MOUSE);
            float clampValue = lastInputMethod == InputMethod.SCROLLWHEEL ? 0.75f : 0.1f;
            coast = Mathf.Clamp(Mathf.Lerp(coast, 0, 10f * 0.75f * Time.deltaTime), -clampValue, clampValue);
            if (lastInputMethod == InputMethod.TOUCHPAD) coast = 0f;
            pos.y -= coast;
            float offset = coast * 10f;
            if (pos.y < minY) pos.y = Mathf.Lerp(pos.y, minY - offset, 10f * 0.75f * Time.deltaTime);
            if (maxY < pos.y) pos.y = Mathf.Lerp(pos.y, maxY - offset, 10f * 0.75f * Time.deltaTime);
        }

        transform.position = pos;
    }

    private void OnGUI () {
        float delta = Event.current.delta.y;
        if (Event.current.type == EventType.ScrollWheel) {
            if (delta != 0) {
                if (delta == Mathf.Floor(delta)) {
                    SetInputMethod(InputMethod.SCROLLWHEEL);
                    scrollwheel = delta < 0 ? -1.25f : 1.25f;
                } else {
                    SetInputMethod(InputMethod.TOUCHPAD);
                    Vector3 pos = transform.position;
                    if ((minY <= pos.y && delta > 0) || (pos.y <= maxY && delta < 0)) touchpad = Mathf.Clamp(delta, -2f, 2f);
                }
            }
        } else {
            scrollwheel = 0;
            touchpad = 0;
        }
    }

    private void SetInputMethod (InputMethod method) {
        if (method != lastInputMethod) {
            InputChanged?.Invoke(method);
            bool hide = method == InputMethod.TOUCH;
            hide |= method == InputMethod.TOUCHPAD && lastInputMethod == InputMethod.TOUCH;
            hide |= method == InputMethod.SCROLLWHEEL && lastInputMethod == InputMethod.TOUCH;
            TileHighlight.OnlyHighlightWithTouch(hide);
            lastInputMethod = method;
        }
    }
}
