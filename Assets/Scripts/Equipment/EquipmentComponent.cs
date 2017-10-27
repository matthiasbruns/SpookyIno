using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentComponent : MonoBehaviour {

	private List<ItemComponent> weaponShieldComponents = new List<ItemComponent>();

	private InventoryComponent inventory;
	private ItemComponent leftHand;
	private ItemComponent rightHand;

	void Awake(){
		weaponShieldComponents.Clear();
		weaponShieldComponents.Add(leftHand);
		weaponShieldComponents.Add(rightHand);

		inventory = GetComponent<InventoryComponent>();
		if(inventory == null){
			inventory = gameObject.AddComponent<InventoryComponent>();
		}
	}

	void OnEnable(){
		inventory.OnChanged += OnInventoryChange;
	}

	void OnDisable(){
        inventory.OnChanged -= OnInventoryChange;
    }

	void OnInventoryChange(List<InventorySlot> slots){
        // TODO: Inventory was changed - update equipment here 
		AutoEquip(slots, leftHand);
    }

	private bool CanEquip(InventoryItem item, ItemComponent target){
		//TODO: might be complex (check other hand, check item type weapon/shield? single, dual hand?)
		if(!item.isEquipment) return false;

		if (item.isWeapon && weaponShieldComponents.Contains(target)){
			return true;
		}

		return false;
	}

	private void AutoEquip(List<InventorySlot> slots, ItemComponent target){
		if(leftHand != null) return;

		foreach(InventorySlot slot in slots) {
			var item = slot.item;
			CanEquip(item, target);
		}
	}
}