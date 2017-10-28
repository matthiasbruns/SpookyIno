using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class WanderState : AiState
{
    public float minDistance = float.Epsilon;
    private bool targetArrived = false;
    private Transform target;
    private bool waiting = false;
    private float waitDuration = 2.0f;

    public override bool IsTransisionAllowed() => targetArrived;
    
    public override IEnumerator Tick(GameObject owner)
    {
        if(waiting){
            yield return null; 
        } else {
            yield return Wander(owner);
        }
    }

    private void DrawPath(NavMeshAgent navMeshAgent, Vector3 position, Vector3 destination){
        var path = navMeshAgent.path;
        var corners = path.corners;

        if (corners.Length <= 0) {
            return;
        }

        Debug.DrawLine (position, path.corners [0], Color.red);

        for (int i = 0; i < path.corners.Length -1; i++){
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }

        Debug.DrawLine (path.corners [path.corners.Length -1], destination, Color.red);
    }

    IEnumerator Wander(GameObject owner){
        if(target == null) yield return null;

        if(Vector2.Distance(target.transform.position.vec2(), target.position.vec2()) <= minDistance) {
            // Target arrived
            targetArrived = true;
        } else {
            targetArrived = false;

            var agent = owner.GetComponent<NavMeshAgent>();
            agent.destination = target.transform.position;

            DrawPath(agent, owner.transform.position, target.position);
        }

        waiting = true;
        yield return new WaitForSeconds(waitDuration);
        waiting = false;
    }
}