using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AiStateEditor : EditorWindow {
    
    private readonly static MethodInfo m_CreateNewEnemyAiState = typeof(AiStateEditor).GetMethod("CreateNewEnemyAiState", BindingFlags.Instance | BindingFlags.NonPublic);

    public AiState aiState;
    public System.Type[] availableStates;

    private bool creating = false;
    
    [MenuItem ("Window/AI/State Editor")]
    static void  Init () 
    {
        EditorWindow.GetWindow (typeof (AiStateEditor));
    }
    
    void  OnEnable () {
        if(EditorPrefs.HasKey("ObjectPath")) 
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            aiState = AssetDatabase.LoadAssetAtPath (objectPath, typeof(AiState)) as AiState;
            availableStates = ReflectionUtils.GetDerivedOfType<AiState>();
        }   
    }

    void  OnGUI () {
        GUILayout.BeginHorizontal ();
        GUILayout.Label ("Enemy AI State Editor", EditorStyles.boldLabel);
        if (GUILayout.Button("Create New ", GUILayout.ExpandWidth(false))) {
            creating = true;
        }

        if (GUILayout.Button("Open Existing", GUILayout.ExpandWidth(false))){
            OpenAiState();
        }

        if (aiState != null) {
            if (GUILayout.Button("Show in Editor")) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = aiState;
            }
        }

        GUILayout.EndHorizontal ();
        GUILayout.Space(20);
            
        if (creating && availableStates != null) {
            GUILayout.BeginHorizontal ();
            GUILayout.Label ("Choose to Create", EditorStyles.boldLabel);

            foreach (System.Type type in availableStates) {
                if (GUILayout.Button(type.Name, GUILayout.ExpandWidth(false))){
                    m_CreateNewEnemyAiState.MakeGenericMethod(type).Invoke(this, new object[0]);
                }
            }

            GUILayout.EndHorizontal ();
            GUILayout.Space(20);
        }    

        if (aiState != null) {
            GUILayout.Space(10);

            if (GUI.changed) {
                EditorUtility.SetDirty(aiState);
            }
        }
    }
    
    void CreateNewEnemyAiState<T> () where T : AiState
    {
        aiState = CreateAiState.Create<T>();
        if (aiState) {
            // TODO create default stuff here
            string relPath = AssetDatabase.GetAssetPath(aiState);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }
    
    void OpenAiState () 
    {
        string absPath = EditorUtility.OpenFilePanel ("Select AI Config", "", "");
        if (absPath.StartsWith(Application.dataPath)) 
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            aiState = AssetDatabase.LoadAssetAtPath (relPath, typeof(AiState)) as AiState;
            if (aiState) {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }
}