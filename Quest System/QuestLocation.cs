using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    public QuestManager qManager;
    public List<QuestEvent> qEvent = new List<QuestEvent>();
    public QuestButton qButton;
    public UnityEngine.UI.Text interationText;
    public KeyCode interactionKey = KeyCode.E;
    int playerLayerBit = 10;
    static string interactionWordingCurrentQuest = "Press \"E\" to interact";
    static string interactionWordingSkipQuestAudio = "Press \"E\" to skip dialogue and interact...";
    static string interactionWordingWaitingQuest = "This looks useful...";
    static string interactionWordingTimedQuest = "I may need to wait...";
    public List<AudioSource> sounds = new List<AudioSource>();

    void Awake ()
    {
        interationText = GameObject.Find("InteractionText").GetComponent<UnityEngine.UI.Text>();
    }

    void Update ()
    {
        foreach (QuestEvent q in qEvent)
        {
            if (q.status == QuestEvent.EventStatus.CURRENT && q.duration > 0f)
            {
                q.currentTimer += Time.deltaTime;
            }
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1 << playerLayerBit;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hit, 10, layerMask))
        {
            Collider objectOfAim = hit.collider;
            /*Debug.Log("Aiming at " + objectOfAim.name);
            Debug.Log("Quest location is " + this.gameObject.name);
            if (objectOfAim.gameObject.transform.parent != null)
                Debug.Log("Object parent is " + objectOfAim.gameObject.transform.parent.gameObject.name);*/
            if (objectOfAim.gameObject.name == this.gameObject.name || (objectOfAim.gameObject.transform.parent != null && 
                objectOfAim.gameObject.transform.parent.gameObject.name == this.gameObject.name))
            {
                bool interactionState = false;
                foreach(QuestEvent questEvent in qEvent)
                {
                    if (questEvent.status != QuestEvent.EventStatus.DONE)
                    {
                        interactionState = true;
                    }
                    if (questEvent.status == QuestEvent.EventStatus.CURRENT)
                    {
                        GameObject audioSourceParent = GameObject.Find("QuestAudio");
                        AudioSource[] audioSources = audioSourceParent.GetComponentsInChildren<AudioSource>();
                        List<AudioSource> currentlyPlayingAudio = new List<AudioSource>();
                        bool isCurrentlyPlaying = false;
                        foreach(AudioSource source in audioSources)
                        {
                            if (source.isPlaying == true)
                            {
                                isCurrentlyPlaying = true;
                                currentlyPlayingAudio.Add(source);
                            }

                        }
                        if (isCurrentlyPlaying)
                        {
                            interationText.text = interactionWordingSkipQuestAudio;
                        }
                        else if (questEvent.currentTimer < questEvent.duration)
                        {
                            interationText.text = interactionWordingTimedQuest;
                            break;
                        }
                        else
                        {
                            interationText.text = interactionWordingCurrentQuest;
                        }
                        if (Input.GetKey(interactionKey))
                        {
                            interationText.gameObject.SetActive(false);
                            questEvent.UpdateQuestEvent(QuestEvent.EventStatus.DONE);
                            qManager.UpdateQuestsOnCompletion(questEvent);
                            foreach (AudioSource currentSound in currentlyPlayingAudio)
                            {
                                currentSound.Stop();
                            }
                            AudioSource[] prexistentAudio = this.gameObject.GetComponentsInChildren<AudioSource>();
                            if (sounds.Count > 0)
                            {
                                foreach (AudioSource audio in prexistentAudio)
                                {
                                    audio.Stop();
                                }
                                sounds[0].gameObject.transform.position = transform.position;
                                sounds[0].Play();
                                sounds.RemoveAt(0);
                            }
                        }
                        break;
                    }
                    else
                    {
                        interationText.text = interactionWordingWaitingQuest;
                    }
                }
                if (interactionState == true)
                {
                    interationText.gameObject.SetActive(true);
                }
                else
                {
                    interationText.gameObject.SetActive(false);
                }
            }
            else
            {
                interationText.gameObject.SetActive(false);
                return;
            }
        }
        else
        {
            interationText.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit ()
    {
        interationText.gameObject.SetActive(false);
    }

    public void Setup(QuestManager qm, QuestEvent qe, QuestButton qb)
    {
        qManager = qm;
        qEvent.Add(qe);
        qButton = qb;
        qe.button = qButton;
    }
}
