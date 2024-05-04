using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Chat.Demo;

public class ControlGameOnLobby : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameRoom;
    [SerializeField] private RectTransform rect;
    [SerializeField] public GameObject contentFriend;
    [SerializeField] private GameObject friendPrefab;
    [SerializeField] private GameObject friendLisst;
    [SerializeField] private Image friendLisstButton;
    [SerializeField] private GameObject roomLisst;
    [SerializeField] private Image roomLisstButton;
    public List<FriendInfo> friendList;

    // Start is called before the first frame update
    void Start()
    {
        getListFriend();
        nameRoom.text = "So Sad";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void createRoom()
    {
        // tạo và vào luôn phòng, người khaccs muốn vào thì join?
        PhotonManager.instance.CreateRoomByName(nameRoom.text);
        //  xử lí chỗ render ra quaiis , npc, nhan vật
        // mục đích : chỉ tạo room, không tạo nhân vật ( tạo quái ... )
    }
    public void getListFriend()
    {
        roomLisst.SetActive(false);
        roomLisstButton.color = new Color(0.5f, 0.5f, 0.5f, 1);
        friendLisst.SetActive(true);
        friendLisstButton.color = new Color(1, 1, 1, 1);
        for (int i = 0; i < contentFriend.transform.childCount; i++)
        {
            // Lấy tham chiếu đến đối tượng con thứ i
            GameObject childObject = contentFriend.transform.GetChild(i).gameObject;

            // Hủy (destroy) đối tượng con
            Destroy(childObject);
        }
        rect.sizeDelta = new Vector2(0, 435);
        var request = new GetFriendsListRequest
        {
            //IncludeSteamFriends = false, // Nếu bạn đang sử dụng Steam, đặt thành true để bao gồm bạn bè từ Steam
            //IncludeFacebookFriends = false // Tương tự, nếu bạn đang sử dụng Facebook, đặt thành true để bao gồm bạn bè từ Facebook
        };

        PlayFabClientAPI.GetFriendsList(request, result =>
        {
            friendList = result.Friends;

            // Xử lý danh sách bạn bè ở đây
            foreach (FriendInfo friend in friendList)
            {
                Debug.Log("Friend ID: " + friend.FriendPlayFabId);
                Debug.Log("Friend Username: " + friend.TitleDisplayName);
                // Các thông tin khác có thể được truy cập qua thuộc tính khác của friend
                GameObject uiFriend = Instantiate(friendPrefab, friendPrefab.transform.position, friendPrefab.transform.rotation);
                uiFriend.transform.SetParent(contentFriend.transform);
                rect.sizeDelta = new Vector2(0, rect.sizeDelta.y + 40f);
                uiFriend.GetComponent<UiFriend>().id = friend.FriendPlayFabId;
                uiFriend.GetComponent<UiFriend>().username = friend.TitleDisplayName;
                uiFriend.GetComponent<UiFriend>().nameFriend.text = friend.TitleDisplayName;
                uiFriend.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }, error =>
        {
            Debug.LogError("Get Friend List Failed: " + error.GenerateErrorReport());
        });
    }
    public void getListRoom()
    {
        friendLisst.SetActive(false);
        friendLisstButton.color = new Color(0.5f, 0.5f, 0.5f, 1);
        roomLisst.SetActive(true);
        roomLisstButton.color = new Color(1, 1, 1, 1);
    }
    public void leaveRoom()
    {
        PhotonManager.instance.Leave(false);
    }
}
