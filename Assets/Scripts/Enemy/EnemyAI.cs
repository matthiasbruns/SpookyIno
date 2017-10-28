using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : AiComponent, HasMovementAi {

	public EnemyAiConfig aiConfig;

	private AiState idleState, chaseState, attackState;

	public AILerp movementAi;

	public float pathUpdateDuration = 5.0f;
	private float pathUpdateTimer = 0f;

    public Transform Target { 
		get {
			return movementAi.target; 
		} 
		set {
			movementAi.target = value;
		} 
	}

    public bool PathCompleted => movementAi.targetReached || !movementAi.canMove;

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
		idleState = Instantiate(aiConfig.idleState, Vector3.zero, Quaternion.identity);
		idleState.transform.parent = gameObject.transform;
		idleState.transform.position = Vector3.zero;

		chaseState = Instantiate(aiConfig.chaseState, Vector3.zero, Quaternion.identity);
		chaseState.transform.parent = gameObject.transform;
		chaseState.transform.position = Vector3.zero;

		attackState = Instantiate(aiConfig.attackState, Vector3.zero, Quaternion.identity);
		attackState.transform.parent = gameObject.transform;
		attackState.transform.position = Vector3.zero;

		initialState = idleState;
		idleState.nextState = chaseState;
		chaseState.previousState = idleState;
		chaseState.nextState = attackState;
		attackState.previousState = chaseState;
		attackState.nextState = initialState;
	}

	void LateUpdate(){
		pathUpdateTimer -= Time.deltaTime;

		if(movementAi.target != null && movementAi.enabled){
			if(pathUpdateTimer <= 0){
				pathUpdateTimer = pathUpdateDuration;
				// movementAi.SearchPath();
			}	
		}
	}
}