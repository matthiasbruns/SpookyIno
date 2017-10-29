using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[System.Serializable]
public class DungeonThemeData {
    public DungeonTheme Theme;

    public GameObject[] Ground = new GameObject[0];
    public GameObject[] Foliage = new GameObject[0];
    public GameObject[] Walls = new GameObject[0];
    public WallBehavior GenerateWall;
    public PastWallBehavior GeneratePastWall;
    public GameObject[] Minions = new GameObject[0];
    public GameObject Boss;

    public enum WallBehavior {
        Random,
        NorthEastSouthWest,
    }

    public enum PastWallBehavior {
        None,
        Ground,
        Wall
    }
}
