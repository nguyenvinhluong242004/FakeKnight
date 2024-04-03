using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Chat.Demo;

public class PlayfabFriendList : MonoBehaviour, IChatClientListener
{
    [SerializeField] static public PlayfabFriendList instance;
    [SerializeField] private RectTransform rect;
    [SerializeField] public GameObject content;
    [SerializeField] private GameObject friendPrefab;
    public List<FriendInfo> friendList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        rect.sizeDelta = new Vector2(0, 380);
    }
    public void UpdateFriend()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            // Lấy tham chiếu đến đối tượng con thứ i
            GameObject childObject = content.transform.GetChild(i).gameObject;

            // Hủy (destroy) đối tượng con
            Destroy(childObject);
        }
        rect.sizeDelta = new Vector2(0, 380);
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
                uiFriend.transform.SetParent(content.transform);
                rect.sizeDelta = new Vector2(0, rect.sizeDelta.y + 49.5f);
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


    public delegate void FriendStatusUpdate(string friendId, bool isOnline);
    public event FriendStatusUpdate OnFriendStatusUpdate;
    private ChatClient chatClient;

    void Start()
    {
        chatClient = new ChatClient(this);
        ConnectoToPhotonChat();
    }
    public void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }
    }
    private void OnDestroy()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }
    private void ConnectoToPhotonChat()
    {
        Debug.Log("Connecting to Photon Chat");
        chatClient.AuthValues = new AuthenticationValues(LoadDataPlayer.instance.namePlayer);
        ChatAppSettings chatSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        chatClient.ConnectUsingSettings(chatSettings);
    }
    public void OnConnected()
    {
        Debug.Log("Connected to Photon Chat!");
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconnected from Photon Chat!");
    }

    public void OnChatStateChange(ChatState state)
    {
        // Handle chat state changes if needed
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        // Handle receiving messages if needed
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // Handle receiving private messages if needed
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        if (status == ChatUserStatus.Online)
        {
            OnFriendStatusUpdate?.Invoke(user, true);
        }
        else
        {
            OnFriendStatusUpdate?.Invoke(user, false);
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        //
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //
    }

    public void OnUnsubscribed(string[] channels)
    {
        //
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
       //
    }
}
