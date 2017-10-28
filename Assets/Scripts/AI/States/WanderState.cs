using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : AiState {
    public float minDistance = float.Epsilon;
    private bool targetArrived = false;
    private Transform target;
    private bool waiting = false;
    private float waitDuration = 2.0f;
    private float waitTimer = 0;
    
    public override void Tick(GameObject owner)
    {
        if(!waiting){
            Wander(owner);
        }
    }

    void Wander(GameObject owner){
        if(target == null) return;

        if(Vector2.Distance(target.transform.position.vec2(), target.position.vec2()) <= minDistance) {
            // Target arrived
            targetArrived = true;
        } else {
            targetArrived = false;

            List<HasMovementAi> movementAIs;
            owner.GetInterfaces<HasMovementAi>(out movementAIs);
            if(movementAIs == null || movementAIs.Count == 0) {
                Debug.LogError("Cannot wander without HasMovementAi interface");
            } else {
                HasMovementAi movement = movementAIs[0];
                movement.Target = target.transform;
                waitTimer = waitDuration;
            }
        }

        isTransitionAllowed = targetArrived;

        waitTimer -= Time.deltaTime;
        waiting = waitTimer <= 0.0f;
        if(waitTimer < 0.0f) waitTimer = 0.0f;

    }
}