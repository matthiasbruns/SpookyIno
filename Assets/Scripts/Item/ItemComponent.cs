using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour, HasInventoryItem {

	[SerializeField]
	private int amount = 1;
    public int Amount => amount;
	
	[SerializeField]
	private int item;
    public int ItemId => item;
}
