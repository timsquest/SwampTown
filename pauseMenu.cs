using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public Animator menuAnimator;
    public GameObject pauseCanvas;
    public GameObject inGameCanvas;
    public GameObject audioManagerInstance;

    void Awake ()
    {
        menuAnimator = GetComponent<Animator>();
    }

    public void MainMenuButton ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


    public void InstructionsButton ()
    {
        menuAnimator.SetBool("isMain", false);
        menuAnimator.SetBool("isInstructions", true);
    }

    public void BackButton ()
    {
        if (menuAnimator.GetBool("isInstructions") != true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            //Seperate audioManager script
            audioManagerInstance.GetComponent<audioManager>().PlayAllAudio();
            inGameCanvas.SetActive(true);
            //Do this last, because it will turn off this script
            pauseCanvas.SetActive(false);
        }
        else
        {
            menuAnimator.SetBool("isInstructions", false);
            menuAnimator.SetBool("isMain", true);
        }        
    }
}
