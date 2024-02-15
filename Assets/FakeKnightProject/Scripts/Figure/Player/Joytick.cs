using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joytick : MonoBehaviour
{
    [SerializeField] private Transform bgr, joy;
    public bool isTick = false;
    public void choose(bool sta)
    {
        if (sta)
        {
            bgr.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            joy.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
        else
        {
            bgr.transform.localScale = new Vector3(1f, 1f, 1f);
            joy.transform.localScale = new Vector3(1f, 1f, 1f);
            joy.transform.position = bgr.transform.position;
            isTick = false;
        }
    }
    public void setJoy(int i)
    {
        if (i == 2)
        {
            joy.transform.position += new Vector3(0, 0.5f, 0);
        }
        else if (i == 4)
        {
            joy.transform.position += new Vector3(-0.5f, 0, 0);
        }
        else if (i == 6)
        {
            joy.transform.position += new Vector3(0.5f, 0, 0);
        }
        else
        {
            joy.transform.position += new Vector3(0, -0.5f, 0);
        }
        isTick = true;
    }
}
