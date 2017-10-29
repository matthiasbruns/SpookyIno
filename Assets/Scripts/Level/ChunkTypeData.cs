using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
    public void AssignTo(ChunkData data) {
        if (Type == ChunkType.Unknown)
            return;
        data.Type = this;
    }
}