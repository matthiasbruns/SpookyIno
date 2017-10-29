using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective {
    public string objectiveName = null;
    public int objectiveItemId;

    public override bool Equals(object obj) {
        if (obj is Objective) {
            return this == (Objective)obj;
        }
        return base.Equals(obj);
    }

    public static bool operator ==(Objective a, Objective b) {
        if (((object)a == null && string.IsNullOrEmpty(b.objectiveName)) ||
            ((object)b == null && string.IsNullOrEmpty(a.objectiveName))) {
            return true;
        }

        return ReferenceEquals(a, b);
    }

    public static bool operator !=(Objective a, Objective b) {
        return !(a == b);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

}
