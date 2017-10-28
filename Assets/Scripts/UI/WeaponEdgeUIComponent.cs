using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEdgeUIComponent : MonoBehaviour {

    RectTransform rectTransform;

    public EquipmentComponent Equipment;
    public Image IconLeft;
    public Image IconRight;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();

        // Whoops I did it again.
        if (Equipment == null)
            Equipment = FindObjectOfType<EquipmentComponent>();
        if (IconLeft == null)
            IconLeft = transform.Find("IconLeft").GetComponent<Image>();
        if (IconRight == null)
            IconRight = transform.Find("IconRight").GetComponent<Image>();
    }

    void Update() {
        if (IconLeft.enabled = (Equipment.mainHand != null)) {
            IconLeft.sprite = Equipment.mainHand.itemIcon;
        }
    }

}
