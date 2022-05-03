using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject pointLight1;
    public GameObject pointLight2;


    private void Update()
    {
        if (ClickEnemy.rayEnter == false && ClickEnemy.rayExit == true)
        {
            Light1(false);
            Light2(false);
            ClickEnemy.rayExit = false;
        }
    }
    public void Light1(bool lightOn)
    {
        if (lightOn == true)
        {
            //Debug.Log("Point Light 1 On ");
            pointLight1.SetActive(true);
            //if (SceneManagment.currentSceneLoaded != SceneManagment.SceneLoaded.attic )
            //{
            //    //Debug.Log("Point Light 2 turn off");
            //    pointLight2.SetActive(false);
            //}
        }
        else
        {
            //Debug.Log("Point Lights Off");
            pointLight1.SetActive(false);
            pointLight2.SetActive(false);
        }
    }

    public void Light2(bool lightOn)
    {
        if (lightOn == true)
        {
            //Debug.Log("Point Light 2 On");
            //pointLight1.SetActive(false);
            pointLight2.SetActive(true);
        }
        else
        {
            //Debug.Log("Point Lights Off");
            pointLight1.SetActive(false);
            pointLight2.SetActive(false);
        }
    }
}
