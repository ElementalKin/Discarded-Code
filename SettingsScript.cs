using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Audio;
//using UnityEngine.UI;
//using TMPro;

public class SettingsScript : MonoBehaviour
{

    //    public AudioMixer audioMixer;

    //    public TMP_Dropdown resolutionDropdown;

    //    Resolution[] resolutions;

    //    private void Start()
    //    {
    //        {
    //            resolutions = Screen.resolutions;

    //            resolutionDropdown.ClearOptions();

    //            //Gather all available computer resolutions for their machine

    //            List<string> options = new List<string>();

    //            int currResolutionIndex = 0;
    //            for (int i = 0; i < resolutions.Length; i++)
    //            {
    //                string option = resolutions[i].width + " x " + resolutions[i].height;
    //                options.Add(option);

    //                //Make the default resolution text on the dropdown equal to default resolution

    //                if (resolutions[i].width == Screen.currentResolution.width && 
    //                    resolutions[i].height == Screen.currentResolution.height)
    //                {
    //                    currResolutionIndex = i;
    //                }
    //            }

    //            resolutionDropdown.AddOptions(options);
    //            resolutionDropdown.value = currResolutionIndex;
    //            resolutionDropdown.RefreshShownValue();
    //        }
    //    }
    //    public void SetVolume(float volume)
    //    {
    //        audioMixer.SetFloat("masterVolume", volume);
    //    }

    //    public void SetFullscreen (bool isFullscreen)
    //    {
    //        Screen.fullScreen = isFullscreen;
    //    }

    //    public void SetResolution (int resolutionIndex)
    //    {
    //        Resolution resolution = resolutions[resolutionIndex];
    //        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //    }
}
