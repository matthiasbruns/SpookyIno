using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateEnemyAiConfig {
    public static EnemyAiConfig Create(string fileName = "EnemyConfig")
    {
        EnemyAiConfig asset = ScriptableObject.CreateInstance<EnemyAiConfig>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/AI/Config/" + fileName + ".asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}