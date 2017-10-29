using UnityEngine;

public class Quest0Condition : ObjectiveCondition
{
    public virtual bool Check(GameObject owner)
    {
        return true;
    }
}