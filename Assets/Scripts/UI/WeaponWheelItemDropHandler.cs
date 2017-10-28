using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponWheelItemDropHandler : MonoBehaviour, IUIItemDropHandler {

    public EquipmentComponent Equipment;
    public int Index = -1;

    void Awake() {
        if (Equipment == null)
            Equipment = FindObjectOfType<EquipmentComponent>();
        if (Index == -1) {
            for (int i = 0; i < transform.parent.childCount; i++) {
                if (transform.parent.GetChild(i) == transform) {
                    Index = i;
                    break;
                }
            }
        }
    }

    void Update() {

    }

    public void OnDrop(InventoryUIItem item, PointerEventData eventData) {
        for (int i = 0; i < Equipment.Items.Length; i++) {
            if (Equipment.Items[i] == item.Item) {
                Equipment.Items[i] = null;
                break;
            }
        }
        Equipment.Items[Index] = item.Item;
    }

}
