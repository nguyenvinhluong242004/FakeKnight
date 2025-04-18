﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skill : MonoBehaviour
{
    public GameObject obj, cir;
    //[SerializeField] private PlayerMove player;
    [SerializeField] private TMP_Text timeLoop;
    public bool isOn;
    float alpha;
    float elapsedTime;
    [SerializeField] private float totalTime;
    [SerializeField] private int sk;
    void Start()
    {
        isOn = false;
        cir.SetActive(false);
    }

    void Update()
    {
        if (isOn)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < totalTime)
            {
                float angle = Mathf.Lerp(0f, 360f, elapsedTime / totalTime);
                transform.eulerAngles = new Vector3(0, 0, -angle);
                timeLoop.text = (totalTime - elapsedTime).ToString("F1");
            }
            else
            {
                ResetSK();
                if (sk == 1)
                {
                    Debug.Log("reset");
                    ObjUse.instance.player.isChooseSk1 = false;
                }    
                else if(sk == 2)
                    ObjUse.instance.player.twoSkill = false;
                else if (sk == 3)
                    ObjUse.instance.player.threeSkill = false;
                else if (sk == 4)
                    ObjUse.instance.player.fourSkill = false;
            }
        }
    }
    public void timeSkill()
    {
        cir.SetActive(true);
    }
    public void ResetSK()
    {
        transform.eulerAngles = Vector3.zero;
        isOn = false;
        cir.SetActive(false);
        elapsedTime = 0f;
    }
}
