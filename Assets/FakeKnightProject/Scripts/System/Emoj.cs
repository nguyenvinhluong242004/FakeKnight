using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Emoj : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    bool isOn = false;
    // Update is called once per frame
    void Update()
    {
        if (!isOn)
        {
            isOn = true;
            Invoke("resetEmote", 0.8f);
        }
        rb.velocity = new Vector2(0, 0.7f);
    }
    void resetEmote()
    {
        gameObject.SetActive(false);
        if (GetComponent<PhotonView>().IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
