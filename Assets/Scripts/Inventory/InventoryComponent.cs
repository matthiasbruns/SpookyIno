using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour {
	public InventoryItemList database;

	private List<InventorySlot> slots;

	void Awake(){
		slots = new List<InventorySlot>();
		database = GameManager.Instance.itemDatabase;
	}

	// UNITY
	void OnTriggerEnter2D(Collider2D other) {
		var itemOwner = other.gameObject.GetComponent<ItemComponent>() as HasInventoryItem;
        if(itemOwner != null){
			AddItem(itemOwner.ItemId, itemOwner.Amount);
			GameObject.Destroy(other.gameObject);
		}
    }

	// INVENTORY
	private InventorySlot GetSlot(InventoryItem item){
		foreach(InventorySlot slot in slots){
			if(slot.item?.itemName == item.itemName) {
				return slot;
			}
		}

		return null;
	}

	private bool ShouldStackItem(InventorySlot slot, int amount){
		if(slot == null || slot.item ==null){
			return false;
		}
		
		return slot.item.isStackable && slot.item.encumbranceValue < slot.amount + amount;
	}

	public void AddItem(int itemId, int amount = 1){
		InventoryItem item = database.itemList[itemId];
		AddItem(item, amount);
	}

	public void AddItem(InventoryItem item, int amount = 1){
		var slot = GetSlot(item);
		// We need to check, if we have to change an existing slot
		if(ShouldStackItem(slot, amount)){
			// Stack item
			slot.amount += amount;
		} else {
			// Create new ItemSlot
			slots.Add(new InventorySlot(item, amount));
		}
	}
}