using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<Level> levels = new List<Level>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one LevelManager");
        }
        GenerateLevels();
    }

    private void GenerateLevels()
    {
        Level level1 = new Level(35, 15, 15, 0, TileType.blue, TileType.green);
        levels.Add(level1);
    }

}
