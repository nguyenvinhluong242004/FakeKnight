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
        ObjUse.instance.player.gameObject.GetComponent<PhotonView>().RPC("ReceiveMessage", ObjUse.instance.targetPhotonView.Owner, LoadDataPlayer.instance.nameInGame + '-' + LoadDataPlayer.instance.playfabID);
    }
    public void closeUiConnect()
    {
        ObjectManager.instance.uiConnectFriend.SetActive(false);
    }
    public void addInvite(string message)
    {
        string[] parts = message.Split('-');
        GameObject uiInvite = Instantiate(invitePrefab, invitePrefab.transform.position, invitePrefab.transform.rotation);
        uiInvite.transform.SetParent(content.transform);
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y + 107.5f);
        uiInvite.GetComponent<UiInvite>().namePlayer.text = parts[0];
        uiInvite.GetComponent<UiInvite>().idPlayfabSender = parts[1];
        uiInvite.GetComponent<UiInvite>().idPlayfabRecipient = LoadDataPlayer.instance.playfabID;
        uiInvite.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }    
    public void clearInvite(GameObject invite)
    {
        invite.SetActive(false);
        Destroy(invite);
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y - 107.5f);
    }
}
