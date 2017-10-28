using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundList : ScriptableObject {
    public List<Sound> soundList;

    public AudioClip GetSoundByName(string name) {
        foreach (Sound sound in soundList) {
            if(sound.name == name) {
                return sound.clip;
            }
        }
        return null;
    }
}