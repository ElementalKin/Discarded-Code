using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickedOn : MonoBehaviour
{
    public GameObject ai;

    public EnemySpawnProto enemySpawn;

    public StateMachine stateMachine;

    public GameObject lightObject;
    public LightController lightC;

    public bool isEnemy1;

    // Start is called before the first frame update
    void Start()
    {
        //switch (SceneManagment.currentSceneLoaded)
        //{
        //    case SceneManagment.SceneLoaded.basement:
        //        lightObject = GameObject.Find("EnemyLight_02");
        //        Debug.Log("light Object Basement");

        //        break;
        //    case SceneManagment.SceneLoaded.bedroom:
        //        lightObject = GameObject.Find("EnemyLight_02");
        //        Debug.Log("light Object Bedroom");
        //        break;
        //    case SceneManagment.SceneLoaded.attic:
        //        lightObject = GameObject.Find("EnemyLight_02");
        //        Debug.Log("light Object Attic");
        //        break;
        //}
        lightObject = GameObject.Find("EnemyLight_02");
        lightC = lightObject.GetComponent<LightController>();
        GameObject SilhouetteModel = transform.GetChild(1).gameObject;
        SilhouetteModel.SetActive(true);
    }

    public void Clicked()
    {
        GameObject NormalModel = transform.GetChild(0).gameObject;
        GameObject SilhouetteModel = transform.GetChild(1).gameObject;
        //GameObject SilhouetteLighting = transform.GetChild(2).gameObject;
        SilhouetteModel.SetActive(false);
        NormalModel.SetActive(true);
        //Spawns given ai from inspector
        //transitionTimer = 0;

        //StartCoroutine(CoroutineSimple());
        //while (transitionTimer < transitionTimerStart)
        //{
        //    transitionTimer += Time.deltaTime;
        //}
        AudioManager.instance.SilhouetteSounds(1);
        ClickEnemy.rayCastOn = false;
        enemySpawn.SpawnEnemy(ai, gameObject);
        stateMachine.beginNewCombat();
        ClickEnemy.rayEnter = false;
        ClickEnemy.rayExit = true;
    }

    public void Hovered()
    {
        // Hover Audio
        AudioManager.instance.SilhouetteSounds(0);
        if (isEnemy1)
        {
            //Debug.Log("Point Light 1 Called");
            lightC.Light1(true);
        }
        else
        {
            //Debug.Log("Point Light 2 Called");
            lightC.Light2(true);
        }

        //Debug.Log("isHovering");
    }

    IEnumerator CoroutineSimple()
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(3);
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

}
