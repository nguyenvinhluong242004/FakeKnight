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
    [SerializeField] public DataPlayer dataPlayer;
    [SerializeField] private string namePlayer;
    [SerializeField] private BagContent bagContent;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] public MoneyPlayer moneyPlayer;
    [SerializeField] private LoadingGame loadingGame;
    [SerializeField] private InformationItem informationItem;
    void Start()
    {
        GetAppearance();
        getUserNamePlayer();
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
            dataPlayer.setDataGame(dataGame);
            _name.text = dataPlayer.name;
            setPlayer();
            setItemBag();
            moneyPlayer.setValue();
            loadingGame.setLoadingGame(true);
            loadingGame.isDone = true;
        }
        else
            Debug.Log("Get Data No Done!");
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
    void setItemBag()
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
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, objectManager.imagesS1[col]);
                    else if(row == 1)
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, objectManager.imagesS2[col]);
                    else if (row == 2)
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, objectManager.imagesS3[col]);
                    else
                        bagContent.initBagItem(row, col, itemValue, informationItem.data[row, col].type, objectManager.imagesS4[col]);
                }
            }
        }
        for (int i = 0; i < 8; i++)
            if (dataPlayer.equipments[i] != -1)
            {
                dataPlayer.items[0, dataPlayer.equipments[i]] += 1;
            }
    } 
    public void setCountItem(int shop, int key, bool k)
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
    void setPlayer()
    {
        if(playerMove)
        {
            // set figure
            Debug.Log(dataPlayer.idPlayer);
            playerMove.anm.runtimeAnimatorController = objectManager.anmPlayers[dataPlayer.idPlayer];
            playerMove.number = dataPlayer.idPlayer;
            playerMove.sprite.sprite = objectManager.imgPlayers[dataPlayer.idPlayer];
            playerMove.setBoxCollider();
            objectManager.imgProfile.sprite = objectManager.imgPlayers[dataPlayer.idPlayer];
            // get Music, Sound Playerr
            objectManager.isSound = dataPlayer.isSound; 
            objectManager._sound.isOn = dataPlayer.isSound; 
            if (dataPlayer.isMusic)
                objectManager.audio.Play();
            objectManager._muic.isOn = dataPlayer.isMusic;
        }
        else
        {
            playerMove = FindObjectOfType<PlayerMove>();
            setPlayer();
        }
    } 
    public void LogOut()
    {
        FindObjectOfType<SceneControl>().LoadScene("Login");
    }

}
