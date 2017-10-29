using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class OutsideGeneratorNeo : MonoBehaviour {

    public static OutsideGeneratorNeo Instance { get; private set; }
    public OutsideGeneratorNeo() {
        Instance = this;
    }

    public const int ChunkWidth = 10;
    public const int ChunkHeight = 10;
    public const int AutoGenerateBorder = 2;

    public ChunkTypeDataList Database;

    public static int Seed;

    private Dictionary<ulong, ChunkData> ChunkMap = new Dictionary<ulong, ChunkData>();

    public GameObject[] Ground = new GameObject[0];
    public GameObject DungeonEntrance;
    public GameObject[] Foliage = new GameObject[0];
    public GameObject[] Walls = new GameObject[0];
    public GameObject[] Enemies = new GameObject[0];

    public AstarPath AStar;

    void Awake() {
        if (Database == null) {
            Database = ScriptableObject.CreateInstance<ChunkTypeDataList>();
            Database.ChunkTypeDatas = new List<ChunkTypeData>() {
                new ChunkTypeData(ChunkType.DungeonEntrance, 0.1f),
                new ChunkTypeData(ChunkType.Empty, 1f)
            };
        }

        if (Seed == 0)
            Seed = new Random().Next();

        AStar = FindObjectOfType<AstarPath>();
        AStar.logPathResults = PathLog.OnlyErrors;
    }

    void Start() {
        Update();
        StartCoroutine(AStarUpdateLoop());
        AstarPath.active.Scan();
    }

    void Update() {
        Camera cam = Camera.main;
        Vector3 camCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -cam.transform.position.z));
        int x = Mathf.RoundToInt(camCenter.x / ChunkWidth);
        int y = Mathf.RoundToInt(camCenter.y / ChunkHeight);
        Vector3 camOrigin = cam.ViewportToWorldPoint(new Vector3(0f, 0f, -cam.transform.position.z));
        int r = Mathf.CeilToInt(Mathf.Max((camCenter.x - camOrigin.x) / ChunkWidth, (camCenter.y - camOrigin.y) / ChunkHeight));
        for (int yy = y - r - AutoGenerateBorder; yy < y + r + AutoGenerateBorder; yy++) {
            for (int xx = x - r - AutoGenerateBorder; xx < x + r + AutoGenerateBorder; xx++) {
                FillChunk(xx, yy);
            }
        }

	}

    IEnumerator AStarUpdateLoop() {
        Camera cam = Camera.main;
        while (this != null && gameObject.activeSelf) {
            yield return new WaitForSeconds(2f);

            Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -cam.transform.position.z));
            center = new Vector3(
                Mathf.Round(center.x),
                Mathf.Round(center.y),
                0f
            );
            ((GridGraph) AStar.graphs[0]).center = center;

            foreach (Progress progress in AstarPath.active.ScanAsync())
                yield return null;
        }
    }

    public Random GetRandom(int x, int y) {
        return new Random(Seed ^ (x * y) + y * y + Seed ^ x + x - y);
    }

    public ChunkData GetChunkData(int x, int y) {
        ulong xy = GetXY(x, y);
        ChunkData data;
        if (ChunkMap.TryGetValue(xy, out data))
            return data; // Chunk already exists, let's just get the data.
        return ChunkMap[xy] = new ChunkData(x, y);
    }

    public void FillChunk(int x, int y) {
        GetChunkData(x, y).FillChunk();
    }

    public static ulong GetXY(int x, int y) {
        return ((ulong) (uint) x << 32) | (uint) y;
    }

}
