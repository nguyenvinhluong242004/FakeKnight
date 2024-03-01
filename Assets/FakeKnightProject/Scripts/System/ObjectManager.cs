using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] public AudioSource audio;
    [SerializeField] public LoadDataPlayer loadDataPlayer;
    [SerializeField] public bool isSound;
    [SerializeField] public Toggle _muic, _sound;
    [SerializeField] public GameObject infor, shop, message, chat, emote, bag, removeEquip, inforItem, buy, notBuy, setting, map, gift;
    [SerializeField] public GameObject shopEquipment, shopAuxiliary, shopUpgrate, shopGeneral;
    [SerializeField] public Sprite greenImage, redImage, imgUseItem;
    [SerializeField] public GameObject pastButton, pastShop, pastMap;
    [SerializeField] public bool isSetItem;
    [SerializeField] public BagItem bagItem;
    [SerializeField] public GameObject equip;
    [SerializeField] public UiInforItem uiInforItem;
    [SerializeField] public UiBuy uiBuy;
    [SerializeField] public ObjUse objUse;
    [SerializeField] public Item item;
    [SerializeField] public Image imgProfile;
    [SerializeField] public RuntimeAnimatorController[] anmPlayers;
    [SerializeField] public Sprite[] imgPlayers;
    [SerializeField] public Sprite[] imagesS1, imagesS2, imagesS3, imagesS4;
    [SerializeField] public RectTransform positionPlayer;
    [SerializeField] public MapControl mapControl;
    [SerializeField] public int poX, poY;


    void Start()
    {
        shop.SetActive(false);
        bag.SetActive(false);
        Debug.Log(positionPlayer.anchoredPosition);
    }
}
