using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DungeonGeneratorNeo : MonoBehaviour {

    public static DungeonGeneratorNeo Instance { get; private set; }
    public DungeonGeneratorNeo() {
        Instance = this;
    }

    public int Seed;
    public Random RNG;

    public Dictionary<ulong, GameObject> TileMap = new Dictionary<ulong, GameObject>();

    public GameObject[] BasicWalls = new GameObject[1];

    void Awake() {
        if (Seed == 0)
            Seed = new Random().Next();
        RNG = new Random(Seed);
    }

    void Start() {
        StartCoroutine(Generate());
    }

    public IEnumerator Generate() {
        GenerateRoom(-8, -8, 16, 16);
        bool xb = RNG.Next(2) == 1;
        bool yb = RNG.Next(2) == 1;
        int x = xb ? -3 : 7;
        int y = yb ? -3 : 7;
        int w, h;
        for (int i = RNG.Next(4, 8); i > -1; --i) {
            w = RNG.Next(10, 25);
            h = RNG.Next(10, 25);
            GenerateRoom(
                xb ? (x - w + RNG.Next(6, 10)) : (x - RNG.Next(6, 10)),
                yb ? (y - h + RNG.Next(6, 10)) : (y - RNG.Next(6, 10)),
                w + RNG.Next(6, 10), h + RNG.Next(6, 10)
            );
            GenerateRoom(
                xb ? (x - w + 4) : (x - 4),
                yb ? (y - h + 4) : (y - 4),
                w + 8, h + 8
            );
            yield return null;
            xb = RNG.Next(2) == 1;
            yb = RNG.Next(2) == 1;
            x = xb ? (x - w + 3) : (x + w - 3);
            y = yb ? (y - h + 3) : (y + h - 3);
        }

        w = RNG.Next(25, 30);
        h = RNG.Next(20, 25);
        GenerateRoom(
            xb ? (x - w + 6) : (x - 6),
            yb ? (y - h + 6) : (y - 6),
            w + 12, h + 12
        );
        // TODO: Generate boss in there.
    }

    public void GenerateRoom(int x, int y, int w, int h) {
        for (int yy = y; yy < y + h; yy++) {
            for (int xx = x; xx < x + w; xx++) {
                ulong xy = GetXY(xx, yy);
                GameObject tile;
                bool wall =
                    yy == y || yy == y + h - 1 ||
                    xx == x || xx == x + w - 1;
                if (TileMap.TryGetValue(xy, out tile)) {
                    if (tile == null)
                        continue;
                    Destroy(tile);
                }
                TileMap[xy] = !wall ? null : CreateWall(xx, yy);
            }
        }
    }

    public GameObject CreateWall(int x, int y) {
        GameObject wallType = BasicWalls[RNG.Next(BasicWalls.Length)];
        if (wallType == null)
            return null;
        return Instantiate(wallType, new Vector3(x, y, 0), Quaternion.identity, null);
    }

    public static ulong GetXY(int x, int y) {
        return ((ulong) (uint) x << 32) | (uint) y;
    }

}
