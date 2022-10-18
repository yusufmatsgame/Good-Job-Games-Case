using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public SpriteRenderer symbolSpriteRenderer;
    public Tile tile;
    public List<TileController> validNeighbors = new List<TileController>();

    public void Initialize(Tile tile)
    {
        this.tile = tile;
        SetName(tile.coordinates);
        symbolSpriteRenderer.sprite = TileManager.instance.tileSymbolSprites[(int)tile.type];
        StartCoroutine(CheckNeighborsDelayed());
    }

    private IEnumerator CheckNeighborsDelayed()
    {
        yield return new WaitForSeconds(1);
        validNeighbors = TileManager.instance.GetDirectNeighbors(this);
    }

    public void SetName(Vector2Int tileCoordinates)
    {
        name = string.Format("Tile: {0}, {1}", tile.coordinates.x.ToString(), tile.coordinates.y.ToString());
        StartCoroutine(CheckNeighborsDelayed());
    }

    private void OnMouseDown()
    {
        TileManager.instance.TileClicked(this);
    }

    private void OnDestroy()
    {
        if (tile.coordinates.y <= 5)
        {
            GameplayUIController.instance.OnTileDestroyed(tile.type);
            MusicManager.instance.PlayTileSound(tile.type);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        tile.coordinates = new Vector2Int(tile.coordinates.x, transform.GetSiblingIndex());
        if (transform.GetSiblingIndex() > 5) Destroy(gameObject);
        SetName(tile.coordinates);
    }
}
