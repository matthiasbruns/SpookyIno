using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeAiState : AiState {

    private int currentState = 0;
    public List<AiState> states = new List<AiState>();
    private List<AiState> initiatedStates = new List<AiState>();
    private bool dirty = false;

    public override void OnEnter(GameObject owner) {
        if(initiatedStates.Count == 0 || initiatedStates.Count != states.Count) {
            // Destroy old states
            foreach(AiState state in initiatedStates) {
                Destroy(state);
            }
            initiatedStates.Clear();

            // Spawn new states
            foreach(AiState prefab in states) {
                var state = GameObject.Instantiate(prefab, Vector2.zero, Quaternion.identity);
                state.transform.parent = gameObject.transform;
                initiatedStates.Add(state);
            }
        }
        
        dirty = false;
        currentState = 0;
        initiatedStates[currentState].OnEnter(owner);
    }

    public override void OnExit(GameObject owner){
        base.OnExit(owner);
        initiatedStates[currentState].OnExit(owner);
    }

    public override void Tick(GameObject owner) {
        int prevState = currentState;
        if(ChangeToPrevious(owner)) {
            currentState--;
            dirty = true;
        } else if (ChangeToNext(owner)) {
            currentState++;
            dirty = true;
        }
        if(currentState < 0) {
            currentState = initiatedStates.Count -1;
        } else if(currentState >= initiatedStates.Count) {
            currentState = 0;
        }

        if(dirty){
            initiatedStates[prevState].OnExit(owner);
            initiatedStates[currentState].OnEnter(owner);
            dirty = false;
        }
        initiatedStates[currentState].Tick(owner);
    }

    protected bool ChangeToNext(GameObject owner) {
        return initiatedStates[currentState].IsTransisionAllowed();
    }
    protected bool ChangeToPrevious(GameObject owner) {
        return initiatedStates[currentState].IsBackTransitionRequested();
    }
}