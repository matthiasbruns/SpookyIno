using UnityEngine;

public class HasWeaponCondition : ObjectiveCondition
{
    public override bool Check(GameObject executer)
    {
        var inventory = executer.GetComponent<InventoryComponent>();
        return inventory.HasWeapons();
    }
}