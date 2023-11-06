using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetVolumeScript : MonoBehaviour {
    [Header("Audio Sources")]
    [SerializeField] private AudioSource[] soundsAudioSources;
    [SerializeField] private AudioSource[] musicAudioSources;

    [Header("Settings Objects")]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;

    //Updates all the sounds
    public void UpdateSoundsVolume() {
        foreach (AudioSource source in soundsAudioSources) {
            source.volume = PlayerPrefs.GetFloat("SoundsVolume");
        }
    }

    //Updates all the music
    public void UpdateMusicVolume() {
        foreach (AudioSource source in musicAudioSources) {
            source.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
    }

    //Updates all music and sounds at the beggining of the scene.
    private void Start() {
        foreach (AudioSource source in soundsAudioSources) {
            source.volume = PlayerPrefs.GetFloat("SoundsVolume");
        }

        foreach (AudioSource source in musicAudioSources) {
            source.volume = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (soundSlider != null && musicSlider != null) {
            soundSlider.value = PlayerPrefs.GetFloat("SoundsVolume");
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
    }
}