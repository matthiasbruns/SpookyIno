using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateAiState {
    public static T Create<T>(string name = "")  where T : AiState
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string fileName =  typeof(T).Name;
        if(name != null && name.Length > 0){
            fileName += "_" + name;
        }
        AssetDatabase.CreateAsset(asset, "Assets/Data/AI/State/" + fileName + ".asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}