using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public GameObject blBGR, bl;
    [SerializeField] float limitBl;
    [SerializeField] float po;
    public void setBlood(float blood)
    {
        float k = blood / limitBl;
        if (k < 0f)
            k = 0f;
        bl.transform.localScale = new Vector3(k, bl.transform.localScale.y, bl.transform.localScale.z);
        bl.transform.position = new Vector3(blBGR.transform.position.x - po * (1 - k), bl.transform.position.y, bl.transform.position.z);
        //Debug.Log(bl.transform.position);
    }   
}
