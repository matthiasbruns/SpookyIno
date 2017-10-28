using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, IActor {

    public Transform hand;

    public Vector2 LookAngle => hand.forward;

    void Awake() {
        if (hand == null)
            hand = transform.Find("Hand"); // Jemand war faul und hat hand nicht gesetzt..

        Animator amin = GetComponent<Animator>();
        amin.SetFloat("LookDirectionX", LookAngle.x);
        amin.SetFloat("LookDirectionY", LookAngle.y);
        amin.SetFloat("Velocity", GetComponent<Rigidbody2D>().velocity.magnitude);
    }
	
	void Update() {

	}

}
