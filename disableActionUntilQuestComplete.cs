using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableActionUntilQuestComplete : MonoBehaviour
{
    void Awake ()
    {
        QuestManager.enableAction -= EnableBlockageAction;
        QuestManager.enableAction += EnableBlockageAction;
    }
    void EnableBlockageAction()
    {
        openAndClose siblingScript = GetComponent<openAndClose>();
        siblingScript.enabled = true;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        QuestManager.enableAction -= EnableBlockageAction;
    }
}
