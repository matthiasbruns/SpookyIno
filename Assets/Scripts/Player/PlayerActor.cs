using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, IActor {

    public Transform hand;

    public Vector2 LookAngle => hand.forward;
    Animator anim;
    Rigidbody2D Own;

    void Awake() {
        if (hand == null)
            hand = transform.Find("Hand"); // Jemand war faul und hat hand nicht gesetzt..
        anim = gameObject.GetComponent<Animator>();
        Own = gameObject.GetComponent<Rigidbody2D>();
    }
	
	void Update() {

        Vector2 fromVector2 = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 toVector2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (cross.z > 0)
            ang = 360 - ang;
        float something = Own.velocity.magnitude;
        anim.SetFloat("Velocity", something);
        anim.SetFloat("Angle", ang);
    }
}
