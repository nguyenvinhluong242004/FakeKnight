﻿using System.Collections;
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
    void Start()
    {
        information = FindObjectOfType<InformationItem>();
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
        ObjectManager.instance.uiBuy._name.text = _name;

        ObjectManager.instance.notBuy.SetActive(false);
        ObjectManager.instance.inforItem.SetActive(false);
        ObjectManager.instance.buy.SetActive(!ObjectManager.instance.buy.activeSelf);

        Debug.Log(imgItem.GetComponent<Image>().sprite);
        //ObjectManager.instance.item = new Item();
        ObjectManager.instance.item = gameObject.GetComponent<Item>();
    }    
    public void inforItem()
    {
        Debug.Log("getInforItem");
        Debug.Log($"{shop}, {key}");
        ObjectManager.instance.uiInforItem._name.text = _name;
        ObjectManager.instance.uiInforItem.properties.text = properties;
        ObjectManager.instance.uiInforItem.damage.text = damage.ToString();
        ObjectManager.instance.uiInforItem.resist.text = resist.ToString();
        ObjectManager.instance.uiInforItem.healing.text = healing.ToString();
        ObjectManager.instance.uiInforItem.infor.text = infor.ToString();

        ObjectManager.instance.buy.SetActive(false);
        ObjectManager.instance.notBuy.SetActive(false);
        ObjectManager.instance.inforItem.SetActive(!ObjectManager.instance.inforItem.activeSelf);

    }
    public void getBuyItem()
    {
        ObjectManager.instance.buy.SetActive(false);

        // Kiểm tra xem ObjectManager.instance.item có đang là null hay không
        if (ObjectManager.instance.item != null)
        {
            Debug.Log(ObjectManager.instance.objUse.moneyPlayer);
            if (!ObjectManager.instance.objUse.moneyPlayer.isEnough(
                int.Parse(ObjectManager.instance.item.price.text),
                ObjectManager.instance.item.type,
                ObjectManager.instance.item.shop,
                ObjectManager.instance.item.key,
                ObjectManager.instance.item.imgItem.sprite))
            {
                ObjectManager.instance.notBuy.SetActive(true);
            }

            Debug.Log(ObjectManager.instance.item.imgItem); // Kiểm tra imgItem của ObjectManager.instance.item
        }
        else
        {
            Debug.LogError("ObjectManager.instance.item is null!");
        }
    }
    //public void getBuyItem()
    //{
    //    ObjectManager.instance.buy.SetActive(false);
    //    Debug.Log(ObjectManager.instance.moneyPlayer);
    //    if (
    //    !ObjectManager.instance.moneyPlayer.isEnough(
    //        int.Parse(ObjectManager.instance.item.price.text),
    //        ObjectManager.instance.item.type,
    //        ObjectManager.instance.item.shop,
    //        ObjectManager.instance.item.key,
    //        ObjectManager.instance.item.imgItem.sprite))
    //    {
    //        ObjectManager.instance.notBuy.SetActive(true);
    //    }    

    //    Debug.Log(imgItem);
    //}
}