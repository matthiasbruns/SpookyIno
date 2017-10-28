using System.Collections;
using UnityEngine;
using UnityEditor;

public class SearchPlayerState : AiState
{
    public float searchRadius = 10;
    public float wanderRadius = 5f;
    public float wanderMinDistance = 1f;
	public GameObject targetPrefab;
    private Vector2 startPosition;
    private GameObject player;
    private HasMovementAi movementAi;
    private GameObject wanderTarget;

    public override void OnEnter(GameObject owner){
        base.OnEnter(owner);
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        movementAi = owner.GetComponent<HasMovementAi>();

        if(startPosition == Vector2.zero){
            startPosition = owner.transform.position;
        }

		#if UNITY_EDITOR		
		if(targetPrefab == null){
			targetPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AI/Target.prefab", typeof(GameObject)) as GameObject;
		}
		#endif
    }

    public override void Tick(GameObject owner)
    {   
        if(Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) <= searchRadius){
            isTransitionAllowed = true;
        }else {
            isTransitionAllowed = false;
        }

        // Generate a new target
        if(wanderTarget == null || 
            movementAi.PathCompleted ||
            Vector2.Distance(owner.transform.position.vec2(), wanderTarget.transform.position.vec2()) <= wanderMinDistance) {
            var destination = new Vector2(
                        Random.Range(startPosition.x + wanderRadius, startPosition.x - wanderRadius),
                        Random.Range(startPosition.y + wanderRadius, startPosition.y - wanderRadius)
                    );

            // A random wandering target
            if(wanderTarget == null){
                wanderTarget = Instantiate(targetPrefab,destination,Quaternion.identity);
            } else {
                wanderTarget.transform.position = destination;
            }

            movementAi.Target = wanderTarget.transform;
        }
    }
}