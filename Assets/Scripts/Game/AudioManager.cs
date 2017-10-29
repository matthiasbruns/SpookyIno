using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public AudioMixer masterMixer;

    public AudioClip menuClip;
    public AudioClip[] dungeonClips;
    public AudioClip ghoulClip;
    public AudioClip reaperClip;
    public AudioClip[] scarecrowClips;
    public AudioClip wailClip;
    public AudioClip leavesClip;
    public AudioClip[] rainClips;
    public AudioClip[] thunderClips;
    public AudioClip[] windClips;
    public AudioClip waterDropClip;
    public AudioClip[] dirtWalkClips;
    public AudioClip[] stepClips;

    public AudioMixerGroup music;
    public AudioMixerGroup sfx;
    public AudioSource musicSource;
    public AudioSource atmoSource;

    SoundList database;
    int walkIndex = 0;
    bool isStopped = false;


    void Awake() {
        database = GameManager.Instance.audioDatabase;
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
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

    public void PlayMenuSound() {
        musicSource.clip = menuClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayDungeonSound() {
        StartCoroutine(StartDungeonMusic());
        musicSource.Play();
    }

    IEnumerator StartDungeonMusic() {
        if (musicSource.clip != dungeonClips[0]) {
            musicSource.clip = dungeonClips[0];
            musicSource.loop = false;
            musicSource.Play();
            yield return new WaitForSeconds(musicSource.clip.length-0.5f);
            musicSource.clip = dungeonClips[1];
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    
    public void PlayGhoulSound() {
        musicSource.clip = ghoulClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayReaperSound() {
        musicSource.clip = reaperClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayScarecrowSound() {
        StartCoroutine(StartScarecrowMusic());
        musicSource.Play();
    }

    IEnumerator StartScarecrowMusic() {
        if (musicSource.clip != scarecrowClips[0]) {
            musicSource.clip = scarecrowClips[0];
            musicSource.loop = false;
            musicSource.Play();
            yield return new WaitForSeconds(musicSource.clip.length - 0.5f);
            musicSource.clip = scarecrowClips[1];
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayWailSound() {
        musicSource.clip = wailClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void ChangeMusicSound() {
        musicSource.Stop();
    }

    public void PlayLeaves() {
        atmoSource.clip = leavesClip;
        atmoSource.Play();
    }

    public void PlayRain() {
        int selector = UnityEngine.Random.Range(0, 2);
        atmoSource.clip = rainClips[selector];
        atmoSource.Play();
    }

    public void PlayThunder() {
        int selector = UnityEngine.Random.Range(0, 3);
        atmoSource.clip = thunderClips[selector];
        atmoSource.Play();
    }

    public void PlayWind() {
        int selector = UnityEngine.Random.Range(0, 3);
        atmoSource.clip = windClips[selector];
        atmoSource.Play();
    }

    public void PlayWaterDrops() {
        atmoSource.clip = waterDropClip;
        atmoSource.Play();
    }

    public void ChangeAtmoSound(int selector) {
        atmoSource.loop = true;
        if (atmoSource.clip.length - atmoSource.time <= 0.5f) {
            atmoSource.Stop();
            switch (selector) {
                case 4:
                    PlayWind();
                    break;
                case 3:
                    PlayThunder();
                    break;
                case 2:
                    PlayRain();
                    break;
                case 1:
                    PlayLeaves();
                    break;
                default:
                    print("Incorrect selector");
                    break;
            }
        }
    }


    IEnumerator DirtWalking() {
        atmoSource.clip = dirtWalkClips[walkIndex];
        atmoSource.Play();
        isStopped = true;
        yield return new WaitForSeconds(atmoSource.clip.length);
        walkIndex = (walkIndex + 1) % 4;
        isStopped = false;
    }

    public void PlayWalkDirt() {
        atmoSource.loop = false;
        if(!isStopped)
            StartCoroutine(DirtWalking());
    }

    IEnumerator StoneWalking() {
        atmoSource.clip = stepClips[walkIndex];
        atmoSource.Play();
        isStopped = true;
        yield return new WaitForSeconds(atmoSource.clip.length);
        walkIndex = (walkIndex + 1) % 4;
        isStopped = false;
    }

    public void PlayWalkStone() {
        atmoSource.loop = false;
        if (!isStopped)
            StartCoroutine(StoneWalking());
    }
}