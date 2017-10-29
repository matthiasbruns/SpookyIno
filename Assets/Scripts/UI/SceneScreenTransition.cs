using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScreenTransition : MonoBehaviour {

    public Vector2 From = new Vector2(0, 1500);
    public Vector2 To = new Vector2(0, -500);
    public float Duration = 0.5f;

    float _TimeStart;

    void Start() {
        _TimeStart = Time.time;
    }

    void Update() {
        Vector2 xy = Vector2.Lerp(From, To, Mathf.Clamp01((Time.time - _TimeStart) / Duration));
        transform.localPosition = new Vector3(
            xy.x,
            xy.y,
            transform.localPosition.z
        );
    }

}
