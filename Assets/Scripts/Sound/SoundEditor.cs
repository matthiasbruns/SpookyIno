#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoundEditor : EditorWindow {

    public SoundList soundList;
    private int viewIndex = 1;

    [MenuItem("Window/Sounds/Sound Editor")]
    static void Init() {
        EditorWindow.GetWindow(typeof(SoundEditor));
    }

    void OnEnable() {
        if (EditorPrefs.HasKey("ObjectPath")) {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            soundList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(SoundList)) as SoundList;
        }

    }

    void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Sound Editor", EditorStyles.boldLabel);
        if (soundList != null) {
            if (GUILayout.Button("Show Sound List")) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = soundList;
            }
        }
        if (GUILayout.Button("Open Sound List")) {
            OpenSoundList();
        }
        if (GUILayout.Button("New Sound List")) {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = soundList;
        }
        GUILayout.EndHorizontal();

        if (soundList == null) {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Sound List", GUILayout.ExpandWidth(false))) {
                CreateNewSoundList();
            }
            if (GUILayout.Button("Open Existing Sound List", GUILayout.ExpandWidth(false))) {
                OpenSoundList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (soundList != null) {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false))) {
                if (viewIndex < soundList.soundList.Count) {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Sound", GUILayout.ExpandWidth(false))) {
                AddSound();
            }
            if (GUILayout.Button("Delete Sound", GUILayout.ExpandWidth(false))) {
                DeleteSound(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (soundList.soundList == null)
                Debug.Log("wtf");
            if (soundList.soundList.Count > 0) {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Sound", viewIndex, GUILayout.ExpandWidth(false)), 1, soundList.soundList.Count);
                EditorGUILayout.LabelField("of   " + soundList.soundList.Count.ToString() + "  sounds", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                soundList.soundList[viewIndex - 1].name = EditorGUILayout.TextField("Sound Name", soundList.soundList[viewIndex - 1].name as string);
                soundList.soundList[viewIndex - 1].clip = EditorGUILayout.ObjectField("Sound Clip", soundList.soundList[viewIndex - 1].clip, typeof(AudioClip), false) as AudioClip;
                soundList.soundList[viewIndex - 1].mixer = EditorGUILayout.ObjectField("Audio Mixer Group", soundList.soundList[viewIndex - 1].mixer, typeof(AudioMixerGroup), false) as AudioMixerGroup;

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                soundList.soundList[viewIndex - 1].volume = EditorGUILayout.Slider("Volume", soundList.soundList[viewIndex - 1].volume, 0.0f, 1.0f, GUILayout.ExpandWidth(false));
                soundList.soundList[viewIndex - 1].loop = (bool)EditorGUILayout.Toggle("Loop", soundList.soundList[viewIndex - 1].loop, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            } else {
                GUILayout.Label("This Inventory List is Empty.");
            }
        }
        if (GUI.changed) {
            EditorUtility.SetDirty(soundList);
        }
    }

    void CreateNewSoundList() {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        soundList = CreateSoundList.Create();
        if (soundList) {
            soundList.soundList = new List<Sound>();
            string relPath = AssetDatabase.GetAssetPath(soundList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenSoundList() {
        string absPath = EditorUtility.OpenFilePanel("Select Inventory Item List", "", "");
        if (absPath.StartsWith(Application.dataPath)) {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            soundList = AssetDatabase.LoadAssetAtPath(relPath, typeof(SoundList)) as SoundList;
            if (soundList.soundList == null)
                soundList.soundList = new List<Sound>();
            if (soundList) {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddSound() {
        Sound newSound = new Sound();
        newSound.name = "New Sound";
        soundList.soundList.Add(newSound);
        viewIndex = soundList.soundList.Count;
    }

    void DeleteSound(int index) {
        soundList.soundList.RemoveAt(index);
    }
}
#endif