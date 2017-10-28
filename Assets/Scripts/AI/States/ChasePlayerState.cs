using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerState : AiState {
    public float minDistance = 2f;
    private GameObject player;
    private List<HasMovementAi> movementAIs;

    public override void Tick(GameObject owner)
    {   
        if(movementAIs == null){
            owner.GetInterfaces<HasMovementAi>(out movementAIs);
        }

        if (player == null) {
            player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        } else {
            if(movementAIs == null || movementAIs.Count == 0) {
                Debug.LogError("Cannot wander without HasMovementAi interface");
            } else {
                HasMovementAi movement = movementAIs[0];
                movement.Target = player.transform;
            }
        }

        if (Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) <= minDistance) {
            isTransitionAllowed = true;
        } else {
            isTransitionAllowed = false;
        }
    }
}