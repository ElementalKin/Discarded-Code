using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundControls
{
    public string name;

    public AudioClip clip;
    [Range(0,10)]
    public float volume = 1f;
    [Range(0, 2)]
    public float pitchMinRange = 1f;
    [Range(0, 2)]
    public float pitchMaxRange = 1f;

    public float sfxDelay = 0f;


    //float rand = Random.Range(s.pitchVariance + 1, 1);
    //s.source.pitch = rand;
    //        Debug.Log("Random Pitch = "+rand);

    //[HideInInspector]
    //public AudioSource source;
}

