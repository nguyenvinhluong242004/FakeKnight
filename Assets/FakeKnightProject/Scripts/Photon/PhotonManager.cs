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
    [SerializeField] public string playerName, nameServer;
    [SerializeField] public int idServer;
    //public UiRoomProfile roomPrefab;
    public List<RoomInfo> updatedRooms;
    public List<NameServer> servers = new List<NameServer>();
    public List<PlayerNameID> playerIDs = new List<PlayerNameID>(); // do nothing


    public static PhotonManager instance;
    public string photonPlayer = "Player";
    public string photonSlime = "Slime";
    public string photonDevil = "Devil";
    public string photonOldMan = "OldMan";
    public string photonGuardsL = "GuardsL";
    public string photonGuardsR = "GuardsR";
    public List<PlayerProfile> players = new List<PlayerProfile>();
    private void Start()
    {
        Login();
        playerName = LoadDataPlayer.instance.dataPlayer.name;
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
    public virtual void Join()
    {
        Debug.Log(transform.name + ": Join room " + nameServer);
        //PhotonNetwork.LocalPlayer.NickName = LoadDataPlayer.instance.dataPlayer.name;
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
        NameServer nameServer_ = new NameServer
        {
            name = nameServer
        };
        this.servers.Add(nameServer_);
        //dataUseLoadGame.add(nameServer);
        idServer = dataUseLoadGame.getIdServer(nameServer);
        // load enemy and NPC
        if (idServer >= 0)
        {
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.enemySlimePositions)
            {
                GameObject p = PhotonNetwork.Instantiate(this.photonSlime, _po, Quaternion.identity);
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.enemyDevilPositions)
            {
                GameObject p = PhotonNetwork.Instantiate(this.photonDevil, _po, Quaternion.identity);
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.NPCPositions)
            {
                GameObject p = PhotonNetwork.Instantiate(this.photonOldMan, _po, Quaternion.identity);
                p.GetComponent<FigureMessage>().message = ObjUse.instance.messageOldMan;
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.NPCKillPositionsL)
            {
                GameObject p = PhotonNetwork.Instantiate(this.photonGuardsL, _po, Quaternion.identity);
            }
            foreach (Vector3 _po in dataUseLoadGame.ServersData[idServer].dataInServers.NPCKillPositionsR)
            {
                GameObject p = PhotonNetwork.Instantiate(this.photonGuardsR, _po, Quaternion.identity);
            }
            //dataUseLoadGame.ServersData[idServer].dataInServers.enemySlimePositions[0] = new Vector3(0, 0, 0);
        }    
        
        //Update data in server // update sau
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
        //int playerCount = PhotonNetwork.CurrentRoom.Players.Count;
        //Debug.Log(playerCount);
        //foreach (KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        //{
        //    int id = -1;
        //    string _name = "";
        //    string nickName = playerData.Value.NickName;
        //    string[] parts = nickName.Split('-'); // Tách chuỗi thành hai phần dựa trên dấu gạch ngang "-"
        //    if (parts.Length == 2)
        //    {
        //        _name = parts[0].Trim(); // Lấy phần đầu tiên và loại bỏ khoảng trắng xung quanh
        //        id = int.Parse(parts[1].Trim()); // Lấy phần thứ hai và chuyển đổi sang số nguyên
        //    }
        //    if (_name != LoadDataPlayer.instance.dataPlayer.name)
        //    {
        //        Debug.Log("trong vong for");
        //        // Spawn playerPrefab tương ứng
        //        GameObject playerr = PhotonNetwork.Instantiate(this.photonPlayer, Vector3.zero, Quaternion.identity);
        //        Debug.Log("id player " + id);
        //        playerr.GetComponent<PlayerMove>().setPlayer(id);
        //        playerr.GetComponent<PlayerMove>().textName.text = name;
        //    }    
            
        //    //playerr.transform.localScale = new Vector3(1, 1, 1); 
        //}
        // xử lí id: chua
        //GameObject playerObj;
        //if (LoadDataPlayer.instance.dataPlayer.idPlayer == 0)
        //    playerObj = Resources.Load(this.photonPlayer0) as GameObject;
        //else
        //    playerObj = Resources.Load(this.photonPlayer1) as GameObject;
        //GameObject player = Instantiate(playerObj, Vector3.zero, Quaternion.identity);
        //player.transform.localScale = new Vector3(1, 1, 1);
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
