using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IUIItemDropHandler {

    void OnDrop(InventoryUIItem item, PointerEventData eventData);

}
