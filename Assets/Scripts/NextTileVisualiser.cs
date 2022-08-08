using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NextTileVisualiser : MonoBehaviour {
    private int next;

    private SpriteRenderer spriteRenderer;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        if (next != TilePlacement.next) {
            if (TilePlacement.next.HasValue) {
                next = TilePlacement.next.Value;
                if (TilePlacement.tiles[next] != null) spriteRenderer.sprite = TilePlacement.tiles[next].sprite;
            } else {
                spriteRenderer.sprite = null;
            }
        }
    }
}
