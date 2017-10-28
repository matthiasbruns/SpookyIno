using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerState : AiState {
    public float minDistance = 2f;
    public float maxDistance = 5f;
    private GameObject player;
    private HasMovementAi movementAI;

    public override void OnEnter(GameObject owner) {
        base.OnEnter(owner);
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        List<HasMovementAi> movementAIs;
        owner.GetInterfaces<HasMovementAi>(out movementAIs);

        if(movementAIs == null || movementAIs.Count == 0) {
            Debug.LogError("Cannot wander without HasMovementAi interface");
        } else {
            movementAI = movementAIs[0];
            movementAI.Target = player.transform;
        }
    }

    public override void Tick(GameObject owner) {   
        if (player != null) {
            if (Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) <= minDistance) {
                isTransitionAllowed = true;
                isBackTransitionRequested = false;
            } else if (Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) >= maxDistance) {
                movementAI.Target = null;
                isTransitionAllowed = false;
                isBackTransitionRequested = true;
            } else {
                isTransitionAllowed = false;
                isBackTransitionRequested = false;
            }
        }
    }
}