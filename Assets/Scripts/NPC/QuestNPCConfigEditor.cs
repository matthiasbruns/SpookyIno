using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class QuestNPCEditor : EditorWindow {
    
    public QuestNPCConfig config;
    
    [MenuItem ("Window/Quests/NPC")]
    static void  Init () 
    {
        EditorWindow.GetWindow (typeof (QuestNPCEditor));
    }
    
    void  OnEnable () {
        if(EditorPrefs.HasKey("ObjectPath")) 
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            config = AssetDatabase.LoadAssetAtPath (objectPath, typeof(QuestNPCConfig)) as QuestNPCConfig;
        }
        
    }
    
    void  OnGUI () {
        GUILayout.BeginHorizontal ();
        GUILayout.Label ("Quest NPC Editor", EditorStyles.boldLabel);

        if (config != null) {
            if (GUILayout.Button("Show Quest NPC")) 
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = config;
            }
            if (GUILayout.Button("Open Quest NPC")) 
            {
                    OpenQuestNPC();
            }
            if (GUILayout.Button("New Quest NPC")) 
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = config;
            }
        }
        GUILayout.EndHorizontal ();
        
        if (config == null) 
        {
            GUILayout.BeginHorizontal ();
            GUILayout.Space(10);
            if (GUILayout.Button("Create Quest NPC", GUILayout.ExpandWidth(false))) 
            {
                CreateNewQuestNPC();
            }
            if (GUILayout.Button("Open Existing Quest NPC", GUILayout.ExpandWidth(false))) 
            {
                OpenQuestNPC();
            }
            GUILayout.EndHorizontal ();
        }
            
        if (config != null) {

            GUILayout.BeginVertical ();
            config.questId = EditorGUILayout.IntField ("Quest ID", config.questId);
            config.questText = EditorGUILayout.TextField("Quest Text", config.questText as string);
            config.questSuccessText = EditorGUILayout.TextField("Quest Success Text", config.questSuccessText as string);
            GUILayout.EndVertical ();

            if (GUI.changed) 
            {
                EditorUtility.SetDirty(config);
            }
        }
    }
    
    void CreateNewQuestNPC () 
    {
        config = CreateQuestNPCConfig.Create();
        if (config) 
        {
            string relPath = AssetDatabase.GetAssetPath(config);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }
    
    void OpenQuestNPC () 
    {
        string absPath = EditorUtility.OpenFilePanel ("Select Quest NPC", "", "");
        if (absPath.StartsWith(Application.dataPath)) 
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            config = AssetDatabase.LoadAssetAtPath (relPath, typeof(QuestNPCConfig)) as QuestNPCConfig;
            if (config) {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }
}