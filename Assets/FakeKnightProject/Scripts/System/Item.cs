using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Item : MonoBehaviour
{
    [SerializeField] private int shop;
    [SerializeField] private int key;
    [SerializeField] private int type;
    [SerializeField] private string _name;
    [SerializeField] private string properties, infor;
    [SerializeField] private int _price;
    [SerializeField] private float damage;
    [SerializeField] private float resist;
    [SerializeField] private float healing;
    [SerializeField] private TMP_Text name_, price;
    [SerializeField] private Image imgItem;
    [SerializeField] private Image imgMoney;
    [SerializeField] private InformationItem information;
    [SerializeField] private ObjectManager objectManager;
    void Start()
    {
        information = FindObjectOfType<InformationItem>();
        objectManager = FindObjectOfType<ObjectManager>();
        information.getItem(shop, key, ref _name, ref _price, ref type, ref properties, ref infor, ref damage, ref resist, ref healing);
        Debug.Log($"{shop}, {key}");
        Debug.Log(price);
        if (price)
            price.text = _price.ToString();
        if (name_)
            name_.text = _name;
    }
    public void getItem()
    {
        Debug.Log("getItem");
        Debug.Log($"{shop}, {key}");
        objectManager.uiBuy._name.text = _name;

        objectManager.notBuy.SetActive(false);
        objectManager.inforItem.SetActive(false);
        objectManager.buy.SetActive(!objectManager.buy.activeSelf);

        Debug.Log(imgItem.GetComponent<Image>().sprite);
        //objectManager.item = new Item();
        objectManager.item = gameObject.GetComponent<Item>();
    }    
    public void inforItem()
    {
        Debug.Log("getInforItem");
        Debug.Log($"{shop}, {key}");
        objectManager.uiInforItem._name.text = _name;
        objectManager.uiInforItem.properties.text = properties;
        objectManager.uiInforItem.damage.text = damage.ToString();
        objectManager.uiInforItem.resist.text = resist.ToString();
        objectManager.uiInforItem.healing.text = healing.ToString();
        objectManager.uiInforItem.infor.text = infor.ToString();

        objectManager.buy.SetActive(false);
        objectManager.notBuy.SetActive(false);
        objectManager.inforItem.SetActive(!objectManager.inforItem.activeSelf);

    }
    public void getBuyItem()
    {
        objectManager.buy.SetActive(false);

        // Kiểm tra xem objectManager.item có đang là null hay không
        if (objectManager.item != null)
        {
            Debug.Log(objectManager.moneyPlayer);
            if (!objectManager.moneyPlayer.isEnough(
                int.Parse(objectManager.item.price.text),
                objectManager.item.type,
                objectManager.item.shop,
                objectManager.item.key,
                objectManager.item.imgItem.sprite))
            {
                objectManager.notBuy.SetActive(true);
            }

            Debug.Log(objectManager.item.imgItem); // Kiểm tra imgItem của objectManager.item
        }
        else
        {
            Debug.LogError("objectManager.item is null!");
        }
    }
    //public void getBuyItem()
    //{
    //    objectManager.buy.SetActive(false);
    //    Debug.Log(objectManager.moneyPlayer);
    //    if (
    //    !objectManager.moneyPlayer.isEnough(
    //        int.Parse(objectManager.item.price.text),
    //        objectManager.item.type,
    //        objectManager.item.shop,
    //        objectManager.item.key,
    //        objectManager.item.imgItem.sprite))
    //    {
    //        objectManager.notBuy.SetActive(true);
    //    }    

    //    Debug.Log(imgItem);
    //}
}
