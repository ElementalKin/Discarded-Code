using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQinSwap : MonoBehaviour
{
    void Start()
    {
        GameObject tryForQin = GameObject.FindWithTag("Player");

        if (tryForQin != null)
        {
            tryForQin.transform.position = this.gameObject.transform.position;
            tryForQin.transform.rotation = this.gameObject.transform.rotation;

            Destroy(this.gameObject);
        }
    }
}