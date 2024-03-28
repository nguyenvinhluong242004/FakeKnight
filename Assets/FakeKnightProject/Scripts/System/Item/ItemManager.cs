using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject notification;
    [SerializeField] private TMP_Text textNotification;

    public string photonHealing = "Healing";
    GameObject healing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool UseItem(int shop, int key)
    {
        if (1 == shop && 0 == key)
        {
            if (healing)
            {
                notification.SetActive(true);
                return false;
            }
            else
            {
                notification.SetActive(false);
                healing = PhotonNetwork.Instantiate(this.photonHealing, ObjUse.instance.player.transform.position, Quaternion.identity);
                ObjUse.instance.playerImpact.setIsHealing();
                Invoke("DestroyItem", 1f);
                return true;
            }
        }
        return false;
    }    
    void DestroyItem()
    {
        healing.SetActive(false);
        PhotonNetwork.Destroy(healing);
        notification.SetActive(false);
    }    
}
