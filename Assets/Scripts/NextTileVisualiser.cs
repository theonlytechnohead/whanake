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
            next = TilePlacement.next;
            spriteRenderer.sprite = TilePlacement.tiles[next].sprite;
        }
    }
}
