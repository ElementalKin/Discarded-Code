using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuProto : MonoBehaviour
{
    public SceneManagment sm;
    public GameObject pauseMenu, menuCanvas, creditsCanvas;
    public Animator playCamAnimator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            }
        }
    }

    public void PlayGame()
    {
        //Debug.Log("Play Game Load scene 2");
        sm.resetStatics();

        PartsManager[] destroyPlayers = FindObjectsOfType<PartsManager>();
        for (int i = 0; i < destroyPlayers.Length; i++)
        {
            if (destroyPlayers[i].CompareTag("Player"))
            {
                Destroy(destroyPlayers[i].gameObject);
            }
        }

        SceneManager.LoadScene(2);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
        //SceneManagment.numberOfBattles = 7;
    }
    public void HallwayAnim()
    {
        playCamAnimator.SetBool("PlayGameAnim", true);
    }
    public void OpeningCutscene()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenMainMenuCanvas()
    {
        menuCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }
}
