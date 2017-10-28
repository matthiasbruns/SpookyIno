using System.Collections;
using UnityEngine;

class IdleState : AiState
{
    public override IEnumerator Tick(GameObject owner){
        yield return null;
    }
    
    public override bool IsTransisionAllowed() => true;
}