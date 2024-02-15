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
    [SerializeField] private LoadDataPlayer loadDataPlayer;
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
        Debug.Log(loadDataPlayer.dataPlayer.day);
        if (day == loadDataPlayer.dataPlayer.day)
            Debug.Log("Trung");
        else
            Debug.Log("khac");
        if (isGet)
        {
            paneled.SetActive(true);
            isGet = false;
            loadDataPlayer.dataPlayer.gift += 1;
            loadDataPlayer.dataPlayer.day = day;
            int val = int.Parse(value.text);
            if (type == 0)
                loadDataPlayer.dataPlayer.gold += val;
            loadDataPlayer.moneyPlayer.setValue();
            loadDataPlayer.SaveDataGamePlayer();
        }    
    }    
}
