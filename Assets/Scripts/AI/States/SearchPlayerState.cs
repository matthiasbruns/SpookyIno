using System.Collections;
using UnityEngine;

public class SearchPlayerState : AiState
{
    public float searchRadius = 10;
    private GameObject player;

    public override void OnEnter(GameObject owner){
        base.OnEnter(owner);
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        
    }

    public override void Tick(GameObject owner)
    {   
        if(Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) <= searchRadius){
            isTransitionAllowed = true;
        }else {
            isTransitionAllowed = false;
        }
    }
}