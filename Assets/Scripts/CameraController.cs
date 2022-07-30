using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public int maxY = 10;
    public int minY = 0;

    private Camera cameraComponent;

    private float scrollwheel = 0;
    private float touchpad = 0;
    private float coast = 0;
    private System.Collections.Generic.List<float> prevMoves = new System.Collections.Generic.List<float>();

    // Start is called before the first frame update
    void Start()
    {
        cameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        //float scroll = -Input.GetAxis("Mouse ScrollWheel"); // deprecated
        if (scrollwheel != 0) coast = scrollwheel / cameraComponent.orthographicSize * cameraComponent.aspect;
        if (touchpad != 0) {
            pos.y -= touchpad / cameraComponent.orthographicSize * cameraComponent.aspect;
            coast = 0;
        }

        if (0 < Input.touchCount) {
            Touch touch = Input.GetTouch(0);
            float moved = touch.deltaPosition.y / Screen.height;
            moved *= cameraComponent.orthographicSize * 2f;
            pos.y -= moved;
            if (touch.phase == TouchPhase.Moved) {
                prevMoves.Add(moved);
            }
            if (touch.phase == TouchPhase.Ended) {
                float average = 0;
                int values = Mathf.Min(5, prevMoves.Count);
                for (int i = prevMoves.Count - values; i < prevMoves.Count; i++) {
                    average += prevMoves[i];
                }
                average /= values;
                coast = average * 10f;
                prevMoves.Clear();
            }
        } else if (Input.GetButton("Fire1")) {
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
            if (delta == Mathf.Floor(delta)) {
                scrollwheel = delta;
            } else {
                touchpad = delta;
                touchpad = Mathf.Clamp(touchpad, -2f, 2f);
            }
        } else {
            scrollwheel = 0;
            touchpad = 0;
        }
    }
}
