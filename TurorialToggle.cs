using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
[System.Serializable] public class StringIntEvent : UnityEvent<string, int> { }
public class TurorialToggle : MonoBehaviour
{
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        // Check if Turorial key is not made.
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            // Set Tutorial PlayerPrefs to 1
            PlayerPrefs.SetInt("Tutorial", 1);
        }
        // Update the toggle to the current state of Tutorial.
        toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Tutorial"));
    }

    /// <summary>
    /// Set PlayerPrefs float name to value.
    /// </summary>
    /// <param name="name">Pref to Set. </param>
    /// <param name="value">Value to set pref to. </param>
    public void SetPreferceFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }
    /// <summary>
    /// Set PlayerPrefs Int name to value.
    /// </summary>
    /// <param name="name">Pref to Set. </param>
    /// <param name="value">Value to set pref to. </param>
    public void SetPreferenceInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }
    /// <summary>
    /// switch name PlayerPrefs to 1 or 0.
    /// </summary>
    /// <param name="name">PlayerPrefs to swap.</param>
    public void SetPreferenceBool(string name)
    {
        PlayerPrefs.SetInt(name, Convert.ToInt32(toggle.isOn));
    }
}
