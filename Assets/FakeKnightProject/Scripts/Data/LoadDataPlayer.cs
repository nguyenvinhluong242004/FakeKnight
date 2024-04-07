 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private LoadingGame loadingGame;
    [SerializeField] private InformationItem informationItem;
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
    }
    public void GetAppearance()  // Get id to player present
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceive, OnError);
    }
    void OnDataReceive(GetUserDataResult result) // get datagame user
    {
        Debug.Log("Sucessful Data Receive!");
        if (result.Data != null && result.Data.ContainsKey("DataGamePlayer"))
        {
            DataGame dataGame = JsonConvert.DeserializeObject<DataGame>(result.Data["DataGamePlayer"].Value);
            dataPlayer.setDataGame(dataGame);// Data ở playfab web null
            _name.text = dataPlayer.name;
            nameInGame = dataPlayer.name;
            //Debug.Log(dataGame.equipments[0]);
            setData();
        }
        else
            Debug.Log("Get Data No Done!");
    }
    public void setData()
    {
        //setPlayer();
        setItemBag();
        loadingGame.isDone = true;
        if (loadingGame.isDone && loadingGame.isDonePhoton)
            loadingGame.setLoadingGame(true);
        photonManager.SetActive(true);
    }    
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
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
    void setItemBag() // set các item trong túi đồ
    {
        for (int i=0; i<8; i++)
            if (dataPlayer.equipments[i] != -1)
            {
                dataPlayer.items[0, dataPlayer.equipments[i]] -= 1;
            }    
        for (int row = 0; row < dataPlayer.items.GetLength(0); row++)
        {
            for (int col = 0; col < dataPlayer.items.GetLength(1); col++)
            {
                int itemValue = dataPlayer.items[row, col];

                if (itemValue != 0)
                {
                    if (row==0)
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS1[col]);
                    else if(row == 1)
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS2[col]);
                    else if (row == 2)
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS3[col]);
                    else
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, ObjectManager.instance.imagesS4[col]);
                }
            }
        }
        for (int i = 0; i < 8; i++)
            if (dataPlayer.equipments[i] != -1)
            {
                dataPlayer.items[0, dataPlayer.equipments[i]] += 1;
            }
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
    public void LogOut() // out game
    {
        FindObjectOfType<SceneControl>().LoadScene("Login");
    }

}
