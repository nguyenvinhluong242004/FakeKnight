using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class GiftContent : MonoBehaviour
{
    [SerializeField] private DataPlayer dataPlayer;
    public GiftItem[] giftItems;
    void Start()
    {
        Debug.Log(dataPlayer.gift);
        Debug.Log(dataPlayer.day);
        giftItems = GetComponentsInChildren<GiftItem>();
        for (int i=0; i<dataPlayer.gift; i++)
        {
            giftItems[i].panel.SetActive(false);
            giftItems[i].paneled.SetActive(true);
        }
        string day = $"{DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}";
        if (day != dataPlayer.day && dataPlayer.gift < 31)
        {
            giftItems[dataPlayer.gift].panel.SetActive(false);
            giftItems[dataPlayer.gift].isGet = true;
        }    
    }
}
