using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveList : ScriptableObject {

    public List<Objective> objectivesList;
    public Objective FindById(int id) {
        foreach(Objective objective  in objectivesList) {
            if(objective.objectiveItemId == id) {
                return objective;
            }
        }
        return null;
    }
}
