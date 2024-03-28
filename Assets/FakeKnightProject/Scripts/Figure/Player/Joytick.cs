using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joytick : MonoBehaviour
{
    public bool isTick = false;
    void Start()
    {

    }
    public void choose(bool sta)
    {
        if (sta)
        {
            ObjUse.instance.bgSta.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            ObjUse.instance.sta.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
        else
        {
            ObjUse.instance.bgSta.transform.localScale = new Vector3(1f, 1f, 1f);
            ObjUse.instance.sta.transform.localScale = new Vector3(1f, 1f, 1f);
            ObjUse.instance.sta.transform.position = ObjUse.instance.bgSta.transform.position;
            isTick = false;
        }
    }
    public void setJoy(int i)
    {
        if (i == 2)
        {
            ObjUse.instance.sta.transform.position += new Vector3(0, 0.5f, 0);
        }
        else if (i == 4)
        {
            ObjUse.instance.sta.transform.position += new Vector3(-0.5f, 0, 0);
        }
        else if (i == 6)
        {
            ObjUse.instance.sta.transform.position += new Vector3(0.5f, 0, 0);
        }
        else
        {
            ObjUse.instance.sta.transform.position += new Vector3(0, -0.5f, 0);
        }
        isTick = true;
    }
}
