using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] private int gold; // type: 0, 1, 2
    [SerializeField] private int diamondRed;
    [SerializeField] private int diamondPurple; 
    public void setValue()
    {
        gold = LoadDataPlayer.instance.dataPlayer.gold;
        diamondPurple = LoadDataPlayer.instance.dataPlayer.diamondPurple;
        diamondRed = LoadDataPlayer.instance.dataPlayer.diamondRed;
    }
}
