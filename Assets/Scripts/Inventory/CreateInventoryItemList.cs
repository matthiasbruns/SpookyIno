using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CreateInventoryItemList {
    public static InventoryItemList Create()
    {
        #if UNITY_EDITOR
        InventoryItemList asset = ScriptableObject.CreateInstance<InventoryItemList>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Inventory/InventoryItemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
        #else
        return null;
        #endif
    }
}