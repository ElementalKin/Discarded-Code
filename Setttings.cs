using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setttings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioManager audioManager;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Dropdown textureDropdown;
    public Dropdown aaDropdown;
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    float currentMasterVolume;
    float currentMusicVolume;
    float currentSFXVolume;
    Resolution[] resolutions;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " +
                     resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                  && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        currentMasterVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        audioManager.musicVolume = volume;
        currentMusicVolume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        audioManager.sfxVolume = volume;
        currentSFXVolume = volume;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetTextureQaulity(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) // if the user is not using 
                               //any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);

        switch (qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }
        qualityDropdown.value = qualityIndex;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference",Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("MasterVolumePreference", currentMasterVolume);
        PlayerPrefs.SetFloat("MusicVolumePreference", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXVolumePreference", currentSFXVolume);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingsPreference")) qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingsPreference");
        else qualityDropdown.value = 3;
        if (PlayerPrefs.HasKey("ResolutionPreference")) resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else resolutionDropdown.value = currentResolutionIndex;
        if (PlayerPrefs.HasKey("TextureQualityPreference")) textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else textureDropdown.value = 0;
        if (PlayerPrefs.HasKey("AntiAliasingPreference")) aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
        else aaDropdown.value = 1;
        if (PlayerPrefs.HasKey("FullscreenPreference")) Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else Screen.fullScreen = true;
        if (PlayerPrefs.HasKey("MasterVolumePreference")) MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolumePreference");
        else MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolumePreference");
        if (PlayerPrefs.HasKey("MusicVolumePreference")) MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicMasterVolumePreference");
        else MasterVolumeSlider.value = PlayerPrefs.GetFloat("MusicMasterVolumePreference");
        if (PlayerPrefs.HasKey("SFXVolumePreference")) SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolumePreference");
        else MasterVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolumePreference");
    }
}
