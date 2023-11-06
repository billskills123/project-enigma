using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text musicSliderText;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_Text soundSliderText;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private AudioSource source;

    public TMP_Dropdown resolutionDropdown; //Use using TMPro instead of TMPro.TMP_Dropdown

    Resolution[] resolutions;

     void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void UpdateSliderText(string sliderType) {
        if (sliderType == "Sound") {
            soundSliderText.text = soundSlider.value.ToString() + "%";
            source.Play();
        }
        else if (sliderType == "Music") {
            musicSliderText.text = musicSlider.value.ToString() + "%";
        }
    }
}