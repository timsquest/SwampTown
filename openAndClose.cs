using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openAndClose : MonoBehaviour
{
    public string applicableInteractionText = "Press \"E\" to interact";
    public string boolTriggerName = "isOpen";
    public bool canReverse = true;
    public KeyCode interactionKey = KeyCode.E;
    public UnityEngine.UI.Text interationText;
    public Animator thisAnim;
    int playerLayerBit = 10;
    bool checkInput = false;
    public bool isOpen = true;
    public List<AudioSource> sounds = new List<AudioSource>();

    void Awake ()
    {
        interationText = GameObject.Find("InteractionText").GetComponent<UnityEngine.UI.Text>();
        thisAnim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (checkInput == true && Input.GetKeyDown(interactionKey) && this.isActiveAndEnabled)
        {
            bool currentState = thisAnim.GetBool(boolTriggerName);
            thisAnim.SetBool(boolTriggerName, !currentState);
            if (isOpen == true && sounds.Count > 0)
            {
                sounds[0].gameObject.transform.position = transform.position;
                sounds[0].Play();
            }
            else if (isOpen == false && sounds.Count > 1)
            {
                sounds[1].gameObject.transform.position = transform.position;
                sounds[1].Play();
            }
            isOpen = !isOpen;
            checkInput = false;
            if (canReverse == false)
            {
                Destroy(GetComponent<openAndClose>());
            }
        }
    }

    //Don't put this near a quest object, fields of interaction are not set up to overlap
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
            //Debug.Log(this.transform.Find(objectOfAim.gameObject.name));
            if (objectOfAim.gameObject.name == this.gameObject.name || (objectOfAim.gameObject.transform.parent != null && 
                objectOfAim.gameObject.transform.parent.gameObject.name == this.gameObject.name))
            {
                if (!this.isActiveAndEnabled)
                    return;
                interationText.gameObject.SetActive(true);
                interationText.text = applicableInteractionText;
            }
            else
            {
                checkInput = false;
                interationText.gameObject.SetActive(false);
                return;
            }
        }
        else
        {
            checkInput = false;
            return;
        }

        //Good to check for player interaction
        //GetInputDown placed in update so key down is not missed between frames
        checkInput = true;
    }

    void OnTriggerExit ()
    {
        checkInput = false;
        interationText.gameObject.SetActive(false);
    }

}