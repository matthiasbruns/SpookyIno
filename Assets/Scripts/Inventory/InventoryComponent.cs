using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InventoryComponent : MonoBehaviour {

 	public delegate void ChangeAction(InventoryItem item, List<InventorySlot> slots);
    public event ChangeAction OnChanged;

	public InventoryItemList database;

	private List<InventorySlot> slotsWeapons = new List<InventorySlot>();
    public int MaxSlotsWeapons = 6;
    private List<InventorySlot> slotsMaterials = new List<InventorySlot>();
    public int MaxSlotsMaterials = 15;
    Vector3 audioPosition;
    SoundList audioManager;

    void Awake(){
		database = GameManager.Instance.itemDatabase;
        audioPosition = gameObject.transform.position;
        audioManager = GameManager.Instance.audioDatabase;
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
		foreach(InventorySlot slot in slotsWeapons){
			if(slot.item?.itemName == item.itemName) {
				return slot;
			}
		}

        foreach (InventorySlot slot in slotsMaterials) {
            if (slot.item?.itemName == item.itemName) {
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
        AudioClip clip = audioManager.GetSoundByName(item.pickUpClip);
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, audioPosition);
		AddItem(item, amount);
	}

	public void AddItem(InventoryItem item, int amount = 1){
		var slot = GetSlot(item);
        var slots = item.isWeapon ? slotsWeapons : slotsMaterials;
        var slotsMax = item.isWeapon ? MaxSlotsWeapons : MaxSlotsMaterials;
        // We need to check, if we have to change an existing slot
        if (ShouldStackItem(slot, amount)){
			// Stack item
			slot.amount += amount;
		} else if (slots.Count < slotsMax) {
            // Create new ItemSlot
            slots.Add(new InventorySlot(item, amount));
		} else {
            // Return prematurely.
            return;
        }

		 if(OnChanged != null) OnChanged(item, slots);
	}

	public bool HasWeapons(){
		return slotsWeapons.Count > 0;
	}
}