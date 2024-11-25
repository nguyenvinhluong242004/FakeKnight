using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public void getLeaveGame()
    {
        if (ObjectManager.instance.scenePlay.activeSelf)
            ObjectManager.instance.leaveGame.SetActive(!ObjectManager.instance.leaveGame.activeSelf);
    }
    public void closeFriendPrivateChat()
    {
        ObjectManager.instance.chatFriend.SetActive(false);
    }
    public void getFriendList()
    {
        ObjectManager.instance.friend.SetActive(!ObjectManager.instance.friend.activeSelf);
    }
    public void getMails()
    {
        ObjectManager.instance.mails.SetActive(!ObjectManager.instance.mails.activeSelf);
    }    
    public void setMusic()
    {
        ObjectManager.instance.isMusic = !ObjectManager.instance.isMusic;
        LoadDataPlayer.instance.setMusic(ObjectManager.instance.isMusic);
        if (ObjUse.instance.dataPlayer.isMusic)
        {
            ObjectManager.instance.musicAudio.Play();
            ObjectManager.instance.music.GetComponent<Image>().sprite = ObjectManager.instance.musicOn;
        }
        else
        {
            ObjectManager.instance.musicAudio.Stop();
            ObjectManager.instance.music.GetComponent<Image>().sprite = ObjectManager.instance.musicOff;
        }
    }
    public void setSound()
    {
        ObjectManager.instance.isSound = !ObjectManager.instance.isSound;
        LoadDataPlayer.instance.setSound(ObjectManager.instance.isSound);
        if (ObjUse.instance.dataPlayer.isSound)
        {
            ObjectManager.instance.sound.GetComponent<Image>().sprite = ObjectManager.instance.musicOn;
        }
        else
        {
            ObjectManager.instance.sound.GetComponent<Image>().sprite = ObjectManager.instance.musicOff;
        }
    }
    public void getInfor()
    {
        ObjectManager.instance.infor.SetActive(!ObjectManager.instance.infor.activeSelf);
    }
    public void getBag()
    {
        ObjectManager.instance.bag.SetActive(!ObjectManager.instance.bag.activeSelf);
        if(!ObjectManager.instance.bag.activeSelf)
        {
            if (ObjectManager.instance.bagItem != null)
                ObjectManager.instance.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
            ObjectManager.instance.isSetItem = false;
        }    
    }
    public void getGift()
    {
        ObjectManager.instance.gift.SetActive(!ObjectManager.instance.gift.activeSelf);
    }    
    public void getShop()
    {
        ObjectManager.instance.shop.SetActive(!ObjectManager.instance.shop.activeSelf);
        if(!ObjectManager.instance.shop.activeSelf)
        {
            getCloseUiInforItem();
            getCloseUiBuyItem();
            getCloseUiNotBuy();
        }    
    }
    public void getMap()
    {
        ObjectManager.instance.map.SetActive(!ObjectManager.instance.map.activeSelf);
        ObjectManager.instance.mapControl.setPositionMap(ObjectManager.instance.poX, ObjectManager.instance.poY);
        if (ObjectManager.instance.pastMap != null)
        {
            ObjectManager.instance.pastMap.GetComponent<Image>().sprite = ObjectManager.instance.redImage;
        }
        //objectManager.positionPlayer.anchoredPosition = new Vector2(objectManager.poX, objectManager.poY);
    }
    public void getLocalMap()
    {
        if (ObjectManager.instance.pastMap != null)
            ObjectManager.instance.pastMap.GetComponent<Image>().sprite = ObjectManager.instance.redImage;
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
            image.sprite = ObjectManager.instance.greenImage;
        if (gameObject.name == "Idronia")
        {
            ObjectManager.instance.mapControl.setPositionMap(-100, 140);
        }
        else if (gameObject.name == "Island")
        {
            ObjectManager.instance.mapControl.setPositionMap(460, 75);
        }
        else if (gameObject.name == "Valley")
        {
            ObjectManager.instance.mapControl.setPositionMap(423, -274);
        }
        else
        {
            ObjectManager.instance.mapControl.setPositionMap(168, -68);
        }

        ObjectManager.instance.pastMap = gameObject;
    }
    public void getSetting()
    {
        ObjectManager.instance.setting.SetActive(!ObjectManager.instance.setting.activeSelf);
    }
    public void getChat()
    {
        ObjectManager.instance.chat.SetActive(!ObjectManager.instance.chat.activeSelf);
    }
    public void getEmote()
    {
        ObjectManager.instance.emote.SetActive(!ObjectManager.instance.emote.activeSelf);
    }
    public void getAccessory()
    {
        if(ObjectManager.instance.pastButton!=null)
            ObjectManager.instance.pastButton.GetComponent<Image>().sprite = ObjectManager.instance.redImage;
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
            image.sprite = ObjectManager.instance.greenImage;
        if(gameObject != ObjectManager.instance.pastButton)
        {
            if (gameObject.name == "Equipment")
            {
                ObjectManager.instance.pastShop.SetActive(!ObjectManager.instance.pastShop.activeSelf);
                ObjectManager.instance.shopEquipment.SetActive(!ObjectManager.instance.shopEquipment.activeSelf);
                ObjectManager.instance.pastShop = ObjectManager.instance.shopEquipment;
            }
            else if (gameObject.name == "Auxiliary")
            {
                ObjectManager.instance.pastShop.SetActive(!ObjectManager.instance.pastShop.activeSelf);
                ObjectManager.instance.shopAuxiliary.SetActive(!ObjectManager.instance.shopAuxiliary.activeSelf);
                ObjectManager.instance.pastShop = ObjectManager.instance.shopAuxiliary;
            }
            else if (gameObject.name == "Upgrade")
            {
                ObjectManager.instance.pastShop.SetActive(!ObjectManager.instance.pastShop.activeSelf);
                ObjectManager.instance.shopUpgrate.SetActive(!ObjectManager.instance.shopUpgrate.activeSelf);
                ObjectManager.instance.pastShop = ObjectManager.instance.shopUpgrate;
            }
            else
            {
                ObjectManager.instance.pastShop.SetActive(!ObjectManager.instance.pastShop.activeSelf);
                ObjectManager.instance.shopGeneral.SetActive(!ObjectManager.instance.shopGeneral.activeSelf);
                ObjectManager.instance.pastShop = ObjectManager.instance.shopGeneral;
            }
            ObjectManager.instance.buy.SetActive(false);
            ObjectManager.instance.inforItem.SetActive(false);
            ObjectManager.instance.notBuy.SetActive(false);
        }
        ObjectManager.instance.pastButton = gameObject;
    }
    public void getCloseUiInforItem()
    {
        ObjectManager.instance.inforItem.SetActive(false);
    }
    public void getCloseUiRemoveEquip()
    {
        ObjectManager.instance.removeEquip.SetActive(false);
        ObjectManager.instance.bagItem = null;
    }    
    public void getRemoveEquip()
    {
        if (ObjectManager.instance.equip)
        {
            ObjectManager.instance.equip.GetComponent<Image>().sprite = ObjectManager.instance.imgUseItem;
            ObjectManager.instance.equip.GetComponent<BagItem>().isUse = false;
            ObjectManager.instance.equip.GetComponent<BagItem>().add.SetActive(true);
            LoadDataPlayer.instance.dataPlayer.equipments[ObjectManager.instance.equip.GetComponent<BagItem>().idx] = -1;

            Debug.Log(ObjectManager.instance.bagItem.shop + " bb " + ObjectManager.instance.bagItem.key);
            float _damage = InformationItem.instance.data[ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key].damage;
            float _resist = InformationItem.instance.data[ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key].resist;
            float _speed = InformationItem.instance.data[ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key].speed;
            ObjUse.instance.changePercent(_damage, _resist, _speed, "sub");

            ObjectManager.instance.equip.GetComponent<BagItem>().bagContent.initBagItem(ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key, 1, ObjectManager.instance.bagItem.type, ObjectManager.instance.bagItem.img.sprite);
            ObjectManager.instance.equip.GetComponent<BagItem>().bagItem = null;
            ObjectManager.instance.equip.GetComponent<BagItem>().img = null;
            ObjectManager.instance.equip.GetComponent<BagItem>().key = 0;


            ObjectManager.instance.bagItem = null;
            ObjectManager.instance.equip = null;
            ObjectManager.instance.removeEquip.SetActive(false);
            LoadDataPlayer.instance.SaveDataGamePlayer();
        }    
    }    
    public void getCloseUiNotBuy()
    {
        ObjectManager.instance.notBuy.SetActive(false);
    }    
    public void getCloseUiBuyItem()
    {
        ObjectManager.instance.buy.SetActive(false);
    }
}
