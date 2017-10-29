#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateSoundList {
    [MenuItem("Assets/Create/Sounds/Sound List")]
    public static SoundList Create() {
        SoundList asset = ScriptableObject.CreateInstance<SoundList>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Sounds/SoundList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
#endif