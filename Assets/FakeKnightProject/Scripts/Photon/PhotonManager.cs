using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private DataUseLoadGame dataUseLoadGame; // script table object
    [SerializeField] private LoadingGame loadingGame;
    [SerializeField] public string playerName, nameServer, nameRoomPlay;
    [SerializeField] public int idServer;
    //public UiRoomProfile roomPrefab;
    [SerializeField] private RectTransform rect;
    [SerializeField] public GameObject contentRoom;
    [SerializeField] private GameObject roomPrefab;
    public List<RoomInfo> updatedRooms;
    public List<NameServer> servers = new List<NameServer>();
    public List<PlayerNameID> playerIDs = new List<PlayerNameID>(); // do nothing

    bool isOut; // check xem player muốn out phòng hay tài khoản

    public static PhotonManager instance;
    public string photonPlayer = "Player";
    public string photonSlime = "Slime";
    public string photonDevil = "Devil";
    public string photonOldMan = "OldMan";
    public string photonGuardsL = "GuardsL";
    public string photonGuardsR = "GuardsR";
    public List<PlayerProfile> players = new List<PlayerProfile>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        Login();
        playerName = LoadDataPlayer.instance.dataPlayer.name;
        nameServer = "SoSad";
        isOut = false;
    }
    //Logout / Login
    public void Logout()
    {
        Debug.Log(transform.name + ": Logout ");
        if (PhotonNetwork.InRoom)
        {
            Leave(true);
        }
        else
        {
            loadingGame._reset();
            PhotonNetwork.Disconnect();
            FindObjectOfType<SceneControl>().LoadScene("Login");
        }
    }
    public void Login()
    {
        Debug.Log(transform.name + ": Login " + playerName);
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.LocalPlayer.NickName = $"{LoadDataPlayer.instance.namePlayer} - {LoadDataPlayer.instance.dataPlayer.name} - {LoadDataPlayer.instance.dataPlayer.idPlayer} - {LoadDataPlayer.instance.dataPlayer.PlayFabID}";
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
        //PhotonNetwork.LocalPlayer.NickName = LoadDataPlayer.instance.dataPlayer.name;
        PhotonNetwork.CreateRoom(nameServer);
    }
    public void CreateRoomByName(string nameRoom)
    {
        nameRoomPlay = nameRoom;
        Debug.Log(transform.name + ": Create server " + nameRoom);
        PhotonNetwork.CreateRoom(nameRoom);
    }
    public void JoinRoomByName(string nameRoom)
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log(transform.name + ": Join room " + nameRoom);
            //PhotonNetwork.LocalPlayer.NickName = LoadDataPlayer.instance.dataPlayer.name;
            PhotonNetwork.JoinRoom(nameRoom);
        }
    }
    public virtual void Join()
    {
        Debug.Log(transform.name + ": Join room " + nameServer);
        //PhotonNetwork.LocalPlayer.NickName = LoadDataPlayer.instance.dataPlayer.name;
        PhotonNetwork.JoinRoom(nameServer);
    }
    public virtual void Leave(bool _isOut)
    {
        Debug.Log(transform.name + ": Leave Room");
        isOut = _isOut;
        if (PhotonNetwork.InRoom)
        {
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    ObjUse.instance.player.gameObject.GetComponent<PhotonView>().RPC("LeaveRoom", RpcTarget.All);
            //}
            //else

            if (PhotonNetwork.CurrentRoom.Players.Count > 1)
            {
                Debug.Log("set master");
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
            }
            PhotonNetwork.LeaveRoom();
        }
    }
    public void LeaveRoomAll()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        NameServer nameServer_ = new NameServer
        {
            name = nameRoomPlay
        };
        this.servers.Add(nameServer_);
        //dataUseLoadGame.add(nameRoomPlay);
        idServer = dataUseLoadGame.getIdServer(nameServer);
        // load enemy and NPC
        if (idServer >= 0)
        {
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.enemySlimePositions)
            {
                GameObject p = PhotonNetwork.InstantiateRoomObject(this.photonSlime, _po, Quaternion.identity, 0);
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.enemyDevilPositions)
            {
                GameObject p = PhotonNetwork.InstantiateRoomObject(this.photonDevil, _po, Quaternion.identity, 0);
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.NPCPositions)
            {
                GameObject p = PhotonNetwork.InstantiateRoomObject(this.photonOldMan, _po, Quaternion.identity, 0);
                p.GetComponent<FigureMessage>().message = ObjUse.instance.messageOldMan;
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.NPCKillPositionsL)
            {
                GameObject p = PhotonNetwork.InstantiateRoomObject(this.photonGuardsL, _po, Quaternion.identity, 0);
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.NPCKillPositionsR)
            {
                GameObject p = PhotonNetwork.InstantiateRoomObject(this.photonGuardsR, _po, Quaternion.identity, 0);
            }
            //dataUseLoadGame.ServersData[idServer].dataInServers.enemySlimePositions[0] = new Vector3(0, 0, 0);
        }
        //ObjectManager.instance.lobby.SetActive(false);
        //ObjectManager.instance.scenePlay.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        LoadRoomPlayers();
        Debug.Log("so player hien tai: " + PhotonNetwork.CurrentRoom.Players.Count);
        

        SpawnPlayer();
        loadingGame.isDonePhoton = true;
        if (loadingGame.isDone && loadingGame.isDonePhoton)
        {
            loadingGame.isDone = false;
            loadingGame.setLoadingGame(true);
        }
        ObjectManager.instance.lobby.SetActive(false);
        ObjectManager.instance.scenePlay.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        if (isOut)
        {
            loadingGame._reset();
            PhotonNetwork.Disconnect();
            FindObjectOfType<SceneControl>().LoadScene("Login");
        }
        else
        {
            loadingGame._reset();
            PhotonNetwork.Disconnect();
            FindObjectOfType<SceneControl>().LoadScene("Play");
        }
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
        for (int i = 0; i < contentRoom.transform.childCount; i++)
        {
            // Lấy tham chiếu đến đối tượng con thứ i
            GameObject childObject = contentRoom.transform.GetChild(i).gameObject;

            // Hủy (destroy) đối tượng con
            Destroy(childObject);
        }
        rect.sizeDelta = new Vector2(0, 435);

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) this.RoomRemove(roomInfo);
            else
            {
                Debug.Log("on update room");
                this.RoomAdd(roomInfo);
                GameObject uiRoom = Instantiate(roomPrefab, roomPrefab.transform.position, roomPrefab.transform.rotation);
                uiRoom.transform.SetParent(contentRoom.transform);
                rect.sizeDelta = new Vector2(0, rect.sizeDelta.y + 40f);

                uiRoom.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                uiRoom.GetComponent<UiRoom>().nameRoom.text = roomInfo.Name;
            }
        }
        


        loadingGame.isDonePhoton = true;

        //loadingGame.textNote.text = "loadgame.isdonePhoton";

        if (loadingGame.isDone && loadingGame.isDonePhoton)
        {
            loadingGame.isDone = false;
            loadingGame.setLoadingGame(true);
        }

        //Invoke("setServerGame", 0.2f); // xxuwr lí tham gia room ở đây
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
            Debug.Log("kkkkkkk" + servers[0]);
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
    }


    protected virtual void LoadRoomPlayers()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined) return;

        PlayerProfile playerProfile;
        foreach (KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        {
            string nickName = playerData.Value.NickName;
            Debug.Log(nickName);
            string[] parts = nickName.Split('-'); 
            if (parts.Length == 4)
            {
                string displayname = parts[0].Trim();
                string name = parts[1].Trim(); 
                int id = int.Parse(parts[2].Trim());
                string playfabID = parts[3].Trim();

                // Tạo một đối tượng PlayerProfile mới với name
                playerProfile = new PlayerProfile
                {
                    nickName = name
                };
                Debug.Log("Displayname: " + displayname + "Name: " + name + "Id: " + id + "PlayfabID: " + playfabID);
                this.players.Add(playerProfile);
            }
        }
    }
    protected virtual void LoadPlayerPrefab()
    {
        GameObject player = PhotonNetwork.Instantiate(this.photonPlayer, Vector3.zero, Quaternion.identity);
        Debug.Log("master " + LoadDataPlayer.instance.dataPlayer.idPlayer);
        player.GetComponent<PlayerMove>().setPlayer(LoadDataPlayer.instance.dataPlayer.idPlayer);
        ObjectManager.instance.imgProfile.sprite = ObjectManager.instance.imgPlayers[LoadDataPlayer.instance.dataPlayer.idPlayer];
        player.GetComponent<PlayerMove>().textName.text = LoadDataPlayer.instance.dataPlayer.name;
        player.GetComponent<PlayerMove>().playerNameID.displayName = LoadDataPlayer.instance.namePlayer;
        player.GetComponent<PlayerMove>().playerNameID.nickName = LoadDataPlayer.instance.dataPlayer.name;
        player.GetComponent<PlayerMove>().playerNameID.playfabID = LoadDataPlayer.instance.dataPlayer.PlayFabID;
        // xử lí lấy trang bị
        // từ playerimpact của player: bắt đầu tính toán các giá trị % trị số...
        player.GetComponent<PlayerImpact>().setPercent();

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom: " + newPlayer.NickName);
        //EnemyLevel1[] enes = FindObjectsOfType<EnemyLevel1>();
        //GetComponent<PhotonView>().GetComponent<PhotonView>().RPC("InitializeEnemy", RpcTarget.OthersBuffered, enes);
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    //EnemyLevel1[] enes = FindObjectsOfType<EnemyLevel1>();
        //    //GetComponent<PhotonView>().RPC("InitializeEnemy", RpcTarget.OthersBuffered, enes);
        //}
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom: " + otherPlayer.NickName);
    }
    [PunRPC]
    public void InitializeEnemy(EnemyLevel1[] enes)
    {
        Debug.Log("koooooooooooooooooooo");
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            foreach (EnemyLevel1 e in enes)
            {
                Debug.Log(e.transform.position);
            }
        }    
    }

    
}
