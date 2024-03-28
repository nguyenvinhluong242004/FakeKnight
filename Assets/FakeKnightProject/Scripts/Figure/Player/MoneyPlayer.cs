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
    void Start()
    {

    }
    public void setValue()
    {
        Debug.Log(ObjUse.instance);
        gold = LoadDataPlayer.instance.dataPlayer.gold;
        diamondPurple = LoadDataPlayer.instance.dataPlayer.diamondPurple;
        diamondRed = LoadDataPlayer.instance.dataPlayer.diamondRed;
        ObjUse.instance.textGold.text = gold.ToString();
        ObjUse.instance.textDiamondPurple.text = diamondPurple.ToString();
        ObjUse.instance.canvasTextGold.text = gold.ToString();
        ObjUse.instance.canvasTextDiamondRed.text = diamondRed.ToString();
        ObjUse.instance.canvasTextDiamondPurple.text = diamondPurple.ToString();
    }
    public bool isEnough(int price, int type, int shop, int key, Sprite img)
    {
        Debug.Log("money");
        if (diamondPurple >= price)
        {
            Debug.Log(ObjUse.instance);
            LoadDataPlayer.instance.dataPlayer.diamondPurple -= price;
            diamondPurple -= price;
            ObjUse.instance.textDiamondPurple.text = diamondPurple.ToString();
            ObjUse.instance.canvasTextDiamondPurple.text = diamondPurple.ToString();
            ObjUse.instance.bagContent.initBagItem(shop, key, 1, type, img);
            Debug.Log(LoadDataPlayer.instance);
            LoadDataPlayer.instance.setCountItem(shop, key, true);
            return true;
        }
        return false;
    }
}
