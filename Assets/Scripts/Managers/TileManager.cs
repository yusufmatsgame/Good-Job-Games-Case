using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Sprite[] tileSymbolSprites;
    public List<TileController> tiles = new List<TileController>();
    public Transform[] spawnPoints;
    public GameObject tilePrefab;
    public static TileManager instance;
    public int rows;
    private WaitForSeconds pacing = new WaitForSeconds(0.1f);

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("birden fazla tile manager");
        }
        else
        {
            instance = this;
        }
    }

    public void StartGame()
    {
        StartCoroutine(GenerateTiles());
    }

    public void Rocket(int y)
    {
        List<TileController> rocketTiles = new List<TileController>();
        foreach(Transform child in spawnPoints[y])
        {
            rocketTiles.Add(child.GetComponent<TileController>());
        }
        foreach(TileController tile in rocketTiles)
        {
            Destroy(tile.gameObject);
        }
    }

    public void Bomb(Vector2Int coordinates)
    {
        //Check tiles around coordinates
        //Add to a list
        // Destroy them
    }

    public void DiscoBall(TileType tileType)
    {
        List<TileController> discoTiles = new List<TileController>();
        foreach(TileController tile in tiles)
        {
            if (tile.tile.type == tileType) discoTiles.Add(tile);
        }

        foreach(TileController tile in discoTiles)
        {
            Destroy(tile);
        }
    }

    internal void TileClicked(TileController tileController)
    {
        List<TileController> interestingNeighbors = new List<TileController>();
        List<TileController> furtherExamination = new List<TileController>();
        foreach(TileController tile in tiles)
        {
            if (tile.validNeighbors.Contains(tileController))
            {
                interestingNeighbors.Add(tile);
            }
            else
            {
                furtherExamination.Add(tile);
            }
        }

        while(furtherExamination.Count > 0)
        {
            TileController checkingTile = furtherExamination[0];
            furtherExamination.Remove(checkingTile);
            bool addToInteresting = false;
            foreach(TileController tile in interestingNeighbors)
            {
                if (checkingTile.validNeighbors.Contains(tile))
                {
                    addToInteresting = true;
                    break;
                }
                 
            }
            if (addToInteresting) interestingNeighbors.Add(checkingTile);
        }
        StartCoroutine(CheckChain(tileController, interestingNeighbors));
    }

    private IEnumerator CheckChain(TileController tileController, List<TileController> directNeighbors)
    {
        /*GenerateTileAtColumn(tileController.tile.coordinates);
        tiles.Remove(tileController);
        Destroy(tileController.gameObject);*/
        foreach (TileController tile in directNeighbors)
        {
            GenerateTileAtColumn(tile.tile.coordinates);
            tiles.Remove(tile);
            if(tile !=null) Destroy(tile.gameObject);
          //  yield return pacing;
        }
        GameplayUIController.instance.LowerRemainingMoves();
        yield return null;
    }

    private IEnumerator ReassignCoordinates()
    {
        yield return new WaitForEndOfFrame();
        foreach (TileController tile in tiles)
        {
            int parentIndex = 0;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i] == tile.transform.parent)
                {
                    parentIndex = i;
                    break;
                }
            }
            tile.tile.coordinates = new Vector2Int(parentIndex, tile.transform.GetSiblingIndex());
            tile.SetName(tile.tile.coordinates);
        }
    }

    private void GenerateTileAtColumn(Vector2Int coordinates)
    {
        GameObject tile = Instantiate(tilePrefab, spawnPoints[coordinates.x]);
        int typeRandomized = UnityEngine.Random.Range(0, 4);
        Tile tileData = new Tile(coordinates, (TileType)typeRandomized);
        tile.GetComponent<TileController>().Initialize(tileData);
        tiles.Add(tile.GetComponent<TileController>());
        StartCoroutine(ReassignCoordinates());
    }

    private IEnumerator GenerateTiles()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < spawnPoints.Length; j++)
            {
                GameObject tile = Instantiate(tilePrefab, spawnPoints[j]);
                Vector2Int coordinates = new Vector2Int(j, i);
                int typeRandomized = UnityEngine.Random.Range(0, 4);
                Tile tileData = new Tile(coordinates, (TileType)typeRandomized);
                tile.GetComponent<TileController>().Initialize(tileData);
                tiles.Add(tile.GetComponent<TileController>());
                yield return pacing;
            }
        }
        yield return null;
    }

    public List<TileController> GetDirectNeighbors(TileController t)
    {
        List<TileController> neighbors = new List<TileController>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                if (Mathf.Abs(i) == 1 && Mathf.Abs(j) == 1) continue;
                int checkX = t.tile.coordinates.x + j;
                int checkY = t.tile.coordinates.y + i;
                if (checkY <= 6 &&
    checkX >= 0 &&
    checkY >= 0 &&
    checkX <= 5 &&
    FindTileByCoordinates(new Vector2Int(checkX, checkY)).tile.type == t.tile.type
    )
                {
                    neighbors.Add(FindTileByCoordinates(new Vector2Int(checkX, checkY)));
                }
            }
        }
        return neighbors;
    }

    private TileController FindTileByCoordinates(Vector2Int coordinates)
    {
        foreach (TileController tile in tiles)
        {
            if (tile.tile.coordinates == coordinates)
            {
                return tile;
            }
        }
        return null;
    }

    public void DestroyTiles()
    {
        foreach (var item in tiles)
        {
            
        Destroy(item);
        }
    }
}
