using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiState : ScriptableObject {

    public AiState nextState;
    public AiState previousState;
    protected bool isBackTransitionRequested = false;
    protected bool isTransitionAllowed = false;

    public virtual void OnEnter(GameObject owner){
        isTransitionAllowed = false;
        isBackTransitionRequested = false;
    }

    public abstract void Tick(GameObject owner);

    public bool IsTransisionAllowed() => isTransitionAllowed;

    public bool IsBackTransitionRequested() => isBackTransitionRequested;
}