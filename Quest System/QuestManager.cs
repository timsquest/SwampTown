using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest quest = new Quest();
    public GameObject questPrintBox;
    public GameObject buttonPrefab;

    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public GameObject E;
    public GameObject F;
    public GameObject G;
    public GameObject GA;
    public GameObject H;
    public GameObject I;
    public GameObject J;
    public GameObject K;
    public GameObject L;
    public GameObject M;
    public GameObject N;

    public GameObject victoryPopup;
    public GameObject inGameUI;
    public float fadeUIAnimationLength = 3f;
    QuestEvent endQuest;
    public List<AudioSource> audioFiles = new List<AudioSource>();
    public delegate void QuestComplete();
    public static event QuestComplete startEvent;
    public static event QuestComplete stopEvent;
    public static event QuestComplete enableAction;
    public static event QuestComplete disableCollider;
    public static event QuestComplete closingAnimation;
    public static event QuestComplete finalFight;

    void Awake ()
    {
        if (!inGameUI)
            inGameUI = GameObject.Find("InGameUIPanel");
    }

    // Start is called before the first frame update
    void Start()
    {
        QuestEvent a = quest.AddQuestEvent("Check the radio","The radio seems to making buzzing noises...", A);
        QuestEvent b = quest.AddQuestEvent("Get the keys","Look for the car keys under the sofa cushions...", B);
        QuestEvent c = quest.AddQuestEvent("Start the car","", C);
        QuestEvent d = quest.AddQuestEvent("Look for a battery","Abe keeps some spare vehicles in the back...", D);
        QuestEvent e = quest.AddQuestEvent("Try the new battery","", E);
        QuestEvent f = quest.AddQuestEvent("Radio Abe","Abe may be able to help...", F);
        QuestEvent g = quest.AddQuestEvent("Scout the area","Make sure areas like the outhouse are clear of zombies...", G);
        QuestEvent ga = quest.AddQuestEvent("Wait for Abe","Don't get bit. Check in with Abe later...", GA, 45f, startEvent);
        QuestEvent h = quest.AddQuestEvent("Check the satellite dish","Abe mentioned a radio station down the road...", H, 0f, stopEvent);
        QuestEvent i = quest.AddQuestEvent("Look for a distraction","Find something that will draw the zombies away...", I, 0f, disableCollider);
        QuestEvent j = quest.AddQuestEvent("Light the fireworks","", J);
        QuestEvent k = quest.AddQuestEvent("Fix the satellite dish","The electronics may need to be reset...", K, 0f, enableAction);
        QuestEvent l = quest.AddQuestEvent("Radio Abe","Find out how to escape...", L);
        QuestEvent m = quest.AddQuestEvent("Get the boat keys","They are probably under the couch cushions...", M, 0f, finalFight);
        QuestEvent n = quest.AddQuestEvent("Ride into the sunset","Escape the zombie horde on Abe's boat...", N, 0f, stopEvent);

        quest.AddPath(a.GetId(),b.GetId());
        quest.AddPath(b.GetId(),c.GetId());
        quest.AddPath(c.GetId(),d.GetId());
        quest.AddPath(d.GetId(),e.GetId());
        quest.AddPath(e.GetId(),f.GetId());
        quest.AddPath(f.GetId(),g.GetId());
        quest.AddPath(g.GetId(),ga.GetId());
        quest.AddPath(ga.GetId(),h.GetId());
        quest.AddPath(h.GetId(),i.GetId());
        quest.AddPath(i.GetId(),j.GetId());
        quest.AddPath(j.GetId(),k.GetId());
        quest.AddPath(k.GetId(),l.GetId());
        quest.AddPath(l.GetId(),m.GetId());
        quest.AddPath(m.GetId(),n.GetId());

        quest.BreadthFirstSearch(l.GetId());

        QuestButton button = CreateButton(a).GetComponent<QuestButton>();
        A.GetComponent<QuestLocation>().Setup(this, a, button);
        button = CreateButton(b).GetComponent<QuestButton>();
        B.GetComponent<QuestLocation>().Setup(this, b, button);
        button = CreateButton(c).GetComponent<QuestButton>();
        C.GetComponent<QuestLocation>().Setup(this, c, button);
        button = CreateButton(d).GetComponent<QuestButton>();
        D.GetComponent<QuestLocation>().Setup(this, d, button);
        button = CreateButton(e).GetComponent<QuestButton>();
        E.GetComponent<QuestLocation>().Setup(this, e, button);
        button = CreateButton(f).GetComponent<QuestButton>();
        F.GetComponent<QuestLocation>().Setup(this, f, button);
        button = CreateButton(g).GetComponent<QuestButton>();
        G.GetComponent<QuestLocation>().Setup(this, g, button);
        button = CreateButton(ga).GetComponent<QuestButton>();
        GA.GetComponent<QuestLocation>().Setup(this, ga, button);
        button = CreateButton(h).GetComponent<QuestButton>();
        H.GetComponent<QuestLocation>().Setup(this, h, button);
        button = CreateButton(i).GetComponent<QuestButton>();
        I.GetComponent<QuestLocation>().Setup(this, i, button);
        button = CreateButton(j).GetComponent<QuestButton>();
        J.GetComponent<QuestLocation>().Setup(this, j, button);
        button = CreateButton(k).GetComponent<QuestButton>();
        K.GetComponent<QuestLocation>().Setup(this, k, button);
        button = CreateButton(l).GetComponent<QuestButton>();
        L.GetComponent<QuestLocation>().Setup(this, l, button);
        button = CreateButton(m).GetComponent<QuestButton>();
        M.GetComponent<QuestLocation>().Setup(this, m, button);
        button = CreateButton(n).GetComponent<QuestButton>();
        N.GetComponent<QuestLocation>().Setup(this, n, button);

        endQuest = n;
    }

    GameObject CreateButton(QuestEvent e)
    {
        GameObject b = Instantiate(buttonPrefab);
        b.GetComponent<QuestButton>().Setup(e, questPrintBox);
        if (e.order == 1)
        {
            b.GetComponent<QuestButton>().UpdateButton(QuestEvent.EventStatus.CURRENT);
            e.status = QuestEvent.EventStatus.CURRENT;
        }
        return b;
    }

    public void UpdateQuestsOnCompletion(QuestEvent e)
    {
        if (e == endQuest)
        {
            closingAnimation();
            Cursor.lockState = CursorLockMode.None;
            Animator inGameUIAnim = inGameUI.GetComponent<Animator>();
            Invoke("SetObjectInactive",fadeUIAnimationLength);
            inGameUIAnim.SetBool("isFinished", true);
            victoryPopup.SetActive(true);
            Animator victoryAnim = victoryPopup.GetComponent<Animator>();
            victoryAnim.SetBool("isFinished", true);
            Cursor.visible = true;
            StartCoroutine(ScaleTime(1.0f, 0.0f, 3.0f));
            GameObject.FindGameObjectWithTag("Player").SetActive(false);
            return;
        }
        foreach (QuestEvent n in quest.questEvents)
        {
            if (n.order == (e.order + 1) && n.status == QuestEvent.EventStatus.WAITING)
            {
                n.UpdateQuestEvent(QuestEvent.EventStatus.CURRENT);
            }
        }
    }

    void SetObjectInactive()
    {
        inGameUI.SetActive(false);
    }

    IEnumerator ScaleTime(float start, float end, float time) {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < time) {
            Time.timeScale = Mathf.Lerp (start, end, timer / time);
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale = end;    
    }
}
