using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestEvent
{
    public enum EventStatus { WAITING, CURRENT, DONE };

    public string name;
    public string description;
    public string id;
    public int order = -1;
    public EventStatus status;
    public QuestButton button;
    public GameObject location;
    public List<QuestPath> pathList = new List<QuestPath>();
    public QuestManager.QuestComplete evnt;
    public float duration = 0f;
    public float currentTimer = 0f;
    public Compass compassController;

    public QuestEvent(string n, string d, GameObject loc)
    {
        id = Guid.NewGuid().ToString();
        name = n;
        description = d;
        status = EventStatus.WAITING;
        location = loc;
        evnt = null;
    }

    public QuestEvent(string n, string d, GameObject loc, float isTimed = 0f)
    {
        id = Guid.NewGuid().ToString();
        name = n;
        description = d;
        status = EventStatus.WAITING;
        location = loc;
        duration = isTimed;
        evnt = null;
    }
    public QuestEvent(string n, string d, GameObject loc, float isTimed = 0f, QuestManager.QuestComplete questEvent = null)
    {
        id = Guid.NewGuid().ToString();
        name = n;
        description = d;
        status = EventStatus.WAITING;
        location = loc;
        duration = isTimed;
        evnt = questEvent;
    }
    
    public void UpdateQuestEvent(EventStatus es)
    {
        if (es == EventStatus.CURRENT)
        {
            compassController = GameObject.Find("Compass").GetComponent<Compass>();
            compassController.currentQuestTarget.target = location;
            QuestEventTriggered();
        }
        status = es;
        button.UpdateButton(es);
    }

    public string GetId ()
    {
        return id;
    }

    void QuestEventTriggered()
    {
        if (evnt != null)
            evnt();
    }
}
