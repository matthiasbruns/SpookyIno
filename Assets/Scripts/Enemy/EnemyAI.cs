using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : AiComponent, HasMovementAi {

	public EnemyAiConfig aiConfig;

	public AILerp movementAi;

	public float pathUpdateDuration = 1.0f;
	private float pathUpdateTimer = 0f;

    public Transform Target { 
		get {
			return movementAi.target; 
		} 
		set {
			if(value == null){
				movementAi.enabled = false;
				movementAi.target = null;
			} else {
				if(movementAi.target != value){
					movementAi.target = value;
					movementAi.enabled = true;
				}
			}
		} 
	}

    void Awake(){
		if(movementAi == null){
			movementAi = GetComponent<AILerp>();
			if(movementAi == null){
				movementAi = GetComponentInParent<AILerp>();
			}
		}

		if(aiConfig.idleState == null){
			 Debug.LogError("idleState is not set");
		}
		if(aiConfig.chaseState == null){
			 Debug.LogError("wanderState is not set");
		}
		if(aiConfig.attackState == null){
			 Debug.LogError("attackState is not set");
		}

		// Setup graph
		initialState = aiConfig.idleState;
		aiConfig.idleState.nextState = aiConfig.chaseState;
		aiConfig.chaseState.previousState = aiConfig.idleState;
		aiConfig.chaseState.nextState = aiConfig.attackState;
		aiConfig.attackState.previousState = aiConfig.chaseState;
		aiConfig.attackState.nextState = initialState;
	}

	void LateUpdate(){
		pathUpdateTimer -= Time.deltaTime;

		if(movementAi.target != null && movementAi.enabled){
			if(pathUpdateTimer <= 0){
				pathUpdateTimer = pathUpdateDuration;
				movementAi.SearchPath();
			}	
		}
	}
}