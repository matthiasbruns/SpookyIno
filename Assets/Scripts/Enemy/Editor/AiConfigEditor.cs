using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AiConfigEditor : EditorWindow {
    
    public EnemyAiConfig aiConfig;
    
    [MenuItem ("Window/AI/Config Editor")]
    static void  Init () 
    {
        EditorWindow.GetWindow (typeof (AiConfigEditor));
    }
    
    void  OnEnable () {
        if(EditorPrefs.HasKey("ObjectPath")) 
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            aiConfig = AssetDatabase.LoadAssetAtPath (objectPath, typeof(EnemyAiConfig)) as EnemyAiConfig;
        }
        
    }
    void  OnGUI () {
        GUILayout.BeginHorizontal ();
        GUILayout.Label ("Enemy AI Config Editor", EditorStyles.boldLabel);
        if (GUILayout.Button("Create New ", GUILayout.ExpandWidth(false))) {
            CreateNewEnemyAiConfig();
        }

        if (GUILayout.Button("Open Existing", GUILayout.ExpandWidth(false))){
            OpenAiConfig();
        }

        if (aiConfig != null) {
            if (GUILayout.Button("Show in Editor")) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = aiConfig;
            }
        }

        GUILayout.EndHorizontal ();
        GUILayout.Space(20);
            
        if (aiConfig != null) {
                aiConfig.idleState = EditorGUILayout.ObjectField ("Idle State", aiConfig.idleState, typeof (AiState), false) as AiState;
                aiConfig.chaseState = EditorGUILayout.ObjectField ("Wander State", aiConfig.chaseState, typeof (AiState), false) as AiState;
                aiConfig.attackState = EditorGUILayout.ObjectField ("Attack State", aiConfig.attackState, typeof (AiState), false) as AiState;
                GUILayout.Space(10);
        }

        if (GUI.changed) {
            EditorUtility.SetDirty(aiConfig);
        }
    }
    
    void CreateNewEnemyAiConfig () 
    {
        aiConfig = CreateEnemyAiConfig.Create();
        if (aiConfig) {
            // TODO create default stuff here
            string relPath = AssetDatabase.GetAssetPath(aiConfig);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }
    
    void OpenAiConfig () 
    {
        string absPath = EditorUtility.OpenFilePanel ("Select AI Config", "", "");
        if (absPath.StartsWith(Application.dataPath)) 
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            aiConfig = AssetDatabase.LoadAssetAtPath (relPath, typeof(EnemyAiConfig)) as EnemyAiConfig;
            if (aiConfig) {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }
}