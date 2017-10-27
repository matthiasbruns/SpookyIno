using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour {
	private List<InventorySlot> slots;

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

	public void AddItem(InventoryItem item, int amount = 1){
		if(item.isStackable){
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
}