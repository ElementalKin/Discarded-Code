using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxDespawn : MonoBehaviour
{
    public SoundControls sc;
    public float vfxDespawnTimer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.VFXSounds(sc);
        StartCoroutine(DestroySelf());
    }

    public IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(vfxDespawnTimer);
        Destroy(gameObject);
    }
}
