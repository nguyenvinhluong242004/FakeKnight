using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillThunderbird : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public PlayerMove player;
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Animator anm;

    Vector2 velocity;

    bool isEnd = false;
    bool isStart = false;
    bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("end", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            isStart = true;
            Invoke("end", 2.8f);
        }
        rb.velocity = 2 * velocity;
    }
    public void setSkillThunderbird(Vector2 velocity_)
    {
        velocity = velocity_;
        if (velocity.x < 0)
        {
            spr.flipX = true;
            box.offset = new Vector2(0.27f, box.offset.y);
        }
        GetComponent<PhotonView>().RPC("setFlip", RpcTarget.All, spr.flipX);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHit && collision.gameObject.CompareTag("Enemy"))
        {
            box.enabled = false;
            isHit = true;
            Invoke("end", 0.3f);
        }
        Debug.Log("cham rồi");
    }
    void end()
    {
        if (gameObject)
        {
            velocity = new Vector2(0, 0);
            rb.velocity = velocity;
            anm.Play("attack");
        }
    }
    [PunRPC]
    void setFlip(bool sta)
    {
        spr.flipX = sta;
    }
}
