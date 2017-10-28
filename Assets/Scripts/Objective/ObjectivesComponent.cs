using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesComponent : MonoBehaviour {
    
    public ObjectiveList database;

    public Objective currentObjective;
    public List<Objective> completedObjectives;

    void Awake() {
        database = GameManager.Instance.objectiveDatabase;
    }
}