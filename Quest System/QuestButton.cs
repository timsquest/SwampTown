using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public Button buttonComponent;
    public RawImage icon;
    public Text eventName;
    public Sprite currentImage;
    public Sprite waitingImage;
    public Sprite doneImage;
    public QuestEvent thisEvent;
    //public Compass compassController;
    public Animator textAnimator;
    public float textAnimationLength = 1.5f;

    QuestEvent.EventStatus status;

    void Awake ()
    {
        //buttonComponent.onClick.AddListener(ClickHandler);
        //compassController = GameObject.Find("Compass").GetComponent<Compass>();
    }

    public void Setup(QuestEvent e, GameObject scrollList)
    {
        thisEvent = e;
        buttonComponent.transform.SetParent(scrollList.transform, false);
        eventName.text = "<b>" + thisEvent.name + "</b>\n" + thisEvent.description;
        status = thisEvent.status;
        icon.texture = waitingImage.texture;
        buttonComponent.interactable = false;
        buttonComponent.gameObject.SetActive(false);
        textAnimator = GetComponent<Animator>();
    }

    public void UpdateButton(QuestEvent.EventStatus s)
    {
        status = s;
        if (status == QuestEvent.EventStatus.DONE)
        {
            icon.texture = doneImage.texture;
            buttonComponent.interactable = false;
            textAnimator.SetBool("isQuestComplete", true);
            Invoke("SetButtonInactive",textAnimationLength);
        }
        else if (status == QuestEvent.EventStatus.WAITING)
        {
            icon.texture = waitingImage.texture;
            buttonComponent.interactable = false;
        }
        else if (status == QuestEvent.EventStatus.CURRENT)
        {
            icon.texture = currentImage.texture;
            //buttonComponent.interactable = true;
            buttonComponent.gameObject.SetActive(true);
            //ClickHandler();
        }
    }

    /*public void ClickHandler()
    {
        compassController.target = thisEvent.location;
    }*/

    void SetButtonInactive()
    {
        buttonComponent.gameObject.SetActive(false);
    }
}
