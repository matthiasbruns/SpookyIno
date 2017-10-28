using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, IActor {

    public Transform hand;

    public Vector2 LookAngle => hand.forward;

    void Start() {
        if (hand == null)
            hand = transform.Find("Hand"); // Jemand war faul und hat hand nicht gesetzt...
    }
	
	void Update() {
		
	}

}
