using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private LoadDataPlayer loadDataPlayer;
    [SerializeField] private LoadingGame loadingGame;
    [SerializeField] public string playerName, nameServer;
    //public UiRoomProfile roomPrefab;
    public List<RoomInfo> updatedRooms;
    public List<NameServer> servers = new List<NameServer>();


    public static PhotonManager instance;
    public string photonPlayer0 = "Player0";
    public string photonPlayer1 = "Player1";
    public List<PlayerProfile> players = new List<PlayerProfile>();
    private void Start()
    {
        Login();
        playerName = loadDataPlayer.dataPlayer.name;
        nameServer = "SoSad";
    }
    //Logout / Login
    public void Logout()
    {
        Debug.Log(transform.name + ": Logout ");
        PhotonNetwork.Disconnect();
    }
    public void Login()
    {
        Debug.Log(transform.name + ": Login " + playerName);
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.LocalPlayer.NickName = loadDataPlayer.dataPlayer.name;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }
    // Create Room auto
    public virtual void Create()
    {
        Debug.Log(transform.name + ": Create server " + nameServer);
        PhotonNetwork.LocalPlayer.NickName = loadDataPlayer.dataPlayer.name;
        PhotonNetwork.CreateRoom(nameServer);
    }
    public virtual void Join()
    {
        Debug.Log(transform.name + ": Join room " + nameServer);
        PhotonNetwork.LocalPlayer.NickName = loadDataPlayer.dataPlayer.name;
        PhotonNetwork.JoinRoom(nameServer);
    }
    public virtual void Leave()
    {
        Debug.Log(transform.name + ": Leave Room");
        PhotonNetwork.LeaveRoom();
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        //loadingGame.isDonePhoton = true;
        //if (loadingGame.isDone && loadingGame.isDonePhoton)
        //    loadingGame.setLoadingGame(true);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom"); 
        LoadRoomPlayers();
        SpawnPlayer();
        loadingGame.isDonePhoton = true;
        if (loadingGame.isDone && loadingGame.isDonePhoton)
            loadingGame.setLoadingGame(true);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        this.updatedRooms = roomList;
        Debug.Log("room created + add");
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) this.RoomRemove(roomInfo);
            else this.RoomAdd(roomInfo);
        }
        Invoke("setServerGame", 0.2f);
    }
    void setServerGame()
    {
        if (servers.Count == 0)
        {
            Create();
            Debug.Log("kkk");
        }
        else
        {
            Debug.Log("kkkkkkk");
            Join();
            //SpawnPlayer();
            //loadingGame.isDonePhoton = true;
            //if (loadingGame.isDone && loadingGame.isDonePhoton)
            //    loadingGame.setLoadingGame(true);
            //this.UpdateRoomProfileUI();
        }
    }    

    protected virtual void RoomAdd(RoomInfo roomInfo)
    {
        Debug.Log("room add is on");
        NameServer nameServer;

        nameServer = this.RoomByName(roomInfo.Name);
        if (nameServer != null) return;

        nameServer = new NameServer
        {
            name = roomInfo.Name
        };
        this.servers.Add(nameServer);
    }

    //protected virtual void UpdateRoomProfileUI()
    //{
    //    foreach (Transform child in this.roomContent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (RoomProfile roomProfile in this.rooms)
    //    {
    //        UiRoomProfile uiRoomProfile = Instantiate(this.roomPrefab);
    //        uiRoomProfile.SetRoomProfile(roomProfile);
    //        uiRoomProfile.transform.SetParent(this.roomContent);
    //        uiRoomProfile.transform.localScale = new Vector3(1, 1, 1);
    //    }
    //}

    protected virtual void RoomRemove(RoomInfo roomInfo)
    {
        NameServer nameServer = this.RoomByName(roomInfo.Name);
        if (nameServer == null) return;
        this.servers.Remove(nameServer);
    }

    protected virtual NameServer RoomByName(string name)
    {
        foreach (NameServer nameServer in this.servers)
        {
            if (nameServer.name == name) return nameServer;
        }
        return null;
    }
    // load data

    protected virtual void SpawnPlayer()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            this.LoadPlayerPrefab();
            return;
        }
        // xử lí id: chua
        GameObject playerObj;
        if (loadDataPlayer.dataPlayer.idPlayer == 0)
            playerObj = Resources.Load(this.photonPlayer0) as GameObject;
        else
            playerObj = Resources.Load(this.photonPlayer1) as GameObject;
        GameObject player = Instantiate(playerObj, Vector3.zero, Quaternion.identity);
        player.transform.localScale = new Vector3(1, 1, 1);
    }


    protected virtual void LoadRoomPlayers()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined) return;

        PlayerProfile playerProfile;
        foreach (KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(playerData.Value.NickName);
            playerProfile = new PlayerProfile 
            {
                nickName = playerData.Value.NickName
            };
            this.players.Add(playerProfile);
        }
    }
    protected virtual void LoadPlayerPrefab()
    {
        GameObject player;
        if (loadDataPlayer.dataPlayer.idPlayer == 0)
            player = PhotonNetwork.Instantiate(this.photonPlayer0, Vector3.zero, Quaternion.identity);
        else
            player = PhotonNetwork.Instantiate(this.photonPlayer1, Vector3.zero, Quaternion.identity);

        //player.transform.localScale = new Vector3(1, 1, 1);
        //player.GetComponent<PlayerMove>().check = true;
        //GameObject _joy = GameObject.Find("joy");
        //GameObject _bgr = GameObject.Find("bgr");
        //player.GetComponent<PlayerMove>().sta = _joy.transform;
        //player.GetComponent<PlayerMove>().bgSta = _bgr.transform;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom: " + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom: " + otherPlayer.NickName);
    }
}
