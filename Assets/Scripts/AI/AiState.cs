using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiState : ScriptableObject {

    public abstract IEnumerator Tick(GameObject owner);

    public abstract bool IsTransisionAllowed();
}