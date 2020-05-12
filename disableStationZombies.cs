using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableStationZombies : MonoBehaviour
{
    public GameObject stationZombieContainer;
    void Awake ()
    {
        QuestManager.enableAction -= DisableZombies;
        QuestManager.enableAction += DisableZombies;
    }

    void DisableZombies()
    {
        stationZombieContainer.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        QuestManager.enableAction -= DisableZombies;
    }
}
