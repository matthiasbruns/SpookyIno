using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiState : ScriptableObject {

    private AiState nextState;
    protected bool isTransitionAllowed = false;
    public AiState NextState{
        get{
            return nextState;
        } 
        set{
            nextState = value;
        }
     }

    public abstract void Tick(GameObject owner);

    public bool IsTransisionAllowed() => isTransitionAllowed;
}