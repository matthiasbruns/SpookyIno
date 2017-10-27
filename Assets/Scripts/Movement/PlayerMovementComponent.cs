using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementComponent : MonoBehaviour {

    public float Speed = 10f;
    [Range(0f, 1f)]
    public float SpeedDragFactor = 0.5f;

    Rigidbody2D body;

	void Start() {
        body = GetComponent<Rigidbody2D>();
        if (body.drag == 0f) {
            // Physikalisch inkorrekt, Spieler hält aber (fast) sofort an, fühlt sich besser an.
            body.drag = Speed * SpeedDragFactor;
        }
	}
	
	void Update() {
        float x = Input.GetAxis("Horizontal") * Speed;
        float y = Input.GetAxis("Vertical") * Speed;
        // Physikalisch inkorrekt, fühlt sich aber "snappier" an.
        if (x != 0)
            body.velocity = new Vector2(
                x,
                body.velocity.y
            );
        if (y != 0)
            body.velocity = new Vector2(
                body.velocity.x,
                y
            );
        // Physikalisch korrekter.
        // body.AddForce(new Vector2(x, y), ForceMode2D.Force);
	}

}
