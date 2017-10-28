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
        return new Random(Seed ^ x + x - y * y);
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

    public enum ChunkType {
        Empty,
        EmptyRoom,
        Random,
        Unknown
    }
    [System.Serializable]
    public class ChunkTypeData {
        public ChunkType Type;
        public double Value;
        public ChunkTypeData() {
        }
        public ChunkTypeData(ChunkType type, double value) {
            Type = type;
            Value = value;
        }
    }
    private readonly static List<ChunkTypeData> ChunkTypeDatas = new List<ChunkTypeData>() {
        new ChunkTypeData(ChunkType.Empty, 0),
        new ChunkTypeData(ChunkType.EmptyRoom, 1),
        new ChunkTypeData(ChunkType.Random, 1) // Value of 1 = fallback
    };

    [System.Flags]
    public enum ExitDirection {
        North = 0x01,
        South = 0x02,
        East = 0x04,
        West = 0x08
    }
    public static System.Array ExitDirections = System.Enum.GetValues(typeof(ExitDirection));

    [System.Serializable]
    public class ChunkData {

        public int X;
        public int Y;
        public ulong XY => GetXY(X, Y);

        public int Seed;
        public Random RNG;
        
        // RNG-steered values.
        public double Value;
        public int ExitDirectionCount;
        public ExitDirection ExitDirection;

        // Anything we get from the RNG values, or anything else we need.
        public ChunkTypeData Type;

        public bool Filled;

        public ChunkData() {
        }
        public ChunkData(int x, int y) {
            X = x;
            Y = y;

            RNG = Instance.GetRandom(x, y);

            // Fill RNG data.
            Value = RNG.NextDouble();
            ExitDirectionCount = 1 + (RNG.NextDouble() < 0.1 ? 1 : 0);
            for (int i = 0; i < ExitDirectionCount; i++) {
                ExitDirection old = ExitDirection;
                ExitDirection |= (ExitDirection) ExitDirections.GetValue(RNG.Next(ExitDirections.Length));
                if (ExitDirection == old)
                    continue;
            }

            // Get type from "value."
            foreach (ChunkTypeData type in ChunkTypeDatas) {
                if (Value <= type.Value) {
                    Type = type;
                    break;
                }
            }

        }

        public bool HasExit(ExitDirection dir) {
            // Check exits of nearby chunks.
            if (Instance.GetChunkData(X - 1, Y)._HasExit(ExitDirection.East))
                return true;
            if (Instance.GetChunkData(X + 1, Y)._HasExit(ExitDirection.West))
                return true;
            if (Instance.GetChunkData(X, Y - 1)._HasExit(ExitDirection.North))
                return true;
            if (Instance.GetChunkData(X, Y + 1)._HasExit(ExitDirection.South))
                return true;
            // Check own exits.
            return _HasExit(dir);
        }
        private bool _HasExit(ExitDirection dir) {
            return (ExitDirection & dir) == dir;
        }

        public void FillChunk() {
            if (Filled)
                return;
            Filled = true;

            if (Type.Type == ChunkType.Empty || Type.Type == ChunkType.Unknown)
                // Empty or unknown - ignore this.
                return;

            // TODO: Generate random data first.

            switch (Type.Type) {
                case ChunkType.EmptyRoom:
                    for (int yy = 0; yy < ChunkHeight; yy++) {
                        for (int xx = 0; xx < ChunkWidth; xx++) {
                            if ((yy != 0 && yy != ChunkHeight - 1 &&
                                xx != 0 && xx != ChunkWidth - 1) &&
                                RNG.NextDouble() >= 0.01)
                                continue;

                            if (HasExit(ExitDirection.North) && (yy == 0 && xx != 0 && xx != ChunkWidth - 1))
                                continue;
                            if (HasExit(ExitDirection.South) && (yy == ChunkHeight - 1 && xx != 0 && xx != ChunkWidth - 1))
                                continue;
                            if (HasExit(ExitDirection.East) && (xx == 0 && yy != 0 && yy != ChunkHeight - 1))
                                continue;
                            if (HasExit(ExitDirection.West) && (xx == ChunkHeight - 1 && yy != 0 && yy != ChunkHeight - 1))
                                continue;

                            Transform wallType = Instance.BasicWalls[RNG.Next(Instance.BasicWalls.Length)];
                            if (wallType == null)
                                continue;
                            Instantiate(wallType, new Vector3(X * ChunkWidth + xx, Y * ChunkHeight + yy, 0), Quaternion.identity, null);
                        }
                    }
                    break;
            }

        }

    }

}
