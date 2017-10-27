using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementComponent : MonoBehaviour {

    public float Speed = 10f;

    Rigidbody2D body;

	void Start() {
        body = GetComponent<Rigidbody2D>();
        if (body.drag == 0f) {
            body.drag = Speed; // Physikalisch inkorrekt, Spieler hält aber sofort an, fühlt sich besser an.
        }
	}
	
	void Update() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        // Physikalisch inkorrekt, fühlt sich aber "snappier" an.
        if (x != 0)
            body.velocity = new Vector2(
                x * Speed,
                body.velocity.y
            );
        if (y != 0)
            body.velocity = new Vector2(
                body.velocity.x,
                y * Speed
            );
	}

}
