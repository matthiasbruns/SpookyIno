using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public AudioMixer masterMixer;

    public Sound[] sounds;

    void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Start() {
        Play("MainMenu");
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void MasterVolume(float masterVolume) {
        masterMixer.SetFloat("masterVol", masterVolume);
    }
    public void MusicVolume(float musicVolume) {
        masterMixer.SetFloat("musicVol", musicVolume);
    }
    public void SfxVolume(float sfxVolume) {
        masterMixer.SetFloat("sfxVol", sfxVolume);
    }

    public AudioClip GetSoundByName(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return null;
        return s.clip;
    }
}