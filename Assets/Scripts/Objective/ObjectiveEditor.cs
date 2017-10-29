#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveEditor : EditorWindow {
    #if UNITY_EDITOR

    public ObjectiveList objectiveList;
    private int viewIndex = 1;

    [MenuItem("Window/Objective/Objective Editor")]
    static void Init() {
        EditorWindow.GetWindow(typeof(ObjectiveEditor));
    }

    void OnEnable() {
        if (EditorPrefs.HasKey("ObjectPath")) {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            objectiveList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(ObjectiveList)) as ObjectiveList;
        }

    }

    void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Objective Editor", EditorStyles.boldLabel);
        if (objectiveList != null) {
            if (GUILayout.Button("Show Objective List")) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = objectiveList;
            }
        }
        if (GUILayout.Button("Open Objective List")) {
            OpenObjectiveList();
        }
        if (GUILayout.Button("New Objective List")) {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = objectiveList;
        }
        GUILayout.EndHorizontal();

        if (objectiveList == null) {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Objective List", GUILayout.ExpandWidth(false))) {
                CreateNewObjectiveList();
            }
            if (GUILayout.Button("Open Existing Objective List", GUILayout.ExpandWidth(false))) {
                OpenObjectiveList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (objectiveList != null) {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false))) {
                if (viewIndex < objectiveList.objectivesList.Count) {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Objective", GUILayout.ExpandWidth(false))) {
                AddObjective();
            }
            if (GUILayout.Button("Delete Objective", GUILayout.ExpandWidth(false))) {
                DeleteObjective(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (objectiveList.objectivesList == null)
                Debug.Log("wtf");
            if (objectiveList.objectivesList.Count > 0) {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Objective", viewIndex, GUILayout.ExpandWidth(false)), 1, objectiveList.objectivesList.Count);
                EditorGUILayout.LabelField("of   " + objectiveList.objectivesList.Count.ToString() + "  Objective", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                objectiveList.objectivesList[viewIndex - 1].objectiveName = EditorGUILayout.TextField("Objective Name", objectiveList.objectivesList[viewIndex - 1].objectiveName as string);
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                objectiveList.objectivesList[viewIndex - 1].objectiveItemId = EditorGUILayout.IntField("Drop Item ID", objectiveList.objectivesList[viewIndex - 1].objectiveItemId, GUILayout.ExpandWidth(false)); //TODO?
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

            } else {
                GUILayout.Label("This Objective List is Empty.");
            }
        }
        if (GUI.changed) {
            EditorUtility.SetDirty(objectiveList);
        }
    }

    void CreateNewObjectiveList() {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        objectiveList = CreateObjectivesList.Create();
        if (objectiveList) {
            objectiveList.objectivesList = new List<Objective>();
            string relPath = AssetDatabase.GetAssetPath(objectiveList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenObjectiveList() {
        string absPath = EditorUtility.OpenFilePanel("Select Objective Item List", "", "");
        if (absPath.StartsWith(Application.dataPath)) {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            objectiveList = AssetDatabase.LoadAssetAtPath(relPath, typeof(ObjectiveList)) as ObjectiveList;
            if (objectiveList.objectivesList == null)
                objectiveList.objectivesList = new List<Objective>();
            if (objectiveList) {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddObjective() {
        Objective newObjective = new Objective();
        newObjective.objectiveName = "New Objective";
        objectiveList.objectivesList.Add(newObjective);
        viewIndex = objectiveList.objectivesList.Count;
    }

    void DeleteObjective(int index) {
        objectiveList.objectivesList.RemoveAt(index);
    }

    #endif
}
#endif