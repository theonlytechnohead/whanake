using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CurrentTileVisualiser : MonoBehaviour {

    private int current;

    private SpriteRenderer spriteRenderer;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        if (current != TilePlacement.current) {
            current = TilePlacement.current;
            spriteRenderer.sprite = TilePlacement.tiles[current].sprite;
        }
    }
}
