using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireAnimation : MonoBehaviour
{
    public string animatorBoolTrigger = "isReady";
    void Awake ()
    {
        QuestManager.enableAction -= AnimateQuest;
        QuestManager.enableAction += AnimateQuest;
        QuestManager.closingAnimation -= AnimateQuest;
        QuestManager.closingAnimation += AnimateQuest;
    }
    void AnimateQuest()
    {
        Animator siblingAnim = GetComponent<Animator>();
        siblingAnim.SetBool(animatorBoolTrigger, true);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        QuestManager.enableAction -= AnimateQuest;
        QuestManager.closingAnimation -= AnimateQuest;
    }
}