using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Collider from the GameObject
        Collider m_Collider = GetComponent<Collider>();
        //Fetch the center of the Collider volume
        Vector3 m_Center = m_Collider.bounds.center;
        //Fetch original height so it isn't lost
        //Debug.Log("m_Center: " + m_Center);
        float heightOrig = transform.position.y;
        //Center the terrain on 0,0. Keep original y
        m_Center.y = 0f;
        transform.position = -m_Center + new Vector3 (0,heightOrig,0);
        //Debug.Log("heightOrig: " + heightOrig);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
