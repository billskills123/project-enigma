using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolumeScript : MonoBehaviour {
    [Header("Sliders")]
    [SerializeField] private Slider soundsVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    public void SetSoundVolume() {
        PlayerPrefs.SetFloat("SoundsVolume", soundsVolumeSlider.value / 100); //Sets volume between 0 and 1
    }

    public void SetMusicVolume() {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value / 100); //Sets volume between 0 and 1
    }
}