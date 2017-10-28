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

    public enum ChunkType {
        Empty,
        SimpleRoom,
        ExtraWall,
        Unknown,
    }
    public class ChunkTypeData {
        public ChunkType Type;
        public double Value;
        public event System.Action<ChunkData> OnAssign;
        public ChunkTypeData() {
        }
        public ChunkTypeData(ChunkType type, double value, System.Action<ChunkData> onAssign = null) {
            Type = type;
            Value = value;
            OnAssign = onAssign;
        }
        public void AssignTo(ChunkData data) {
            data.Type = this;
            OnAssign?.Invoke(data);
        }
    }
    private readonly static List<ChunkTypeData> ChunkTypeDatas = new List<ChunkTypeData>() {
        new ChunkTypeData(ChunkType.Empty, 0, data => data.ExitDirection = ExitDirection.All),
        new ChunkTypeData(ChunkType.SimpleRoom, 1), // Value of 1 = fallback
    };
    private readonly static Dictionary<ChunkType, ChunkTypeData> ChunkTypeExtras = new Dictionary<ChunkType, ChunkTypeData>() {
        { ChunkType.ExtraWall, new ChunkTypeData(ChunkType.ExtraWall, 0) }
    };

    [System.Flags]
    public enum ExitDirection {
        North = 0x01,
        South = 0x02,
        East = 0x04,
        West = 0x08,
        All = North | South | East | West
    }
    public const int ExitDirectionMax = 4;
    public static System.Array ExitDirections = System.Enum.GetValues(typeof(ExitDirection));

    public class ChunkData {

        public int X;
        public int Y;
        public ulong XY => GetXY(X, Y);

        public Random RNG;
        
        // RNG controlled values.
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
            ExitDirectionCount = 2 + (RNG.NextDouble() < 0.1 ? 1 : 0);
            for (int i = 0; i < ExitDirectionCount; i++) {
                ExitDirection old = ExitDirection;
                System.Array wtf = ExitDirections;
                ExitDirection |= (ExitDirection) ExitDirections.GetValue(RNG.Next(ExitDirectionMax));
                if (ExitDirection == old)
                    continue;
            }

            // Get type from "value."
            foreach (ChunkTypeData type in ChunkTypeDatas) {
                if (Value <= type.Value) {
                    type.AssignTo(this);
                    break;
                }
            }

        }

        public bool HasExit(ExitDirection dir) {
            // Check exits of nearby chunks.
            if (_IsExit(dir, ExitDirection.West) && Instance.GetChunkData(X - 1, Y)._HasExit(ExitDirection.East))
                return true;
            if (_IsExit(dir, ExitDirection.East) && Instance.GetChunkData(X + 1, Y)._HasExit(ExitDirection.West))
                return true;
            if (_IsExit(dir, ExitDirection.South) && Instance.GetChunkData(X, Y - 1)._HasExit(ExitDirection.North))
                return true;
            if (_IsExit(dir, ExitDirection.North) && Instance.GetChunkData(X, Y + 1)._HasExit(ExitDirection.South))
                return true;
            // Check own exits.
            return _HasExit(dir);
        }
        private bool _HasExit(ExitDirection dir) {
            return (ExitDirection & dir) == dir;
        }
        private bool _IsExit(ExitDirection a, ExitDirection b) {
            return (a & b) == b;
        }

        public bool IsIsolated {
            get {
                // Check exits of nearby chunks.
                if (Instance.GetChunkData(X - 1, Y)._HasExit(ExitDirection.East))
                    return false;
                if (Instance.GetChunkData(X + 1, Y)._HasExit(ExitDirection.West))
                    return false;
                if (Instance.GetChunkData(X, Y - 1)._HasExit(ExitDirection.North))
                    return false;
                if (Instance.GetChunkData(X, Y + 1)._HasExit(ExitDirection.South))
                    return false;
                return true;
            }
        }

        public void FillChunk() {
            if (Filled)
                return;
            Filled = true;

            // If the room is isolated, set the type to "wall."
            if (IsIsolated)
                ChunkTypeExtras[ChunkType.ExtraWall].AssignTo(this);

            if (Type.Type == ChunkType.Empty || Type.Type == ChunkType.Unknown)
                // Empty or unknown - ignore this.
                return;

            switch (Type.Type) {
                case ChunkType.SimpleRoom:
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

                case ChunkType.ExtraWall:
                    for (int yy = 0; yy < ChunkHeight; yy++) {
                        for (int xx = 0; xx < ChunkWidth; xx++) {
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
