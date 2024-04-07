using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonChat : MonoBehaviour
{
    [SerializeField] public static PhotonChat instance;
    public string senderPlayer;
    public string recipientPlayer;
    public TMP_InputField message;
    public TMP_InputField messagePrivate;
    public TMP_Text nameRecipient;
    public RectTransform chatContent;
    public RectTransform chatPrivateContent;
    public UiChat chatPrefabsIsmine, chatprefabsOther;
    public UiChat chatPrefabsPrivateIsmine, chatprefabsPrivateOther;
    public DataPlayer dataPlayer;

    [SerializeField] public Dictionary<string, List<string>> messagesBySender;

    private void Awake()
    {
        messagesBySender = new Dictionary<string, List<string>>();
        if (instance == null)
            instance = this;
    }
    public void AddMessage(string sender, string recipient, string message)
    {
        // Nếu sender chưa tồn tại trong Dictionary, tạo mới một List để lưu tin nhắn của sender
        if (!messagesBySender.ContainsKey(recipient))
        {
            messagesBySender[recipient] = new List<string>();
        }

        // Thêm tin nhắn vào List tương ứng với sender
        messagesBySender[recipient].Add(sender + ".|." + message);
    }

    // Hàm để lấy tin nhắn từ một người gửi cụ thể
    public List<string> GetMessagesFromSender(string sender)
    {
        // Kiểm tra xem sender có tồn tại trong Dictionary không
        if (messagesBySender.ContainsKey(sender))
        {
            // Trả về List tin nhắn của sender
            return messagesBySender[sender];
        }
        else
        {
            // Nếu sender không tồn tại, trả về List rỗng
            return new List<string>();
        }
    }
    public void sendMessagekk()
    {
        if (message.text.Length > 0)
        {
            Debug.Log("chat");
            SendChatMessage(message.text);
            message.text = "";
        }
    }
    public void setRecipient(string _name)
    {
        recipientPlayer = _name;
        nameRecipient.text = recipientPlayer;
    }
    public void loadMessagePrivateChat()
    {
        chatPrivateContent.sizeDelta = new Vector2(0, 0);
        foreach (Transform child in chatPrivateContent.transform)
        {
            Destroy(child.gameObject);
        }
        // Kiểm tra xem sender có tồn tại trong Dictionary không
        if (messagesBySender.ContainsKey(recipientPlayer))
        {
            // Trả về List tin nhắn của sender
            foreach(string mess in messagesBySender[recipientPlayer])
            {
                Debug.Log(mess);
                string[] parts = mess.Split(".|.");
                UiChat uiChatt;

                if (senderPlayer == parts[0])
                {
                    uiChatt = Instantiate(this.chatPrefabsPrivateIsmine);
                }
                else
                {
                    uiChatt = Instantiate(this.chatprefabsPrivateOther);
                }

                uiChatt.transform.SetParent(this.chatPrivateContent);
                uiChatt.transform.localScale = new Vector3(1, 1, 1);
                uiChatt.textChat.text = parts[1];
                chatPrivateContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 43f);
                
            }
        }
        else
        {
            // Nếu sender không tồn tại, trả về List rỗng
           
        }

    }
    public void SendChatPrivateFriend()
    {
        ObjUse.instance.player.gameObject.GetComponent<PhotonView>().RPC("ReceivePrivateMessage", RpcTarget.All, senderPlayer + ".|." + recipientPlayer + ".|." + messagePrivate.text);

        string formattedMessage = $"{recipientPlayer} -- {senderPlayer}: {messagePrivate.text}";
        UiChat uiChatt = Instantiate(this.chatPrefabsPrivateIsmine);

        uiChatt.transform.SetParent(this.chatPrivateContent);
        uiChatt.transform.localScale = new Vector3(1, 1, 1);
        uiChatt.textChat.text = messagePrivate.text;
        chatPrivateContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 43f);
        Debug.Log(formattedMessage);
        AddMessage(senderPlayer ,recipientPlayer, messagePrivate.text);



        messagePrivate.text = "";
    }
    public void setMessagePrivateReceive(string sender, string recipient, string message)
    {
        string formattedMessage = $"{sender} -- {recipient}: {messagePrivate.text}";
        if (nameRecipient.text == sender)
        {
            UiChat uiChatt = Instantiate(this.chatprefabsPrivateOther);

            uiChatt.transform.SetParent(this.chatPrivateContent);
            uiChatt.transform.localScale = new Vector3(1, 1, 1);
            uiChatt.textChat.text = message;
            chatPrivateContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 43f);
        }
        Debug.Log(formattedMessage);
        AddMessage(recipient, sender, message);
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
        if (parts.Length == 4)
        {
            senderName = parts[1].Trim();
            int id = int.Parse(parts[2].Trim());
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
        chatContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60f);
        Debug.Log(formattedMessage);
    }
}
