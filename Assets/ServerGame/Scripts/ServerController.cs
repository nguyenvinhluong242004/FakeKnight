using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ServerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _nameRoom;


    [SerializeField] private DataUseLoadGame dataUseLoadGame; // script table object
    [SerializeField] private LoadingGame loadingGame;
    [SerializeField] public string playerName, nameServer, nameRoomPlay;
    [SerializeField] public int idServer;

    [SerializeField] private RectTransform rect;
    [SerializeField] public GameObject contentRoom;
    [SerializeField] private GameObject roomPrefab;

    public List<RoomInfo> updatedRooms;
    public List<NameServer> servers = new List<NameServer>();


    public static ServerController instance;
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
        _nameRoom.text = "hihi";
        //CreateRoomOnGame();
    }
    public void Login()
    {
        Debug.Log(transform.name + ": Login " + playerName);
        //PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        //PhotonNetwork.LocalPlayer.NickName = $"{LoadDataPlayer.instance.namePlayer} - {LoadDataPlayer.instance.dataPlayer.name} - {LoadDataPlayer.instance.dataPlayer.idPlayer} - {LoadDataPlayer.instance.dataPlayer.PlayFabID}";
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
    public void CreateRoomOnGame()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (_nameRoom.text == "") return;
        nameServer = _nameRoom.text; // server trong game hiểu là room

        Debug.Log(transform.name + ": Create server " + nameServer);

        //RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = 10;  // Giới hạn số người chơi trong phòng
        //roomOptions.IsVisible = true;  // Phòng có thể được tìm thấy
        //roomOptions.IsOpen = true;     // Phòng có thể được tham gia

        //PhotonNetwork.CreateRoom(nameServer, roomOptions);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10; // Số lượng người chơi tối đa trong một phòng
        roomOptions.EmptyRoomTtl = 60000;
        PhotonNetwork.CreateRoom(nameServer, roomOptions, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        NameServer nameServer_ = new NameServer
        {
            name = nameRoomPlay
        };
        this.servers.Add(nameServer_);
        
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



        //loadingGame.isDonePhoton = true;

        //if (loadingGame.isDone && loadingGame.isDonePhoton)
        //{
        //    loadingGame.isDone = false;
        //    loadingGame.setLoadingGame(true);
        //}
        //Invoke("setServerGame", 0.2f); // xxuwr lí tham gia room ở đây
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
}
