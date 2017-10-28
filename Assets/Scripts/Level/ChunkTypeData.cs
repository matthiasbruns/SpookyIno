using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[System.Serializable]
public class ChunkTypeData {
    public ChunkType Type;
    public double Value;
    public ChunkExitFlag ForceExit = ChunkExitFlag.NoForce;
    public ChunkTypeData() {
    }
    public ChunkTypeData(ChunkType type, double value, System.Action<ChunkData> onAssign = null) {
        Type = type;
        Value = value;
    }
    public void AssignTo(ChunkData data) {
        if (Type == ChunkType.Unknown)
            return;
        data.Type = this;
        if (ForceExit == ChunkExitFlag.NoForce)
            data.CloseFlags = ForceExit;
    }
}