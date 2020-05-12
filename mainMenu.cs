using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public Animator menuAnimator;

    void Awake ()
    {
        menuAnimator = GetComponent<Animator>();
    }
    public void PlayGame ()
    {
        SceneManager.LoadScene("current");
    }


    public void InstructionsButton ()
    {
        menuAnimator.SetBool("isMain", false);
        menuAnimator.SetBool("isInstructions", true);
    }

    public void CreditsButton ()
    {
        menuAnimator.SetBool("isMain", false);
        menuAnimator.SetBool("isCredits", true);
    }

    public void BackButton ()
    {
        menuAnimator.SetBool("isInstructions", false);
        menuAnimator.SetBool("isCredits", false);
        menuAnimator.SetBool("isMain", true);
    }

    public void QuitButton ()
    {
        Application.Quit();
    }    
}
