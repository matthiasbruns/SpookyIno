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
    public ChunkExitFlag CloseFlags;

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


        // Get type from "value."
        foreach (ChunkTypeData type in LevelGeneratorNeo.Instance.Database.ChunkTypeDatas) {
            if (Value <= type.Value) {
                type.AssignTo(this);
                break;
            }
        }

    }

    public bool IsClosed(ChunkExitFlag dir) {
        // Check exits of nearby chunks.
        if (_IsDirection(dir, ChunkExitFlag.West) && LevelGeneratorNeo.Instance.GetChunkData(X - 1, Y)._IsClosed(ChunkExitFlag.East))
            return true;
        if (_IsDirection(dir, ChunkExitFlag.East) && LevelGeneratorNeo.Instance.GetChunkData(X + 1, Y)._IsClosed(ChunkExitFlag.West))
            return true;
        if (_IsDirection(dir, ChunkExitFlag.South) && LevelGeneratorNeo.Instance.GetChunkData(X, Y - 1)._IsClosed(ChunkExitFlag.North))
            return true;
        if (_IsDirection(dir, ChunkExitFlag.North) && LevelGeneratorNeo.Instance.GetChunkData(X, Y + 1)._IsClosed(ChunkExitFlag.South))
            return true;
        // Check own exits.
        return _IsClosed(dir);
    }
    private bool _IsClosed(ChunkExitFlag dir) {
        return (CloseFlags & dir) == dir;
    }
    private bool _IsDirection(ChunkExitFlag a, ChunkExitFlag b) {
        return (a & b) == b;
    }

    public bool IsIsolated {
        get {
            // Check walls nearby chunks.
            return
                LevelGeneratorNeo.Instance.GetChunkData(X - 1, Y)._IsClosed(ChunkExitFlag.East) &&
                LevelGeneratorNeo.Instance.GetChunkData(X + 1, Y)._IsClosed(ChunkExitFlag.West) &&
                LevelGeneratorNeo.Instance.GetChunkData(X, Y - 1)._IsClosed(ChunkExitFlag.North) &&
                LevelGeneratorNeo.Instance.GetChunkData(X, Y + 1)._IsClosed(ChunkExitFlag.South);
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

                        if (!IsClosed(ChunkExitFlag.North) && (yy == 0 && xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1))
                            continue;
                        if (!IsClosed(ChunkExitFlag.South) && (yy == LevelGeneratorNeo.ChunkHeight - 1 && xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1))
                            continue;
                        if (!IsClosed(ChunkExitFlag.East) && (xx == 0 && yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1))
                            continue;
                        if (!IsClosed(ChunkExitFlag.West) && (xx == LevelGeneratorNeo.ChunkHeight - 1 && yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1))
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

            case ChunkType.DungeonEntranceOutside:
                for (int yy = 0; yy < LevelGeneratorNeo.ChunkHeight; yy++) {
                    for (int xx = 0; xx < LevelGeneratorNeo.ChunkWidth; xx++) {
                        if (yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1 &&
                            xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1)
                            continue;

                        if (!IsClosed(ChunkExitFlag.North) && (yy == 0 && xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1))
                            continue;
                        if (!IsClosed(ChunkExitFlag.South) && (yy == LevelGeneratorNeo.ChunkHeight - 1 && xx != 0 && xx != LevelGeneratorNeo.ChunkWidth - 1))
                            continue;
                        if (!IsClosed(ChunkExitFlag.East) && (xx == 0 && yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1))
                            continue;
                        if (!IsClosed(ChunkExitFlag.West) && (xx == LevelGeneratorNeo.ChunkHeight - 1 && yy != 0 && yy != LevelGeneratorNeo.ChunkHeight - 1))
                            continue;

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
