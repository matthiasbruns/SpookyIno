using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

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

    private bool dragging = false;
    private Transform preDragParent;
    private Vector3 preDragPosition;

    public void OnBeginDrag(PointerEventData eventData) {
        if (Slot == null || Slot.item == null)
            return;

        dragging = true;
        preDragParent = transform.parent;
        preDragPosition = transform.localPosition;
        while (transform.parent.GetComponent<Canvas>() == null)
            transform.parent = transform.parent.parent;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!dragging)
            return;

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!dragging)
            return;

        transform.parent = preDragParent;
        transform.localPosition = preDragPosition;
        dragging = false;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results) {
            IUIItemDropHandler handler = result.gameObject.GetComponent<IUIItemDropHandler>();
            if (handler != null) {
                handler.OnDrop(this, eventData);
                break;
            }
        }

    }

}
