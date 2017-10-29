using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class ChunkData {

    public int X;
    public int Y;
    public ulong XY => OutsideGeneratorNeo.GetXY(X, Y);

    public Random RNG;

    // RNG controlled values.
    public double Value;

    // Anything we get from the RNG values, or anything else we need.
    public ChunkTypeData Type;

    public bool Filled;

    private List<GameObject> Objects = new List<GameObject>();

    public ChunkData() {
    }
    public ChunkData(int x, int y) {
        X = x;
        Y = y;

        RNG = OutsideGeneratorNeo.Instance.GetRandom(x, y);

        // Fill RNG data.
        Value = RNG.NextDouble();

        // Get type from "value."
        foreach (ChunkTypeData type in OutsideGeneratorNeo.Instance.Database.ChunkTypeDatas) {
            if (Value <= type.Value) {
                type.AssignTo(this);
                break;
            }
        }

    }

    public void FillChunk() {
        if (Filled)
            return;
        Filled = true;

        // Fill ground randomly.
        for (int yy = 0; yy < OutsideGeneratorNeo.ChunkHeight; yy += 2) {
            for (int xx = 0; xx < OutsideGeneratorNeo.ChunkWidth; xx += 2) {
                Register(Object.Instantiate(OutsideGeneratorNeo.Instance.Ground[RNG.Next(OutsideGeneratorNeo.Instance.Ground.Length)], new Vector3(X * OutsideGeneratorNeo.ChunkWidth + xx, Y * OutsideGeneratorNeo.ChunkHeight + yy, 0.99f), Quaternion.identity, null));

                if (RNG.NextDouble() < 0.8)
                    continue;
                Register(Object.Instantiate(OutsideGeneratorNeo.Instance.Foliage[RNG.Next(OutsideGeneratorNeo.Instance.Foliage.Length)], new Vector3(X * OutsideGeneratorNeo.ChunkWidth + xx, Y * OutsideGeneratorNeo.ChunkHeight + yy, 0.99f), Quaternion.identity, null));
            }
        }

        if (Type.Type == ChunkType.Empty || Type.Type == ChunkType.Unknown)
            // Empty or unknown - ignore this.
            return;

        switch (Type.Type) {
            case ChunkType.DungeonEntrance:
                if (-1 < X && X < 1 &&
                    -1 < Y && Y < 1)
                    // Don't spawn if too close to world origin.
                    break;
                GameObject entrance;
                Objects.Add(entrance = Object.Instantiate(OutsideGeneratorNeo.Instance.DungeonEntrance, new Vector3((X + 0.5f) * OutsideGeneratorNeo.ChunkWidth, (Y + 0.5f) * OutsideGeneratorNeo.ChunkHeight, 0), Quaternion.identity, null));

                DungeonTransitionComponent transition = entrance.transform.GetComponentInChildren<DungeonTransitionComponent>();
                transition.Seed = RNG.Next();

                break;
        }

    }

    private void Register(GameObject obj) {
        Objects.Add(obj);
        SceneManager.MoveGameObjectToScene(obj, OutsideGeneratorNeo.Instance.gameObject.scene);
    }

}
