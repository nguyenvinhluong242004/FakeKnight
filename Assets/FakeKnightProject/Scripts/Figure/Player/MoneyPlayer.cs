using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] private int gold, diamondRed, diamondPurple; // type: 0, 1, 2
    [SerializeField] private TMP_Text textGold, textDiamondPurple, canvasTextGold, canvasTextDiamondRed, canvasTextDiamondPurple;
    [SerializeField] private BagContent bagContent;
    [SerializeField] private LoadDataPlayer loadDataPlayer;
    public void setValue()
    {
        gold = loadDataPlayer.dataPlayer.gold;
        diamondPurple = loadDataPlayer.dataPlayer.diamondPurple;
        diamondRed = loadDataPlayer.dataPlayer.diamondRed;
        textGold.text = gold.ToString();
        textDiamondPurple.text = diamondPurple.ToString();
        canvasTextGold.text = gold.ToString();
        canvasTextDiamondRed.text = diamondRed.ToString();
        canvasTextDiamondPurple.text = diamondPurple.ToString();
    }
    public bool isEnough(int price, int type, int shop, int key, Sprite img)
    {
        Debug.Log("money");
        if (diamondPurple >= price)
        {
            loadDataPlayer.dataPlayer.diamondPurple -= price;
            diamondPurple -= price;
            textDiamondPurple.text = diamondPurple.ToString();
            canvasTextDiamondPurple.text = diamondPurple.ToString();
            bagContent.initBagItem(shop, key, 1, type, img);
            Debug.Log(loadDataPlayer);
            loadDataPlayer.setCountItem(shop, key, true);
            return true;
        }
        return false;
    }
}
