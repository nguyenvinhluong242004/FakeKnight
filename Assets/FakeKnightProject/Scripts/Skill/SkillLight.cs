using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillLight : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public PlayerMove player;
    [SerializeField] private CircleCollider2D cir;
    [SerializeField] private SpriteRenderer spr;

    Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("setLight", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity;
    }
    public void setSkillLight(Vector2 velocity_)
    {
        velocity = velocity_;
        if (velocity.x < 0)
        {
            spr.flipX = true;
            cir.offset = new Vector2(-0.17f, cir.offset.y);
        }
        GetComponent<PhotonView>().RPC("setFlip", RpcTarget.All, spr.flipX);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("cham rồi");
        cir.enabled = false;
    }
    void setLight()
    {
        cir.enabled = true;
    }
    [PunRPC]
    void setFlip(bool sta)
    {
        spr.flipX = sta;
    }
}
