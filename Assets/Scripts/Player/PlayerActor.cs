using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, IActor, DeathHandler {

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
        
        float angleLook = (float)((Mathf.Atan2(LookAngle.x, LookAngle.y) / Mathf.PI) * 180f) + 45f;
        if (angleLook < 0) angleLook += 360f;

        anim.SetFloat("Velocity", Own.velocity.magnitude);
        anim.SetFloat("Angle", angleLook);
    }

    public void HandleDeath() {
        GameSceneManager.Instance.ExitOrRetry();
    }

}