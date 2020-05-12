using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableColliderWhenQuestComplete : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake ()
    {
        QuestManager.disableCollider -= DisableCollider;
        QuestManager.disableCollider += DisableCollider;
    }
    void DisableCollider()
    {
        BoxCollider siblingCollider = GetComponent<BoxCollider>();
        siblingCollider.enabled = false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        QuestManager.disableCollider -= DisableCollider;
    }
}
