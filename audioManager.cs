using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    private AudioSource[] audioSourceList;
    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    void Awake ()
    {
        audioSourceList = GameObject.FindObjectsOfType<AudioSource>();
    }

    public void PauseAllAudio ()
    {
        foreach (AudioSource audio in audioSourceList)
        {
            if (audio.isPlaying == true)
            {
                audio.Pause();
                activeAudioSources.Add(audio);
            }
        }
    }

    public void PlayAllAudio ()
    {
        foreach (AudioSource audio in activeAudioSources)
        {
            audio.Play();
        }
        activeAudioSources.Clear();
    }
}
