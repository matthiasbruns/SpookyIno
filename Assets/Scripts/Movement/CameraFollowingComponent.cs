using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollowingComponent : MonoBehaviour {

    public List<Transform> Following = new List<Transform>() {
        null
    };
    public string FollowingTag = "Player";

    public float PositionSmoothing = 0.3f;

    public float SizeSmoothing = 0.02f;
    public float SizeMin = 5f;
    public float SizeMax = 0f;
    public float SizeBorder = 0f;

    public float DepthMin = 10f;
    public float DepthOffset = 4f;
    public float DepthSizeFactor = 0.4f;

    public List<Transform> following = new List<Transform>() {
        null
    };

    new Transform transform;
    Camera cam;


    void Start() {
        transform = GetComponent<Transform>();
        cam = GetComponent<Camera>();
        if (SizeMax == 0f) {
            SizeMax = float.MaxValue;
        }
    }

    void FixedUpdate() {
        if (string.IsNullOrEmpty(FollowingTag)) {
            following = Following;
        } else {
            GameObject[] tagged = GameObject.FindGameObjectsWithTag(FollowingTag);
            following.Clear();
            foreach (GameObject obj in tagged) {
                following.Add(obj.GetComponent<Transform>());
            }
        }

        Vector3 diff = Vector3.zero;
        float depthMin = DepthMin;
        foreach (Transform follow in following) {
            diff += follow.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            if (follow.position.z < depthMin) {
                depthMin = follow.position.z;
            }
        }
        depthMin -= DepthOffset;
        diff.z = depthMin - transform.position.z;
        transform.position = transform.position + diff * PositionSmoothing;

        float size = 0f;

        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        foreach (Transform follow in following) {
            if (follow.position.x < min.x) {
                min.x = follow.position.x;
            }
            if (follow.position.y < min.y) {
                min.y = follow.position.y;
            }
            if (max.x < follow.position.x) {
                max.x = follow.position.x;
            }
            if (max.y < follow.position.y) {
                max.y = follow.position.y;
            }
        }

        size = (max - min).magnitude;

        size += SizeBorder;
        size = Mathf.Max(SizeMin, Mathf.Min(SizeMax, size));

        cam.orthographicSize += (size - cam.orthographicSize) * SizeSmoothing;
        if (!cam.orthographic) {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - cam.orthographicSize * DepthSizeFactor);
        }

    }

    float GetHighest(Vector3 v) {
        Vector3 av = new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        if (av.y < av.x && av.z < av.x) {
            return v.x;
        }
        if (av.z < av.y && av.x < av.y) {
            return v.y;
        }
        if (av.x < av.z && av.y < av.z) {
            return v.z;
        }
        return Mathf.Max(v.x, v.y, v.z);
    }
}
