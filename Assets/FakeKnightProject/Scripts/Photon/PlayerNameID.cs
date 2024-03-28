using UnityEngine;
using Photon.Pun;

public class PlayerNameID : MonoBehaviour
{
    public string nickName;
    public string playfabID;
    private void OnMouseDown()
    {
        Debug.Log("ID player: " + playfabID);
        if (playfabID != LoadDataPlayer.instance.playfabID)
        if (ObjectManager.instance)
        {
            ObjUse.instance.targetPhotonView = gameObject.GetComponent<PhotonView>();
            ObjectManager.instance.uiConnectFriend.SetActive(true);
        }
    }
}