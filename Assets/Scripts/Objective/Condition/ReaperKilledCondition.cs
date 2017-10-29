using UnityEngine;

public class ReaperKilledCondition : ObjectiveCondition
{
    public override bool Check(GameObject owner)
    {
        return GameManager.Instance.gameState.HasFlag(Flags.REAPER_KILLED);
    }
}