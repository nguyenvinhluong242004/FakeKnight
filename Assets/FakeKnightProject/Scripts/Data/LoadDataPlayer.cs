 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using System;

public class LoadDataPlayer : MonoBehaviour
{
    [SerializeField] public static LoadDataPlayer instance;
    [SerializeField] private GameObject photonManager;
    [SerializeField] public DataPlayer dataPlayer;
    [SerializeField] public string namePlayer;
    [SerializeField] public string nameInGame;
    [SerializeField] public string playfabID;
    [SerializeField] private BagContent bagContent;
    [SerializeField] private TMP_Text _name;
    [SerializeField] public MoneyPlayer moneyPlayer;
    [SerializeField] public LoadingGame loadingGame;
    [SerializeField] private InformationItem informationItem;
    public bool checkDone = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        GetAppearance();
        getUserNamePlayer();
        PlayfabFriendList.instance.UpdateFriend();
        //loadingGame.textNote.text = "start";
    }
    void getUserNamePlayer()
    {
        GetPlayerProfileRequest request = new GetPlayerProfileRequest();
        PlayFabClientAPI.GetPlayerProfile(request, OnGetPlayerProfileSuccess, OnError);
    }    
    private void OnGetPlayerProfileSuccess(GetPlayerProfileResult result)
    {
        // get username
        namePlayer = result.PlayerProfile.DisplayName;
        PhotonChat.instance.senderPlayer = namePlayer;
        string idPlayer = result.PlayerProfile.PlayerId;
        playfabID = idPlayer;
        dataPlayer.PlayFabID = idPlayer;
        Debug.Log("ID: " + idPlayer);
        Debug.Log("PlayFab Username: " + namePlayer);

        //loadingGame.textNote.text = "OnGetPlayerProfileSuccess";
    }
    public void GetAppearance()  // Get id to player present
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceive, OnError);
    }
    void OnDataReceive(GetUserDataResult result) // get datagame user
    {
        if (result.Data != null && result.Data.ContainsKey("DataGamePlayer")) // bug
        { 
            
            Debug.Log("Sucessful Data Receive!");

            //loadingGame.textNote.text = "Sucessful Data Receive!";

            //Debug.Log(result.Data["DataGamePlayer"].Value); // In ra dữ liệu JSON
            DataGame dataGame = JsonConvert.DeserializeObject<DataGame>(result.Data["DataGamePlayer"].Value);

            dataPlayer.setDataGame(dataGame);// Data ở playfab web null
            _name.text = dataPlayer.name;
            nameInGame = dataPlayer.name;
            Debug.Log(dataGame.equipments[0]);
            SetValue();
            setData();
            checkDone = true;
        }
        else
        {

            Debug.Log("Get Data No Done!");
            //loadingGame.textNote.text = "Get Data No Done!";
        }
    }
    
    public void setData()
    {
        //setPlayer();
        setItemBag();
        // set percent for player
        setTotalPercentToObjUse();
        Debug.Log("loadgame.isdone");
        loadingGame.textNote.text = "loadgame.isdone";
        loadingGame.isDone = true;
        if (loadingGame.isDone && loadingGame.isDonePhoton)
            loadingGame.setLoadingGame(true);
        photonManager.SetActive(true);
        loadingGame.textNote.text = "set value data sucess!";
    }    
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        //loadingGame.textNote.text = "OnError";
    }
    public void SaveDataGamePlayer()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                {
                    { "DataGamePlayer", JsonConvert.SerializeObject(dataPlayer.ReturnClass()) }
                }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Sucessful Data Send!");
    }
    public void setTotalPercentToObjUse()
    {
        float totalDamage = 0;     // phần trăm sát thương
        float totalResistance = 0; // phần trăm chống chịu
        float totalSpeed = 0;      // phần trăm tốc chạy


        // xử lí equipment ( giày, giáp, mũ ...)
        for (int i = 0; i < dataPlayer.equipments.Length; i++)
        {
            int idItem = dataPlayer.equipments[i];
            if (idItem >= 0)
            {
                Debug.Log(InformationItem.instance.data[0, idItem].name);
                totalDamage += InformationItem.instance.data[0, idItem].damage;
                totalResistance += InformationItem.instance.data[0, idItem].resist;
                totalSpeed += InformationItem.instance.data[0, idItem].speed;
            }
        }

        ObjUse.instance.damageTotal = totalDamage;
        ObjUse.instance.resistanceTotal = totalResistance;
        ObjUse.instance.speedTotal = totalSpeed;

        ObjUse.instance.setTextPercent();
    }
    public void setItemBag() // set các item trong túi đồ
    {
        loadingGame.textNote.text = "set item bag!";
        for (int i=0; i<8; i++)
            if (dataPlayer.equipments[i] != -1)
            {
                dataPlayer.items[0, dataPlayer.equipments[i]] -= 1;
            }

        loadingGame.textNote.text = "set item bag 1!";
        Debug.Log(dataPlayer.items.GetLength(0));
        Debug.Log(dataPlayer.items.GetLength(1));



        int[] columnsPerRow = { 17, 12, 12, 7 };
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < columnsPerRow[row]; col++)
            {
                int itemValue = dataPlayer.items[row, col];
                loadingGame.textNote.text = row.ToString() + " " + col.ToString();
                loadingGame.textNote.text = row.ToString() + " " + col.ToString() + " " + "null" + informationItem.data[row, col].type.ToString();
                if (itemValue != 0)
                {
                    if (ObjectManager.instance.imagesS1[col] == null)
                    {
                        loadingGame.textNote.text = row.ToString() + " " + col.ToString() + " " + "null";
                    }
                    if (row == 0)
                    {
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS1[col]);
                        loadingGame.textNote.text = row.ToString() + " " + col.ToString() + " " + "line 1";
                    }
                    else if (row == 1)
                    {
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS2[col]);
                        loadingGame.textNote.text = row.ToString() + " " + col.ToString() + " " + "line 2";
                    }
                    else if (row == 2)
                    {
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS3[col]);
                        loadingGame.textNote.text = row.ToString() + " " + col.ToString() + " " + "line 3";
                    }
                    else
                    {
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS4[col]);
                        loadingGame.textNote.text = row.ToString() + " " + col.ToString() + " " + "line 4";
                    }
                }
            }

            loadingGame.textNote.text = "set item bag 111!";
        }

        loadingGame.textNote.text = "set item bag 2!";
        for (int i = 0; i < 8; i++)
            if (dataPlayer.equipments[i] != -1)
            {
                dataPlayer.items[0, dataPlayer.equipments[i]] += 1;
            }

        loadingGame.textNote.text = "set item bag sucess!";
    } 
    public void setCountItem(int shop, int key, bool k) // thay đổi số lượng item
    {
        if (k)
            dataPlayer.items[shop, key]++;
        else
            dataPlayer.items[shop, key]--;
        SaveDataGamePlayer();
    } 
    public void setMusic(bool k)
    {
        dataPlayer.isMusic = k;
        SaveDataGamePlayer();
    }
    public void setSound(bool k)
    {
        dataPlayer.isSound = k;
        SaveDataGamePlayer();
    }
    public void setPlayer() // set hình ảnh, âm thanh
    {
        //ObjectManager.instance.imgProfile.sprite = ObjectManager.instance.imgPlayers[dataPlayer.idPlayer];

        //// get Music, Sound Playerr
        //ObjectManager.instance.isSound = dataPlayer.isSound;
        //if (dataPlayer.isSound)
        //    ObjectManager.instance.
        //if (dataPlayer.isMusic)
        //    ObjectManager.instance.musicAudio.Play();
        //ObjectManager.instance._muic.isOn = dataPlayer.isMusic;
    } 
    public void SetValue()
    {
        Debug.Log(ObjUse.instance);
        if (moneyPlayer)
        {
            moneyPlayer.setValue();
        }
        ObjUse.instance.textGold.text = dataPlayer.gold.ToString();
        ObjUse.instance.textDiamondPurple.text = dataPlayer.diamondPurple.ToString();
        ObjUse.instance.canvasTextGold.text = dataPlayer.gold.ToString();
        ObjUse.instance.canvasTextDiamondRed.text = dataPlayer.diamondRed.ToString();
        ObjUse.instance.canvasTextDiamondPurple.text = dataPlayer.diamondPurple.ToString();
    }
    public void GetKillEnemy(int _gold)
    {
        dataPlayer.gold += _gold;
        SetValue();
        SaveDataGamePlayer();
    }
    public bool isEnough(int price, int type, int shop, int key, Sprite img)
    {
        Debug.Log("money");
        Debug.Log(price);
        if (dataPlayer.diamondPurple >= price)
        {
            Debug.Log(ObjUse.instance);
            dataPlayer.diamondPurple -= price;
            ObjUse.instance.textDiamondPurple.text = dataPlayer.diamondPurple.ToString();
            ObjUse.instance.canvasTextDiamondPurple.text = dataPlayer.diamondPurple.ToString();
            ObjUse.instance.bagContent.initBagItem(shop, key, 1, type, img);
            setCountItem(shop, key, true);
            return true;
        }
        return false;
    }
}
