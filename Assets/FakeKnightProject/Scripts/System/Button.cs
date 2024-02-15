using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    ObjectManager objectManager;
    // Start is called before the first frame update
    void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
    }
    public void setMusic()
    {
        objectManager.loadDataPlayer.setMusic(objectManager._muic.isOn);
        if (objectManager._muic.isOn)
        {
            objectManager.audio.Play();
        }
        else
        {
            objectManager.audio.Stop();
        }
    }
    public void setSound()
    {
        objectManager.loadDataPlayer.setSound(objectManager._sound.isOn);
        objectManager.isSound = objectManager._sound.isOn;
    }
    public void getInfor()
    {
        objectManager.infor.SetActive(!objectManager.infor.activeSelf);
    }
    public void getBag()
    {
        objectManager.bag.SetActive(!objectManager.bag.activeSelf);
        if(!objectManager.bag.activeSelf)
        {
            if (objectManager.bagItem != null)
                objectManager.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
            objectManager.isSetItem = false;
        }    
    }
    public void getGift()
    {
        objectManager.gift.SetActive(!objectManager.gift.activeSelf);
    }    
    public void getShop()
    {
        objectManager.shop.SetActive(!objectManager.shop.activeSelf);
        if(!objectManager.shop.activeSelf)
        {
            getCloseUiInforItem();
            getCloseUiBuyItem();
            getCloseUiNotBuy();
        }    
    }
    public void getMap()
    {
        objectManager.map.SetActive(!objectManager.map.activeSelf);
        objectManager.mapControl.setPositionMap(objectManager.poX, objectManager.poY);
        if (objectManager.pastMap != null)
        {
            objectManager.pastMap.GetComponent<Image>().sprite = objectManager.redImage;
        }
        //objectManager.positionPlayer.anchoredPosition = new Vector2(objectManager.poX, objectManager.poY);
    }
    public void getLocalMap()
    {
        if (objectManager.pastMap != null)
            objectManager.pastMap.GetComponent<Image>().sprite = objectManager.redImage;
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
            image.sprite = objectManager.greenImage;
        if (gameObject.name == "Idronia")
        {
            objectManager.mapControl.setPositionMap(-100, 140);
        }
        else if (gameObject.name == "Island")
        {
            objectManager.mapControl.setPositionMap(460, 75);
        }
        else if (gameObject.name == "Valley")
        {
            objectManager.mapControl.setPositionMap(423, -274);
        }
        else
        {
            objectManager.mapControl.setPositionMap(168, -68);
        }
        
        objectManager.pastMap = gameObject;
    }
    public void getSetting()
    {
        objectManager.setting.SetActive(!objectManager.setting.activeSelf);
    }
    public void getChat()
    {
        objectManager.chat.SetActive(!objectManager.chat.activeSelf);
    }
    public void getEmote()
    {
        objectManager.emote.SetActive(!objectManager.emote.activeSelf);
    }
    public void getAccessory()
    {
        if(objectManager.pastButton!=null)
            objectManager.pastButton.GetComponent<Image>().sprite = objectManager.redImage;
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
            image.sprite = objectManager.greenImage;
        if(gameObject!=objectManager.pastButton)
        {
            if (gameObject.name == "Equipment")
            {
                objectManager.pastShop.SetActive(!objectManager.pastShop.activeSelf);
                objectManager.shopEquipment.SetActive(!objectManager.shopEquipment.activeSelf);
                objectManager.pastShop = objectManager.shopEquipment;
            }
            else if (gameObject.name == "Auxiliary")
            {
                objectManager.pastShop.SetActive(!objectManager.pastShop.activeSelf);
                objectManager.shopAuxiliary.SetActive(!objectManager.shopAuxiliary.activeSelf);
                objectManager.pastShop = objectManager.shopAuxiliary;
            }
            else if (gameObject.name == "Upgrate")
            {
                objectManager.pastShop.SetActive(!objectManager.pastShop.activeSelf);
                objectManager.shopUpgrate.SetActive(!objectManager.shopUpgrate.activeSelf);
                objectManager.pastShop = objectManager.shopUpgrate;
            }
            else
            {
                objectManager.pastShop.SetActive(!objectManager.pastShop.activeSelf);
                objectManager.shopGeneral.SetActive(!objectManager.shopGeneral.activeSelf);
                objectManager.pastShop = objectManager.shopGeneral;
            }
            objectManager.buy.SetActive(false);
            objectManager.inforItem.SetActive(false);
            objectManager.notBuy.SetActive(false);
        }    
        objectManager.pastButton = gameObject;
    }
    public void getCloseUiInforItem()
    {
        objectManager.inforItem.SetActive(false);
    }
    public void getCloseUiRemoveEquip()
    {
        objectManager.removeEquip.SetActive(false);
        objectManager.bagItem = null;
    }    
    public void getRemoveEquip()
    {
        if (objectManager.equip)
        {
            objectManager.equip.GetComponent<Image>().sprite = objectManager.imgUseItem;
            objectManager.equip.GetComponent<BagItem>().isUse = false;
            objectManager.equip.GetComponent<BagItem>().add.SetActive(true);
            objectManager.loadDataPlayer.dataPlayer.equipments[objectManager.equip.GetComponent<BagItem>().idx] = -1;
            objectManager.equip.GetComponent<BagItem>().bagContent.initBagItem(objectManager.bagItem.shop, objectManager.bagItem.key, 1, objectManager.bagItem.type, objectManager.bagItem.img.sprite);
            objectManager.equip.GetComponent<BagItem>().bagItem = null;
            objectManager.equip.GetComponent<BagItem>().img = null;
            objectManager.equip.GetComponent<BagItem>().key = 0;
            objectManager.bagItem = null;
            objectManager.equip = null; 
            objectManager.removeEquip.SetActive(false);
            objectManager.loadDataPlayer.SaveDataGamePlayer();
        }    
    }    
    public void getCloseUiNotBuy()
    {
        objectManager.notBuy.SetActive(false);
    }    
    public void getCloseUiBuyItem()
    {
        objectManager.buy.SetActive(false);
    }
}
