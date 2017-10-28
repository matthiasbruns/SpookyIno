using System.Collections;
using UnityEngine;

public class SearchPlayerState : AiState
{
    public float searchRadius = 10;
    private GameObject player;

    public override void Tick(GameObject owner)
    {   
        if(player == null){
            player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        }

        if(Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) <= searchRadius){
            isTransitionAllowed = true;
        }else {
            isTransitionAllowed = false;
        }
    }
}