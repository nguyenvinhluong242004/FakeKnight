using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Emote : MonoBehaviour
{
    public Sprite emote;
    public GameObject uiShop, image;
    public string Emo = "Emote";
    public void getEmote()
    {
        image = PhotonNetwork.Instantiate(this.Emo, ObjUse.instance.player.transform.position + new Vector3(0, 1.08f, 0), Quaternion.identity);
        image.GetComponent<SpriteRenderer>().sprite = emote;
        uiShop.SetActive(false);
    }     
}
