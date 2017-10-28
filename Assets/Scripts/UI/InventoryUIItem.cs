using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryUIItem : MonoBehaviour {

    public InventorySlot Slot;

    private Image icon;

    void Awake() {
        icon = GetComponent<Image>();
    }

    void Update() {
        if (icon.enabled = (Slot != null && Slot.item != null)) {
            icon.sprite = Slot.item.itemIcon;
        }
    }
}
