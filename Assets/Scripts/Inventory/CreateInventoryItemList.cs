using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateInventoryItemList {
    public static InventoryItemList Create()
    {
        InventoryItemList asset = ScriptableObject.CreateInstance<InventoryItemList>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Inventory/InventoryItemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}