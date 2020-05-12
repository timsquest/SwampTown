using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thickenFog : MonoBehaviour
{
    public float originalFogDensity;
    public float newFogDensity;
    public float changeScale = .2f;
    void Awake ()
    {
        originalFogDensity = RenderSettings.fogDensity;
    }
    void OnTriggerEnter (Collider hopefullyPlayer)
    {
        if (hopefullyPlayer.CompareTag("Player"))
        {
            StopCoroutine(ReturnFogToNormalValue());
            StartCoroutine(ThickenFog());
        }
    }

    void OnTriggerExit (Collider hopefullyPlayer)
    {
        if (hopefullyPlayer.CompareTag("Player"))
        {
            StopCoroutine(ThickenFog());
            StartCoroutine(ReturnFogToNormalValue());
        }
    }

    IEnumerator ThickenFog ()
    {
        while (RenderSettings.fogDensity < newFogDensity)
        {
            RenderSettings.fogDensity += 0.001f;
            yield return new WaitForSeconds(changeScale);
        }
        if (RenderSettings.fogDensity > newFogDensity)
            RenderSettings.fogDensity = newFogDensity;
    }

    IEnumerator ReturnFogToNormalValue ()
    {
        while (RenderSettings.fogDensity > originalFogDensity)
        {
            RenderSettings.fogDensity -= 0.001f;
            yield return new WaitForSeconds(changeScale);
        }
        if (RenderSettings.fogDensity < originalFogDensity)
            RenderSettings.fogDensity = originalFogDensity;
    }
}
