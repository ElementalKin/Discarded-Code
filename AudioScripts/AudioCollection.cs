using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newAudioCollection", menuName = "Create Audio Collection")]
public class AudioCollection : ScriptableObject
{
    public Sounds[] sounds;

    [SerializeField] Sounds[] AudioClipCollection;
    public Sounds GetAudioClip (int arrayIDX)
    {
        return AudioClipCollection[arrayIDX];
    }
}
