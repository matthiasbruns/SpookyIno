using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelUIComponent : MonoBehaviour {

    RectTransform rectTransform;

    public EquipmentComponent Equipment;
    public List<Image> Icons = new List<Image>();

    void Awake() {
        rectTransform = GetComponent<RectTransform>();

        // Whoops I did it again.
        if (Equipment == null)
            Equipment = FindObjectOfType<EquipmentComponent>();
        if ((Icons?.Count ?? 0) == 0)
            transform.GetComponentsInChildren(Icons);
        Icons.Remove(GetComponent<Image>());
    }

    void Update() {
        for (int i = 0; i < Icons.Count; i++) {
            Image icon = Icons[i];
            InventoryItem item = Equipment.Items[i];
            if (icon.enabled = (item != null)) {
                icon.sprite = item.itemIcon;
            }
        }

    }

}
