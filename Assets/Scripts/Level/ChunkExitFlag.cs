using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[System.Flags]
public enum ChunkExitFlag {
    North = 0x01,
    South = 0x02,
    East = 0x04,
    West = 0x08,
    All = North | South | East | West,
    None = 0x00,
    NoForce = -1
}
