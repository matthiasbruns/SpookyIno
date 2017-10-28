using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentComponent : MonoBehaviour {

	public InventoryItem secondaryHand = null;
	public InventoryItem mainHand = null;

	private InventoryComponent inventory;
	private ActionsComponent actions;

	void Awake(){
		inventory = GetComponent<InventoryComponent>();
		if(inventory == null){
			inventory = gameObject.AddComponent<InventoryComponent>();
		}
		actions = GetComponent<PlayerActionsComponent>();
		if(actions == null){
			actions = gameObject.AddComponent<PlayerActionsComponent>();
		}
	}

	void OnEnable(){
		inventory.OnChanged += OnInventoryChange;
		actions.OnPrimary += OnFire1;
		actions.OnSecondary += OnFire2;
	}

	void OnDisable(){
        inventory.OnChanged -= OnInventoryChange;
		actions.OnPrimary -= OnFire1;
		actions.OnSecondary -= OnFire2;
    }

	void OnFire1(){
		if(mainHand != null){
			mainHand.action?.execute(gameObject);
		}
	}

	void OnFire2(){
		if(secondaryHand != null){
			secondaryHand.action?.execute(gameObject);
		}
	}

	void OnInventoryChange(List<InventorySlot> slots){
        // TODO: Inventory was changed - update equipment here 
		AutoEquip(slots);
    }

	private bool CanEquip(InventoryItem item){
		//TODO: might be complex (check other hand, check item type weapon/shield? single, dual hand?)
		if(!item.isEquipment) return false;

		if (item.isWeapon && (CanEquipWeapon(mainHand) || CanEquipWeapon(secondaryHand))){
			return true;
		}

		return false;
	}

	private bool CanEquipWeapon(InventoryItem slot){
		return slot == null || !slot.isEquipment || !slot.isWeapon;
	}

	private void AutoEquip(List<InventorySlot> slots){
		foreach(InventorySlot slot in slots) {
			var item = slot.item;
			if(CanEquip(item)){
				if (item.isWeapon){
					if(CanEquipWeapon(mainHand)) {
						mainHand = item;	
					} else if(CanEquipWeapon(secondaryHand)) {
						secondaryHand = item;	
					}
				}				
			}
		}
	}
}