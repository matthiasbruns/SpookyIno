using UnityEngine;

public class Quest0Condition : MonoBehaviour, PreCondition
{
    public virtual bool Check(GameObject owner)
    {
        return true;
    }
}