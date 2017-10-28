using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]  
public class InventorySlot {

	public InventorySlot(){}
	public InventorySlot(InventoryItem item, int amount = 1){
		this.item = item;
		this.amount = amount;
	}

	public int amount;
	public InventoryItem item;


}
