using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using System;
using PlayFab.ClientModels;

public class PlayerNameID : MonoBehaviour
{
    public string displayName;
    public string nickName;
    public string playfabID;
    private void OnMouseDown()
    {
        Player a = GetComponent<PhotonView>().Owner;
        Debug.Log(a);

        Debug.Log("ID player: " + playfabID);
        if (playfabID != LoadDataPlayer.instance.playfabID)
        {
            bool check = false;
            foreach (PlayFab.ClientModels.FriendInfo friend in PlayfabFriendList.instance.friendList)
            {
                if (string.Equals(displayName, friend.TitleDisplayName, StringComparison.OrdinalIgnoreCase))
                {
                    check = true;
                    break;
                }    
            }
            if (check)
            {
                ObjectManager.instance.add.SetActive(false);
                ObjectManager.instance.noAdd.SetActive(true);
            }
            else
            {
                ObjectManager.instance.add.SetActive(true);
                ObjectManager.instance.noAdd.SetActive(false);
            }
            ObjUse.instance.targetPhotonView = gameObject.GetComponent<PhotonView>();
            ObjectManager.instance.textUiConnectFriend.text = nickName;
            ObjectManager.instance.uiConnectFriend.SetActive(true);
        }
    }
}