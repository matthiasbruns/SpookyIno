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

    public int Factor = 1;

    public DungeonBoss Boss;

    public bool Done { get; private set; }

    void Awake() {
        if (Seed == 0)
            Seed = GameSceneManager.Instance.DungeonSeed;
        RNG = new Random(Seed);
        Boss = GameSceneManager.Instance.LoadingBoss;
    }

    void Start() {
        StartCoroutine(Generate());
    }

    public IEnumerator Generate() {
        Done = false;

        Retry:
        GenerateRoom(-8, -8, 16, 16);
        bool xb = RNG.Next(2) == 1;
        bool yb = RNG.Next(2) == 1;
        int x = xb ? -3 : 7;
        int y = yb ? -3 : 7;
        int w, h;
        for (int i = Factor * RNG.Next(6, 12); i > -1; --i) {
            w = RNG.Next(8, 12) / Factor;
            h = RNG.Next(8, 12) / Factor;
            GenerateRoom(
                xb ? (x - w + RNG.Next(2, 5) * Factor) : (x - RNG.Next(2, 5) * Factor),
                yb ? (y - h + RNG.Next(2, 5) * Factor) : (y - RNG.Next(2, 5) * Factor),
                w + RNG.Next(2, 5) * Factor, h + RNG.Next(2, 5) * Factor
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

        if (new Vector2(x, y).sqrMagnitude <= (5 * 5) * Factor)
            goto Retry;

        bool corridorH = RNG.Next(2) == 1;

        w = corridorH ? 60 : 4;
        h = corridorH ? 4 : 60;
        GenerateRoom(
            x, y,
            w, h
        );
        yield return null;

        if (corridorH)
            x += w - 8;
        else
            y += h - 8;

        w = RNG.Next(25, 30);
        h = RNG.Next(20, 25);
        GenerateRoom(
            x - w / 2, y - h / 2,
            w, h
        );
        // TODO: Generate boss in there.

        Done = true;
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
                    if (wall)
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
        return Instantiate(wallType, new Vector3(x, y, 0f), Quaternion.identity, null);
    }

    public static ulong GetXY(int x, int y) {
        return ((ulong) (uint) x << 32) | (uint) y;
    }

}
