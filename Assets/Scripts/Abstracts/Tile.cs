using UnityEngine;

public class Tile
{
    public Vector2Int coordinates;
    public TileType type;

    public Tile(Vector2Int coordinates, TileType type)
    {
        this.coordinates = coordinates;
        this.type = type;
    }
}
