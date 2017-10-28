using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, IActor {

    public Transform hand;

    public Vector2 LookAngle => hand.forward;
    Animator anim; 

    void Awake() {
        if (hand == null)
            hand = transform.Find("Hand"); // Jemand war faul und hat hand nicht gesetzt..
        anim = GetComponent<Animator>();
        anim.SetFloat("LookDirectionX", LookAngle.x);
        anim.SetFloat("LookDirectionY", LookAngle.y);
        anim.SetFloat("Velocity", GetComponent<Rigidbody2D>().velocity.magnitude);
    }
	
	void Update() {
        anim.SetFloat("LookDirectionX", LookAngle.x);
        anim.SetFloat("LookDirectionY", LookAngle.y);
        anim.SetFloat("Velocity", GetComponent<Rigidbody2D>().velocity.magnitude);

    }

}
