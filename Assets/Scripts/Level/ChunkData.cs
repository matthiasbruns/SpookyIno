using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ChunkData {

    public int X;
    public int Y;
    public ulong XY => LevelGeneratorNeo.GetXY(X, Y);

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

        RNG = LevelGeneratorNeo.Instance.GetRandom(x, y);

        // Fill RNG data.
        Value = RNG.NextDouble();
        ExitDirectionCount = 2 + (RNG.NextDouble() < 0.1 ? 1 : 0);
        for (int i = 0; i < ExitDirectionCount; i++) {
            ExitDirection old = ExitDirection;
            ExitDirection |= (ExitDirection) LevelGeneratorNeo.ExitDirections.GetValue(RNG.Next(LevelGeneratorNeo.ExitDirectionMax));
            if (ExitDirection == old)
                continue;
        }

        // Get type from "value."
        foreach (ChunkTypeData type in LevelGeneratorNeo.Instance.Database.ChunkTypeDatas) {
            if (Value <= type.Value) {
                type.AssignTo(this);
                break;
            }
        }

    }

    public bool HasExit(ExitDirection dir) {
        // Check exits of nearby chunks.
        if (_IsExit(dir, ExitDirection.West) && LevelGeneratorNeo.Instance.GetChunkData(X - 1, Y)._HasExit(ExitDirection.East))
            return true;
        if (_IsExit(dir, ExitDirection.East) && LevelGeneratorNeo.Instance.GetChunkData(X + 1, Y)._HasExit(ExitDirection.West))
            return true;
        if (_IsExit(dir, ExitDirection.South) && LevelGeneratorNeo.Instance.GetChunkData(X, Y - 1)._HasExit(ExitDirection.North))
            return true;
        if (_IsExit(dir, ExitDirection.North) && LevelGeneratorNeo.Instance.GetChunkData(X, Y + 1)._HasExit(ExitDirection.South))
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
            if (LevelGeneratorNeo.Instance.GetChunkData(X - 1, Y)._HasExit(ExitDirection.East))
                return false;
            if (LevelGeneratorNeo.Instance.GetChunkData(X + 1, Y)._HasExit(ExitDirection.West))
                return false;
            if (LevelGeneratorNeo.Instance.GetChunkData(X, Y - 1)._HasExit(ExitDirection.North))
                return false;
            if (LevelGeneratorNeo.Instance.GetChunkData(X, Y + 1)._HasExit(ExitDirection.South))
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
            LevelGeneratorNeo.Instance.Database.ChunkTypeExtras[(int) ChunkType.ExtraWall].AssignTo(this);

        if (Type.Type == ChunkType.Empty || Type.Type == ChunkType.Unknown)
            // Empty or unknown - ignore this.
            return;

        switch (Type.Type) {
            case ChunkType.SimpleRoom:
                for (int yy = 0; yy < LevelGeneratorNeo.ChunkHeight; yy++) {
                    for (int xx = 0; xx < LevelGeneratorNeo.ChunkWidth; xx++) {
                        if ((yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1 &&
                            xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1) &&
                            RNG.NextDouble() >= 0.01)
                            continue;

                        if (HasExit(ExitDirection.North) && (yy == 0 && xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1))
                            continue;
                        if (HasExit(ExitDirection.South) && (yy == LevelGeneratorNeo.ChunkHeight - 1 && xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1))
                            continue;
                        if (HasExit(ExitDirection.East) && (xx == 0 && yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1))
                            continue;
                        if (HasExit(ExitDirection.West) && (xx == LevelGeneratorNeo.ChunkHeight - 1 && yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1))
                            continue;

                        Transform wallType = LevelGeneratorNeo.Instance.BasicWalls[RNG.Next(LevelGeneratorNeo.Instance.BasicWalls.Length)];
                        if (wallType == null)
                            continue;
                        Object.Instantiate(wallType, new Vector3(X * LevelGeneratorNeo.ChunkWidth + xx, Y * LevelGeneratorNeo.ChunkHeight + yy, 0), Quaternion.identity, null);
                    }
                }
                break;

            case ChunkType.ExtraWall:
                for (int yy = 0; yy < LevelGeneratorNeo.ChunkHeight; yy++) {
                    for (int xx = 0; xx < LevelGeneratorNeo.ChunkWidth; xx++) {
                        Transform wallType = LevelGeneratorNeo.Instance.BasicWalls[RNG.Next(LevelGeneratorNeo.Instance.BasicWalls.Length)];
                        if (wallType == null)
                            continue;
                        Object.Instantiate(wallType, new Vector3(X * LevelGeneratorNeo.ChunkWidth + xx, Y * LevelGeneratorNeo.ChunkHeight + yy, 0), Quaternion.identity, null);
                    }
                }
                break;
        }

    }

}
