using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : AiComponent {

	public EnemyAiConfig aiConfig;

	private NavMeshAgent navMeshAgent;

	void Awake(){
		navMeshAgent = GetComponent<NavMeshAgent>();
		if(aiConfig.idleState == null){
			 Debug.LogError("idleState is not set");
		}
		if(aiConfig.wanderState == null){
			 Debug.LogError("wanderState is not set");
		}
		if(aiConfig.attackState == null){
			 Debug.LogError("attackState is not set");
		}

		initialState = aiConfig.idleState;
	}
}