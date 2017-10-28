using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentComponent : MonoBehaviour {

	[System.Serializable]
	struct WeaponSlots {

		public InventoryItem leftHand;
		public InventoryItem rightHand;
	}

	private InventoryComponent inventory;
	private ActionsComponent actions;
	private WeaponSlots weaponSlots;

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
		if(weaponSlots.rightHand != null){
			weaponSlots.rightHand.action?.execute(gameObject);
		}
	}

	void OnFire2(){
		if(weaponSlots.leftHand != null){
			weaponSlots.leftHand.action?.execute(gameObject);
		}
	}

	void OnInventoryChange(List<InventorySlot> slots){
        // TODO: Inventory was changed - update equipment here 
		AutoEquip(slots);
    }

	private bool CanEquip(InventoryItem item){
		//TODO: might be complex (check other hand, check item type weapon/shield? single, dual hand?)
		if(!item.isEquipment) return false;

		if (item.isWeapon && (weaponSlots.leftHand == null || weaponSlots.rightHand == null)){
			return true;
		}

		return false;
	}

	private void AutoEquip(List<InventorySlot> slots){
		foreach(InventorySlot slot in slots) {
			var item = slot.item;
			if(CanEquip(item)){
				if (item.isWeapon){
					if(weaponSlots.rightHand == null) {
						weaponSlots.rightHand = item;	
					} else if(weaponSlots.leftHand == null) {
						weaponSlots.leftHand = item;	
					}
				}				
			}
		}
	}
}