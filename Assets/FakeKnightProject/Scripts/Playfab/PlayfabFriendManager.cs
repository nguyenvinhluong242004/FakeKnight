using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;

public class PlayfabFriendManager : MonoBehaviour
{
    [SerializeField] public static PlayfabFriendManager instance;
    [SerializeField] public GameObject invitePrefab;
    [SerializeField] public GameObject content;
    [SerializeField] public RectTransform rect;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        rect.sizeDelta = new Vector2(0, 450);
    }
    public void SendFriendRequest()
    {
        bool check = false;
        foreach (PlayFab.ClientModels.FriendInfo friend in PlayfabFriendList.instance.friendList)
        {
            Debug.Log(ObjUse.instance.targetPhotonView.GetComponent<PlayerNameID>().displayName);
            Debug.Log(friend.TitleDisplayName);
            if (string.Equals(ObjUse.instance.targetPhotonView.GetComponent<PlayerNameID>().displayName, friend.TitleDisplayName, StringComparison.OrdinalIgnoreCase))
            {
                check = true;
                break;
            }
        }
        if (check)
        {
            Debug.Log("da la ban be roi");
            ObjectManager.instance.add.SetActive(false);
            ObjectManager.instance.noAdd.SetActive(true);
        }
        else
        {
            // truyền vào sender.owner để khi thêm bạn, người gửi cũng thêm => cả 2 đồng thời thành bạn bè
            PhotonView sender = ObjUse.instance.player.gameObject.GetComponent<PhotonView>();
            Debug.Log(sender);
            Debug.Log(ObjUse.instance.targetPhotonView.Owner);
            sender.RPC("ReceiveMessage", ObjUse.instance.targetPhotonView.Owner, LoadDataPlayer.instance.nameInGame + '-' + LoadDataPlayer.instance.playfabID, sender.Owner);
        }
    }
    public void closeUiConnect()
    {
        ObjectManager.instance.uiConnectFriend.SetActive(false);
    }
    public void addInvite(string message, Player sender)
    {
        string[] parts = message.Split('-');
        GameObject uiInvite = Instantiate(invitePrefab, invitePrefab.transform.position, invitePrefab.transform.rotation);
        uiInvite.transform.SetParent(content.transform);
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y + 77.5f);
        uiInvite.GetComponent<UiInvite>().sender = sender;
        uiInvite.GetComponent<UiInvite>().namePlayer.text = parts[0];
        uiInvite.GetComponent<UiInvite>().idPlayfabSender = parts[1];
        uiInvite.GetComponent<UiInvite>().idPlayfabRecipient = LoadDataPlayer.instance.playfabID;
        uiInvite.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

    }    
    public void clearInvite(GameObject invite)
    {
        invite.SetActive(false);
        Destroy(invite);
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y - 77.5f);
    }
    public void addFriendPlayfab(string sender, string recipient, Player senderPlayfab)
    {
        AddFriendRequest request = new AddFriendRequest
        {
            FriendPlayFabId = sender // id của người bạn muốn thêm
        };

        // Gửi yêu cầu để thêm bạn từ id1
        PlayFabClientAPI.AddFriend(request, result =>
        {
            Debug.Log("Đã thêm bạn thành công cho " + sender);
            ObjUse.instance.player.gameObject.GetComponent<PhotonView>().RPC("AddFriend", senderPlayfab, recipient);
            // gọi hàm update
            PlayfabFriendList.instance.UpdateFriend();
        }, error =>
        {
            Debug.Log("Lỗi khi thêm bạn cho " + sender + ": " + error.ErrorMessage);
        });
    }
    public void addFriendPlayfabBySender(string recipient)
    {
        AddFriendRequest request = new AddFriendRequest
        {
            FriendPlayFabId = recipient // id của người bạn muốn thêm
        };

        // Gửi yêu cầu để thêm bạn từ id1
        PlayFabClientAPI.AddFriend(request, result =>
        {
            Debug.Log("Đã thêm bạn thành công cho " + recipient);
            PlayfabFriendList.instance.UpdateFriend();
        }, error =>
        {
            Debug.Log("Lỗi khi thêm bạn cho " + recipient + ": " + error.ErrorMessage);
        });
    }
}
