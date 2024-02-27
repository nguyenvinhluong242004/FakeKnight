using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] private int gold, diamondRed, diamondPurple; // type: 0, 1, 2
    [SerializeField] private ObjUse objUse;
    void Start()
    {
        objUse = FindObjectOfType<ObjUse>();
    }
    public void setValue()
    {
        Debug.Log(objUse);
        if (!objUse)
            objUse = FindObjectOfType<ObjUse>();
        gold = objUse.loadDataPlayer.dataPlayer.gold;
        diamondPurple = objUse.loadDataPlayer.dataPlayer.diamondPurple;
        diamondRed = objUse.loadDataPlayer.dataPlayer.diamondRed;
        objUse.textGold.text = gold.ToString();
        objUse.textDiamondPurple.text = diamondPurple.ToString();
        objUse.canvasTextGold.text = gold.ToString();
        objUse.canvasTextDiamondRed.text = diamondRed.ToString();
        objUse.canvasTextDiamondPurple.text = diamondPurple.ToString();
    }
    public bool isEnough(int price, int type, int shop, int key, Sprite img)
    {
        Debug.Log("money");
        if (diamondPurple >= price)
        {
            Debug.Log(objUse);
            if (!objUse)
                objUse = FindObjectOfType<ObjUse>();
            objUse.loadDataPlayer.dataPlayer.diamondPurple -= price;
            diamondPurple -= price;
            objUse.textDiamondPurple.text = diamondPurple.ToString();
            objUse.canvasTextDiamondPurple.text = diamondPurple.ToString();
            objUse.bagContent.initBagItem(shop, key, 1, type, img);
            Debug.Log(objUse.loadDataPlayer);
            objUse.loadDataPlayer.setCountItem(shop, key, true);
            return true;
        }
        return false;
    }
}
