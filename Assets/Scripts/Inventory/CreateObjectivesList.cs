using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateObjectivesList {
    [MenuItem("Assets/Create/Objectives/Objective List")]
    public static ObjectiveList Create() {
        ObjectiveList asset = ScriptableObject.CreateInstance<ObjectiveList>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Objectives/ObjectiveList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
