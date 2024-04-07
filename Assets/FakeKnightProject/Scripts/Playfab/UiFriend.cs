using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;

public class UiFriend : MonoBehaviour
{
    [SerializeField] public TMP_Text nameFriend;
    [SerializeField] public string id;
    [SerializeField] public string username;
    [SerializeField] public Image status;

    public void getPPrivateChat()
    {
        PhotonChat.instance.setRecipient(nameFriend.text);
        ObjectManager.instance.chatFriend.SetActive(true);
        PhotonChat.instance.loadMessagePrivateChat();
    }

    private void Start()
    {
        // Đăng ký sự kiện để theo dõi trạng thái của người chơi
        if (PlayfabFriendList.instance != null)
        {
            PlayfabFriendList.instance.OnFriendStatusUpdate += UpdateFriendStatus;
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị hủy
        if (PlayfabFriendList.instance != null)
        {
            PlayfabFriendList.instance.OnFriendStatusUpdate -= UpdateFriendStatus;
        }
    }

    // Hàm được gọi khi trạng thái của bạn bè được cập nhật
    private void UpdateFriendStatus(string friendId, bool isOnline)
    {
        Debug.Log("cha gehvbhjsdbfjbsdjhbfjg");
        if (friendId == id)
        {
            if (isOnline)
            {
                // Bạn bè đang trực tuyến
                status.color = Color.red; // hoặc bất kỳ màu nào bạn muốn hiển thị
            }
            else
            {
                // Bạn bè không trực tuyến
                status.color = Color.green; // hoặc bất kỳ màu nào bạn muốn hiển thị
            }
        }
    }
}
