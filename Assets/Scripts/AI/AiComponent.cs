using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiComponent : MonoBehaviour {
	
	public AiState initialState;
    protected AiState currentState;
    
	void Update(){
		if(currentState != null){
			currentState.Tick(gameObject);
			if(currentState.IsTransisionAllowed()){
				// TriggerTransition();
			}
		}
	}
    protected void TriggerTransition(AiState next){
        // TODO
    }
}