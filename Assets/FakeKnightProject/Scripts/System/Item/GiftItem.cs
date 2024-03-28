using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GiftItem : MonoBehaviour
{
    public GameObject panel;
    public GameObject paneled;
    public bool isGet = false;
    [SerializeField] private int type;
    [SerializeField] private int shop;
    [SerializeField] private int key;
    [SerializeField] private TMP_Text value;
    public string day;
    // Start is called before the first frame update
    void Start()
    {
        day = $"{DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}";
        Debug.Log(day);
    }
    public void getGift()
    {
        Debug.Log(LoadDataPlayer.instance.dataPlayer.day);
        if (day == LoadDataPlayer.instance.dataPlayer.day)
            Debug.Log("Trung");
        else
            Debug.Log("khac");
        if (isGet)
        {
            paneled.SetActive(true);
            isGet = false;
            LoadDataPlayer.instance.dataPlayer.gift += 1;
            LoadDataPlayer.instance.dataPlayer.day = day;
            int val = int.Parse(value.text);
            if (type == 0)
                LoadDataPlayer.instance.dataPlayer.gold += val;
            LoadDataPlayer.instance.moneyPlayer.setValue();
            LoadDataPlayer.instance.SaveDataGamePlayer();
        }    
    }    
}
