using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneManagment
{
    public static PartsManager oldParts;
    public static int numberOfBattles;
    public static int playerHealth;
    public static bool isSceneLoaded;

    public static float sfxVolume = 1f;
    public static float musicVolume =1f;

    public enum SceneLoaded {menu, cutscene, basement, bedroom, attic};
    public static SceneLoaded currentSceneLoaded;

    public void numberOfBattleUp()
    {
        numberOfBattles++;
    }

    /// <summary>
    /// Resets all statics to default, (0, false, etc)
    /// </summary>
    public void resetStatics()
    {
        numberOfBattles = 0;
        isSceneLoaded = false;
        currentSceneLoaded = SceneLoaded.basement;
    }

    public void restartGame()
    {
        numberOfBattles = 0;
        isSceneLoaded = false;
        currentSceneLoaded = SceneLoaded.basement;
    }

    public void loadLevel(PartsManager playerParts)
    {
        switch(numberOfBattles)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                oldParts = playerParts;
                //Debug.Log("Scene Managment Load Scene 3");
                currentSceneLoaded = SceneLoaded.bedroom;
                SceneManager.LoadScene(3);
                ClickEnemy.rayEnter = false;
                ClickEnemy.rayExit = true;
                //AudioManager.instance.DeckSounds(5);
                //state.numberOfBattle = numberOfBattles;
                break;
            case 5:
            case 6:
            case 7:
            case 8:
                oldParts = playerParts;
                //Debug.Log("Scene Managment Load Scene 4");
                currentSceneLoaded = SceneLoaded.attic;
                SceneManager.LoadScene(4);
                ClickEnemy.rayEnter = false;
                ClickEnemy.rayExit = true;
                //AudioManager.instance.DeckSounds(6);
                break;
            case 9:
                oldParts = playerParts;
                //Debug.Log("Scene Managment Load Scene 5");
                SceneManager.LoadScene(5);
                ClickEnemy.rayEnter = false;
                ClickEnemy.rayExit = true;
                //AudioManager.instance.DeckSounds(7);
                break;
        }
    }
}
