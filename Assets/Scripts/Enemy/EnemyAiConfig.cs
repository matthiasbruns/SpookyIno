using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAiConfig : ScriptableObject {
	public AiState idleState;
	public AiState wanderState;
	public AiState attackState;
}