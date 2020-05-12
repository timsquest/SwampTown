using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getPauseInput : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject inGameCanvas;
    private bool isPaused = false;
    AudioSource[] audioSourceList = new AudioSource[]{};
    public GameObject audioManagerInstance;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape)) 
        {
             if(isPaused == true)
             {
                Time.timeScale = 1.0f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                //Seperate audioManager script
                audioManagerInstance.GetComponent<audioManager>().PlayAllAudio();
                isPaused = false;
                inGameCanvas.SetActive(true);
                //Do this last, because it will turn off this script
                pauseCanvas.SetActive(false);
            } 
            else 
            {
                Time.timeScale = 0.0f;
                pauseCanvas.SetActive(true);
                inGameCanvas.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                //Seperate audioManager script
                audioManagerInstance.GetComponent<audioManager>().PauseAllAudio();
                isPaused = true;
            }
        }
    }
}
