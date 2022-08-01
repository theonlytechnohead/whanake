using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CurrentTileVisualiser : MonoBehaviour {

    private int current;
    private bool followMouse = false;
    private SpriteRenderer spriteRenderer;

    private Vector3 origin;
    private Vector3 scale;
    private Transform parent;
    private Vector3 followScale = new(0.75f, 0.75f, 0.75f);
    private Vector3 followOffset = new(0.5f, 0.5f, 0.5f);

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        origin = transform.localPosition;
        scale = transform.localScale;
        parent = transform.parent;

        CameraController.InputChanged += delegate (CameraController.InputMethod method) {
            switch (method) {
                case CameraController.InputMethod.MOUSE:
                    followMouse = true;
                    break;
                case CameraController.InputMethod.SCROLLWHEEL:
                    followMouse = true;
                    break;
                case CameraController.InputMethod.TOUCHPAD:
                    if (CameraController.lastInputMethod == CameraController.InputMethod.TOUCH) followMouse = false;
                    else followMouse = true;
                    break;
                case CameraController.InputMethod.TOUCH:
                    followMouse = false;
                    break;
            }
        };
    }

    void Update () {
        if (followMouse) {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;
            transform.parent = null;
            transform.localScale = followScale;
            transform.localPosition = mouse + followOffset;
        } else {
            transform.parent = parent;
            transform.localScale = scale;
            transform.localPosition = origin;
        }
        UpdateHighlight();
    }

    void UpdateHighlight () {
        if (current != TilePlacement.current) {
            if (TilePlacement.current.HasValue) {
                current = TilePlacement.current.Value;
                spriteRenderer.sprite = TilePlacement.tiles[current].sprite;
            } else {
                spriteRenderer.sprite = null;
            }
        }
    }
}
