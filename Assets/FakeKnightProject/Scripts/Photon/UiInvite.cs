using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class UiInvite : MonoBehaviour
{
    [SerializeField] public TMP_Text namePlayer;
    [SerializeField] public TMP_Text message;
    public string idPlayfabSender;
    public string idPlayfabRecipient;
    public Player sender;

    public void deleteMail()
    {
        PlayfabFriendManager.instance.clearInvite(gameObject);
    } 
    public void acceptMail()
    {
        PlayfabFriendManager.instance.addFriendPlayfab(idPlayfabSender, idPlayfabRecipient, sender);
        PlayfabFriendManager.instance.clearInvite(gameObject);
    }
}
