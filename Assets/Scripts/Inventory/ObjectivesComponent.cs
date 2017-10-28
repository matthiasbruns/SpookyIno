using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesComponent : MonoBehaviour {

    public delegate void ChangeAction(InventoryItem item, List<InventorySlot> slots);
    public event ChangeAction OnChanged;
    public ObjectiveList database;
    public InventoryItemList iDatabase;

    void Awake() {
        database = GameManager.Instance.objectiveDatabase;
        iDatabase = GameManager.Instance.itemDatabase;
    }

    void OnTriggerEnter2D(Collider2D other) {
        var itemOwner = other.gameObject.GetComponent<ItemComponent>() as HasInventoryItem;
        if (itemOwner != null) {
            AddItem(itemOwner.ItemId, itemOwner.Amount);
            GameObject.Destroy(other.gameObject);
        }
    }

    public void AddItem(int itemId, int amount = 1, ) {
        InventoryItem item = iDatabase.itemList[database.objectivesList[itemId].objectiveItemId];
        //give item to player item list
    }
}