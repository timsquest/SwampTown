using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCameraTarget : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject newCamera;
    void Awake ()
    {
        QuestManager.closingAnimation -= ChangeTarget;
        QuestManager.closingAnimation += ChangeTarget;
    }

    void ChangeTarget()
    {
        mainCamera.GetComponent<Camera>().enabled = false;
        mainCamera.GetComponent<AudioListener>().enabled = false;
        newCamera.GetComponent<Camera>().enabled = true;
        newCamera.GetComponent<AudioListener>().enabled = true;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        QuestManager.closingAnimation -= ChangeTarget;
    }
}
