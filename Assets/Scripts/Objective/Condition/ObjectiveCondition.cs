using UnityEngine;

public class ObjectiveCondition : MonoBehaviour, PreCondition
{
    public virtual bool Check(GameObject owner)
    {
        return true;
    }
}