using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public GameObject pointer;
    public GameObject player;
    public RectTransform compassLine;
    public float maxAngle = 75f;
    public CompassTarget currentQuestTarget;
    public List<CompassTarget> otherTargets = new List<CompassTarget>();

    // Update is called once per frame
    void Update()
    {
        UpdateIconOnCompass(currentQuestTarget);
        foreach (CompassTarget ct in otherTargets)
        {
            UpdateIconOnCompass(ct, false);
        }
    }

    void UpdateIconOnCompass (CompassTarget ct, bool isFixed = true)
    {
        Vector3[] v = new Vector3[4];
        compassLine.GetLocalCorners(v);
        float pointerScale = Vector3.Distance(v[1], v[2]);

        Vector3 direction = new Vector3 (ct.target.transform.position.x, player.transform.position.y, ct.target.transform.position.z) - player.transform.position;
        float angleToTarget = Vector3.SignedAngle(direction.normalized, player.transform.forward.normalized, player.transform.up);
        if (Mathf.Abs(angleToTarget) > maxAngle && !isFixed)
        {
            ct.icon.SetActive(false);
            return;
        }
        else
        {
            ct.icon.SetActive(true);
        }
        angleToTarget = Mathf.Clamp(-angleToTarget, -maxAngle, maxAngle) / 180.0f * pointerScale;
        ct.SetRectPosition(new Vector3(angleToTarget, ct.GetRect().localPosition.y, ct.GetRect().localPosition.z));
    }
}

[System.Serializable]
public class CompassTarget
{
    public GameObject target;
    public GameObject icon;
    private RectTransform rect;

    public CompassTarget(GameObject t, GameObject i)
    {
        target = t;
        icon = i;
        rect = icon.GetComponent<RectTransform>();
    }

    public RectTransform GetRect ()
    {
        if (!rect)
            rect = icon.GetComponent<RectTransform>();
        return rect;
    }

    public void SetRect (RectTransform newRect)
    {
        rect = newRect;
    }

    public void SetRectPosition (Vector3 newRectPosition)
    {
        rect.localPosition = newRectPosition;
    }
}