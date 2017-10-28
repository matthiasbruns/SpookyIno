using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class InverseNormalComponent : MonoBehaviour {

    void Awake() {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length / 3; i++) {
            int t = triangles[i * 3];
            triangles[i * 3] = triangles[(i * 3) + 2];
            triangles[(i * 3) + 2] = t;
        }
        mesh.triangles = triangles;

        Vector2[] uv = mesh.uv;
        for (int i = 0; i < uv.Length; i++) {
            uv[i] = new Vector2(1 - uv[i].x, uv[i].y);
        }
        mesh.uv = uv;

    }

}
