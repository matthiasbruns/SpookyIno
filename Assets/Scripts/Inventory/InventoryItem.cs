using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]                                                           //  Our Representation of an InventoryItem
public class InventoryItem 
{
    public string itemName = null;                                      //  What the item will be called in the inventory
    public Sprite itemIcon = null;                                           //  What the item will look like in the inventory
    public Rigidbody2D itemObject = null;                                         //  Optional slot for a PreFab to instantiate when discarding
    public bool isUnique = false;                                               //  Optional checkbox to indicate that there should only be one of these items per game
    public bool isIndestructible = false;                                       //  Optional checkbox to prevent an item from being destroyed by the player (unimplemented)
    public bool isQuestItem = false;     
    public bool isEquipment = false;   
    public bool isWeapon = false;     
    public int weaponSlotSize = 0;                                          //  Examples of additional information that could be held in InventoryItem
    public bool isStackable = false;                                            //  Examples of additional information that could be held in InventoryItem
    public bool destroyOnUse = false;                                           //  Examples of additional information that could be held in InventoryItem
    public float encumbranceValue = 0;   
    public BaseAction action;                                     //  Examples of additional information that could be held in InventoryItem  !!!
    public string pickUpClip = null;

    // Kill me.
    public override bool Equals(object obj) {
        if (obj is InventoryItem) {
            return this == (InventoryItem) obj;
        }
        return base.Equals(obj);
    }

    public static bool operator ==(InventoryItem a, InventoryItem b) {
        if (((object) a == null && (object) b == null) ||
            ((object) a == null && string.IsNullOrEmpty(b.itemName)) ||
            ((object) b == null && string.IsNullOrEmpty(a.itemName))) {
            return true;
        }

        return ReferenceEquals(a, b);
    }

    public static bool operator !=(InventoryItem a, InventoryItem b) {
        return !(a == b);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    private BaseAction executableAction;

    public void execute(GameObject executer) {
        if(executableAction == null){
            if(action != null) {
                executableAction = GameObject.Instantiate(action, Vector3.zero, Quaternion.identity);
                executableAction.transform.parent = executer.transform;
            }
        }
        if(executableAction != null) {
            executableAction.execute(executer);
        }
    }
}
