using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapLerp : MonoBehaviour
{
    private Animator anim;

    public Slider minimapSlider;

    public float maxLerpTime = 1f;
    private float currTime;

    private float desiredLocation = 1.25f;
    private float currentLocation = 1.25f;

    private static readonly string openMap = "OpenMap";

    public StateMachine stateMachine;

    void Start()
    {
        anim = GetComponent<Animator>();

        currTime = maxLerpTime;

        if (SceneManagment.numberOfBattles > 0)
        {
            minimapSlider.value = CardHandUI.minimapLocations[Mathf.Clamp(SceneManagment.numberOfBattles + 1, 1, CardHandUI.minimapLocations.Length - 1)];
        }
    }

    public void OpenMapUI()
    {
        anim.SetBool(openMap, true);
    }

    public void CloseMapUI()
    {
        anim.SetBool(openMap, false);
        ClickEnemy.rayCastOn = true;
    }

    public void MoveCamsBack()
    {
        stateMachine.SwitchToEnemySelectCam();
    }

    private void BeginLerp()
    {
        currentLocation = minimapSlider.value;
        desiredLocation = CardHandUI.minimapLocations[Mathf.Clamp(SceneManagment.numberOfBattles + 1, 1, CardHandUI.minimapLocations.Length - 1)];

        currTime = 0;
    }

    void Update()
    {
        if (currTime < maxLerpTime)
        {
            minimapSlider.value = Mathf.Lerp(currentLocation, desiredLocation, currTime / maxLerpTime);
            currTime += Time.deltaTime;
        }
    }
}
