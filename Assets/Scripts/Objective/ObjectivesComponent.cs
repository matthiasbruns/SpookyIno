using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesComponent : MonoBehaviour {
    
    public ObjectiveList database;

    public Objective currentObjective;
    public SortedSet<Objective> completedObjectives = new SortedSet<Objective>();

    void Awake() {
        database = GameManager.Instance.objectiveDatabase;
    }

    public int CurrentObjectiveId {
        set {
            Debug.Log("Setting objective " + value, this);
            currentObjective = database.FindById(value);
            if (currentObjective == null) {
                Debug.LogError("Invalid objective id received " + value, this);
            }
            if (completedObjectives.Contains(currentObjective)) {
                Debug.LogError("Completed objective id received " + value, this);
            }
        }
        get {
            if (currentObjective == null) return -1;

            return currentObjective.objectiveItemId;
        }
    }

    public void FinishCurrentQuest(){
        Debug.Log("Finishing objective " + currentObjective.objectiveItemId, this);
        completedObjectives.Add(currentObjective);
        currentObjective = null;
    }
}