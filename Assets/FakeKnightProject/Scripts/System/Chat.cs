using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chat : MonoBehaviour
{
    public TMP_InputField messageChat;
    public TMP_Text messPL;
    public GameObject textChat;

    public void sendChat()
    {
        messPL.text = messageChat.text;
        messageChat.text = "";
        textChat.SetActive(true);
        Invoke("hideChat", 2.4f);
    }    
    void hideChat()
    {
        textChat.SetActive(false);
    }    
}
