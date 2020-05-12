using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGame : MonoBehaviour
{
    public GameObject inGameUI;
    private Animator inGameUIAnim;
    private Animator zombieAnim;
    public GameObject deathPopup;
    public float fadeUIAnimationLength = 4f;
    public string playerTagName = "Player";

    // Start is called before the first frame update
    void Start()
    {
        inGameUIAnim = inGameUI.GetComponent<Animator>();
        zombieAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider hopefullyPlayer)
    {
        if (hopefullyPlayer.gameObject.CompareTag(playerTagName) && zombieAnim.GetBool("isAttacking"))
        {
            Cursor.lockState = CursorLockMode.None;
            Invoke("SetObjectInactive",fadeUIAnimationLength);
            inGameUIAnim.SetBool("isFinished", true);
            deathPopup.SetActive(true);
            Animator deathAnim = deathPopup.GetComponent<Animator>();
            deathAnim.SetBool("isFinished", true);
            Cursor.visible = true;
            StartCoroutine(fadeTimeScale(fadeUIAnimationLength));
            return;
        }
    }

    void SetObjectInactive()
    {
        inGameUI.SetActive(false);
    }

    IEnumerator fadeTimeScale(float fadeTime)
    {
        float timer = 0.00f;
        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            Time.timeScale -= Time.deltaTime / fadeTime;
            yield return null;
        }
        Time.timeScale = 0f;
    }
}
