#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateQuestNPCConfig {
    public static QuestNPCConfig Create()
    {
        QuestNPCConfig asset = ScriptableObject.CreateInstance<QuestNPCConfig>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/NPCs/QuestConfig.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
#endif