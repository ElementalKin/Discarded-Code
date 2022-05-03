using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxSpawner : MonoBehaviour
{
    public List<GameObject> vfxHolders = new List<GameObject>();
    public EnemyAI[] ai;
    public Transform QuinTrans;

    /// <summary>
    /// Status is none/0 when not a support type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isPlayer"></param>
    /// <param name="status"></param>
    public void VFXSpawn(CardHelpers.EffectType type, bool isPlayer, int status)
    {
        ai = FindObjectsOfType<EnemyAI>();

        if (isPlayer == true)
        {
            Debug.Log("Player vfx Play");
            switch (type)
            {
                case CardHelpers.EffectType.Attack:
                    Instantiate(vfxHolders[0], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                    break;
                case CardHelpers.EffectType.Block:
                    switch(SceneManagment.currentSceneLoaded)
                    {
                        case SceneManagment.SceneLoaded.basement:
                            Instantiate(vfxHolders[9], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            //Debug.Log("Basement Shield");
                            break;
                        case SceneManagment.SceneLoaded.bedroom:
                            Instantiate(vfxHolders[10], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            //Debug.Log("Bedroom Shield");
                            break;
                        case SceneManagment.SceneLoaded.attic:
                            Instantiate(vfxHolders[11], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            //Debug.Log("Attic Shield");
                            break;
                    }
                    break;
                case CardHelpers.EffectType.Support:
                    switch (status)
                    {
                        case 1:
                            Instantiate(vfxHolders[2], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(vfxHolders[3], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        case 3:
                            Instantiate(vfxHolders[4], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        case 4:
                            Instantiate(vfxHolders[5], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        case 5:
                            Instantiate(vfxHolders[6], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        case 6:
                            Instantiate(vfxHolders[7], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        case 7:
                            Instantiate(vfxHolders[8], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        case 8:
                            Instantiate(vfxHolders[9], new Vector3(QuinTrans.position.x, 0.23f, QuinTrans.position.z), Quaternion.identity);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {   // Else for AI/enemy
            Debug.Log("Enemy vfx Play");
            switch (type)
            {
                case CardHelpers.EffectType.Attack:
                    Instantiate(vfxHolders[0], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                    break;
                case CardHelpers.EffectType.Block:
                    switch (SceneManagment.currentSceneLoaded)
                    {
                        case SceneManagment.SceneLoaded.basement:
                            Instantiate(vfxHolders[9], new Vector3(ai[0].transform.position.x + 0.05f, 0.23f, ai[0].transform.position.z + 0.05f), new Quaternion(Quaternion.identity.x,Quaternion.identity.y -180f,Quaternion.identity.z, Quaternion.identity.w));
                            //Debug.Log("Basement Shield");
                            break;
                        case SceneManagment.SceneLoaded.bedroom:
                            Instantiate(vfxHolders[10], new Vector3(ai[0].transform.position.x + 0.05f, 0.23f, ai[0].transform.position.z + 0.05f), new Quaternion(Quaternion.identity.x, Quaternion.identity.y - 180f, Quaternion.identity.z, Quaternion.identity.w));
                            //Debug.Log("Bedroom Shield");
                            break;
                        case SceneManagment.SceneLoaded.attic:
                            Instantiate(vfxHolders[11], new Vector3(ai[0].transform.position.x + 0.05f, 0.23f, ai[0].transform.position.z + 0.05f), new Quaternion(Quaternion.identity.x, Quaternion.identity.y - 180f, Quaternion.identity.z, Quaternion.identity.w));
                            //Debug.Log("Attic Shield");
                            break;
                    }
                    break;
                case CardHelpers.EffectType.Support:
                    switch (status)
                    {
                        case 1:
                            Instantiate(vfxHolders[2], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(vfxHolders[3], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                            break;
                        case 3:
                            Instantiate(vfxHolders[4], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                            break;
                        case 4:
                            Instantiate(vfxHolders[5], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                            break;
                        case 5:
                            Instantiate(vfxHolders[6], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                            break;
                        case 6:
                            Instantiate(vfxHolders[7], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                            break;
                        case 7:
                            Instantiate(vfxHolders[8], new Vector3(ai[0].transform.position.x, 0.23f, ai[0].transform.position.z), Quaternion.identity);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    //public void SFXSpawn(int status)
    //{
    //    switch (status)
    //    {
    //        case 0:
    //            AudioManager.instance.VFXSounds(0);
    //            break;
    //        case 1:
    //            AudioManager.instance.VFXSounds(1);
    //            break;
    //        case 2:
    //            AudioManager.instance.VFXSounds(2);
    //            break;
    //        case 3:
    //            AudioManager.instance.VFXSounds(3);
    //            break;
    //        case 4:
    //            AudioManager.instance.VFXSounds(4);
    //            break;
    //        case 5:
    //            AudioManager.instance.VFXSounds(5);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //public IEnumerator VFXTimer(CardHelpers.EffectType type, float length, bool isPlayer)
    //{
    //    ai = FindObjectsOfType<EnemyAI>();
    //    if (isPlayer == true)
    //    {
    //        switch (type)
    //        {
    //            case CardHelpers.EffectType.Attack:
    //                Instantiate(vfxHolders[0], QuinTransform.position, Quaternion.identity);
    //                break;
    //            case CardHelpers.EffectType.Block:
    //                Instantiate(vfxHolders[1], QuinTransform.position, Quaternion.identity);
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        switch (type)
    //        {
    //            case CardHelpers.EffectType.Block:
    //                //QuinTransform.GetChild()
    //                //vfxHolderEnemy.transform.GetChild(1).gameObject.SetActive(true);
    //                yield return new WaitForSeconds(length);
    //                //vfxHolderEnemy.transform.GetChild(1).gameObject.SetActive(false);
    //                break;
    //            case CardHelpers.EffectType.Attack:


    //                break;



    //            default:
    //                break;
    //        }
    //    }



    //}

}
