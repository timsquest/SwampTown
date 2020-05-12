using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class stationProximityTrigger : MonoBehaviour
{
    public string playerTagName = "Player";
    public GameObject originalZTarget;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider hopefullyPlayer)
    {
        if (hopefullyPlayer.CompareTag(playerTagName))
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("StationZ"))
                {
                    AICharacterControl zombieScript = (AICharacterControl)child.gameObject.GetComponentInChildren(typeof(AICharacterControl));
                    zombieScript.SetTarget(hopefullyPlayer.gameObject.transform);
                }
            }
        }
    }

    void OnTriggerExit(Collider hopefullyPlayer)
    {
        if (hopefullyPlayer.CompareTag(playerTagName))
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("StationZ"))
                {
                    AICharacterControl zombieScript = (AICharacterControl)child.gameObject.GetComponentInChildren(typeof(AICharacterControl));
                    zombieScript.SetTarget(originalZTarget.transform);
                }
            }
        }
    }
}
