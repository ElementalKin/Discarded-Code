using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string name;

    public AudioClip clip;
    [Range(0, 10)]
    public float volume = 1f;

    public bool loop = false;

    // Only enabled for some Audio
    [Range(0, 2)]
    public float pitchMinRange = 1f;
    [Range(0, 2)]
    public float pitchMaxRange = 1f;

    public float sfxDelay = 0f;
}

