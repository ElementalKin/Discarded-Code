using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float destroyTime;
    public bool random;
    public float minDestroy;
    public float maxDestroy;
    // Start is called before the first frame update
    void Start()
    {
        if (!random)
        {
            Destroy(gameObject, destroyTime);
        }
        else
        {
            Destroy(gameObject, Random.Range(minDestroy, maxDestroy));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
