using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joytick : MonoBehaviour
{
    public bool isTick = false;
    [SerializeField] private ObjUse objUse;
    void Start()
    {
        objUse = FindObjectOfType<ObjUse>();
    }
    public void choose(bool sta)
    {
        if (sta)
        {
            objUse.bgSta.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            objUse.sta.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
        else
        {
            objUse.bgSta.transform.localScale = new Vector3(1f, 1f, 1f);
            objUse.sta.transform.localScale = new Vector3(1f, 1f, 1f);
            objUse.sta.transform.position = objUse.bgSta.transform.position;
            isTick = false;
        }
    }
    public void setJoy(int i)
    {
        if (i == 2)
        {
            objUse.sta.transform.position += new Vector3(0, 0.5f, 0);
        }
        else if (i == 4)
        {
            objUse.sta.transform.position += new Vector3(-0.5f, 0, 0);
        }
        else if (i == 6)
        {
            objUse.sta.transform.position += new Vector3(0.5f, 0, 0);
        }
        else
        {
            objUse.sta.transform.position += new Vector3(0, -0.5f, 0);
        }
        isTick = true;
    }
}
