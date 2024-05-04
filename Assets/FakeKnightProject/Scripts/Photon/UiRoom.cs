using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiRoom : MonoBehaviour
{
    [SerializeField] public TMP_Text nameRoom;

    public void joinRoom()
    {
        PhotonManager.instance.JoinRoomByName(nameRoom.text);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
