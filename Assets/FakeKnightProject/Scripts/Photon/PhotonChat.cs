using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonChat : MonoBehaviour
{
    public TMP_InputField message;
    public Transform chatContent;
    public UiChat chatPrefabsIsmine, chatprefabsOther;
    public DataPlayer dataPlayer;
    public void sendMessagekk()
    {
        if (message.text.Length > 0)
        {
            Debug.Log("chat");
            SendChatMessage(message.text);
            message.text = "";
        }
    }
    public void SendChatMessage(string message)
    {
        // Gửi tin nhắn đến tất cả các người dùng khác trong phòng
        GetComponent<PhotonView>().RPC("ReceiveMessage", RpcTarget.All, message);
    }

    // Nhận tin nhắn
    [PunRPC]
    private void ReceiveMessage(string message, PhotonMessageInfo info)
    {
        // Hiển thị tin nhắn trong giao diện người dùng
        DisplayMessage(message, info.Sender);
    }

    // Hiển thị tin nhắn
    private void DisplayMessage(string message, Player sender)
    {
        string nickName = sender.NickName;
        string[] parts = nickName.Split('-');
        string senderName = "";
        if (parts.Length == 3)
        {
            senderName = parts[0].Trim();
            int id = int.Parse(parts[1].Trim());
        }
        string formattedMessage = $"{senderName}: {message}";
        UiChat uiChat;
        if (senderName == dataPlayer.name)
            uiChat = Instantiate(this.chatPrefabsIsmine);
        else
            uiChat = Instantiate(this.chatprefabsOther);
        uiChat.transform.SetParent(this.chatContent);
        uiChat.transform.localScale = new Vector3(1, 1, 1);
        uiChat.namePlayer.text = senderName;
        uiChat.textChat.text = message;
        chatContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 55f);
        Debug.Log(formattedMessage);
    }
}
