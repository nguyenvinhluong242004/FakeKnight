using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiInvite : MonoBehaviour
{
    [SerializeField] public TMP_Text namePlayer;
    [SerializeField] public TMP_Text message;
    public string idPlayfabSender;
    public string idPlayfabRecipient;

    public void deleteMail()
    {
        PlayfabFriendManager.instance.clearInvite(gameObject);
    } 
    public void acceptMail()
    {

    }
}
