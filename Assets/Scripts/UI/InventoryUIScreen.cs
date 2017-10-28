using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScreen : MonoBehaviour, IUIScreen {

    public Image Darkening;
    public RectTransform Panel;

    public InventoryComponent Inventory;
    public EquipmentComponent Equipment;

    public List<Image> WheelIcons = new List<Image>();

    public RectTransform ItemSlotPrefab;

    public RectTransform WeaponsContent;
    public RectTransform MaterialsContent;

    public bool IsVisible {
        get {
            return Panel.gameObject.activeInHierarchy;
        }
        set {
            Darkening.enabled = value;
            Panel.gameObject.SetActive(value);
        }
    }

    void Awake() {
        if (Darkening == null)
            Darkening = GetComponent<Image>();
        if (Panel == null)
            Panel = (RectTransform) transform.Find("Panel");

        if (Inventory == null)
            Inventory = FindObjectOfType<InventoryComponent>();
        if (Equipment == null)
            Equipment = FindObjectOfType<EquipmentComponent>();

        if ((WheelIcons?.Count ?? 0) == 0)
            Panel.Find("Wheel").GetComponentsInChildren(WheelIcons);
        WheelIcons.Remove(Panel.Find("Wheel").GetComponent<Image>());

        if (WeaponsContent == null)
            WeaponsContent = Panel.Find("Weapons").Find("Content").GetComponent<RectTransform>();
        for (int i = 0; i < Inventory.MaxSlotsWeapons; i++) {
            int x = i % 3;
            int y = i / 3;
            RectTransform slot = Instantiate(ItemSlotPrefab, Vector3.zero, Quaternion.identity, WeaponsContent);
            slot.anchoredPosition = new Vector2(4 + x * slot.sizeDelta.x, -4 - y * slot.sizeDelta.x);
        }

        if (MaterialsContent == null)
            MaterialsContent = Panel.Find("Materials").Find("Scroll View").Find("Viewport").Find("Content").GetComponent<RectTransform>();
        for (int i = 0; i < Inventory.MaxSlotsMaterials; i++) {
            int x = i % 3;
            int y = i / 3;
            RectTransform slot = Instantiate(ItemSlotPrefab, Vector3.zero, Quaternion.identity, MaterialsContent);
            slot.anchoredPosition = new Vector2(4 + x * slot.sizeDelta.x, -4 - y * slot.sizeDelta.x);
        }

    }

    void Update() {

        for (int i = 0; i < WheelIcons.Count; i++) {
            Image icon = WheelIcons[i];
            InventoryItem item = Equipment.Items[i];
            if (icon.enabled = (item != null)) {
                icon.sprite = item.itemIcon;
            }
        }

    }

}
