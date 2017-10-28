using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class LevelGeneratorNeo : MonoBehaviour {

    public static LevelGeneratorNeo Instance { get; private set; }
    public LevelGeneratorNeo() {
        Instance = this;
    }

    public const int ChunkWidth = 10;
    public const int ChunkHeight = 10;
    public const int AutoGenerateBorder = 2;

    public const int ExitDirectionMax = 4;
    public static System.Array ExitDirections = System.Enum.GetValues(typeof(ChunkExitFlag));

    public ChunkTypeDataList Database => GameManager.Instance.InDungeon ? GameManager.Instance.chunkDungeonDatabase : GameManager.Instance.chunkOutsideDatabase;

    public Transform[] BasicWalls = new Transform[1];

    public int Seed;

    private Dictionary<ulong, ChunkData> ChunkMap = new Dictionary<ulong, ChunkData>();

    void Awake() {
        if (Seed == 0)
            Seed = new Random().Next();

        FillChunk(0, 0);
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

    public Random GetRandom(int x, int y) {
        return new Random(Seed ^ (x * y) + y * y - x + Seed ^ x + x - y * y);
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
