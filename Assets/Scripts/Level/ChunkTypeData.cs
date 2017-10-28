using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[System.Serializable]
public class ChunkTypeData {
    public ChunkType Type;
    public double Value;
    public System.Action<ChunkData> OnAssign;
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