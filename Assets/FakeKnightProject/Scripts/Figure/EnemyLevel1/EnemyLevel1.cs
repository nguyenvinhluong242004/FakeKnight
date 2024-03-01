using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyLevel1 : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] public float blood;
    [SerializeField] private bool isEfect, isDie, isRandom, isSkill, isChangeConstrains, isChoose, isSK4;
    [SerializeField] private Blood blood_;
    [SerializeField] private GameObject bl;
    [SerializeField] private GameObject cut;
    public string Cut = "Cut";
    SpriteRenderer spr;
    Rigidbody2D rb;
    public PlayerMove player;
    Animator anm;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        anm = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        isEfect = false;
        isDie = false;
        isRandom = false;
        isSkill = false;
        isChangeConstrains = false;
        isChoose = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSK4)
        {
            if (!isDie && isEfect)
            {
                if (player)
                {
                    if (player.transform.position.y > transform.position.y)
                        spr.sortingOrder = 2;
                    else
                        spr.sortingOrder = 0;
                    if (!isChangeConstrains)
                    {
                        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                        isChangeConstrains = true;
                    }
                    if (spr.flipX && player.transform.position.x > transform.position.x)
                        photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, false);
                    else if (!spr.flipX && player.transform.position.x < transform.position.x)
                        photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, true);
                    if (!bl.activeSelf)
                        bl.SetActive(true);
                    if (!isSkill)
                    {
                        if (spr.flipX)
                            cut = PhotonNetwork.Instantiate(this.Cut, transform.position - new Vector3(0.4f, 0, 0), Quaternion.identity);
                        else
                            cut = PhotonNetwork.Instantiate(this.Cut, transform.position + new Vector3(0.4f, 0, 0), Quaternion.Euler(0, 0, 180f));
                        isSkill = true;
                        Invoke("resetSkill", 1f);
                    }
                    if (player.isSkill)
                    {
                        if (player.oneSkill)
                        {
                            if (player.isU)
                            {
                                if (transform.position.y - player.transform.position.y > -0.4f && transform.position.y - player.transform.position.y < 0.8f)
                                {
                                    if (transform.position.x < player.transform.position.x + 0.9f && transform.position.x > player.transform.position.x - 0.9f)
                                    {
                                        Debug.Log("hitU");
                                        player.oneSkill = false;
                                        blood -= 3f;
                                        blood_.setBlood(blood);
                                        isChoose = true;
                                    }
                                }
                            }
                            else if (player.isD)
                            {
                                if (player.transform.position.y - transform.position.y > 0.5f && player.transform.position.y - transform.position.y < 1.24f)
                                {
                                    if (transform.position.x < player.transform.position.x + 0.9f && transform.position.x > player.transform.position.x - 0.9f)
                                    {
                                        Debug.Log("hitD");
                                        player.oneSkill = false;
                                        blood -= 3f;
                                        blood_.setBlood(blood);
                                        isChoose = true;
                                    }
                                }
                            }
                            else if (player.sprite.flipX)
                            {
                                if (transform.position.x - player.transform.position.x < 0.8f)
                                {
                                    Debug.Log("hitL");
                                    player.oneSkill = false;
                                    blood -= 3f;
                                    blood_.setBlood(blood);
                                    isChoose = true;
                                }
                            }
                            else
                            {
                                if (player.transform.position.x - transform.position.x < 0.8f)
                                {
                                    Debug.Log("hitR");
                                    player.oneSkill = false;
                                    blood -= 3f;
                                    blood_.setBlood(blood);
                                    isChoose = true;

                                }
                            }
                        }
                        //else if (player.number == 1 && player.oneSkill) // xử lí bên script player đối với bắn cung
                        else if (player.twoSkill)
                        {
                            // Xử lí skill dịch chuyển gây sát thương
                            Vector2 v = player.worldPosition - transform.position;
                            if (v.magnitude < 2f)
                            {
                                if (!bl.activeSelf)
                                    bl.SetActive(true);
                                Debug.Log("trung chieu 2");
                                isChoose = true;
                                player.twoSkill = false;
                                blood -= 12f;
                                blood_.setBlood(blood);
                            }

                        }
                    }
                }
                else
                {
                    player = FindObjectOfType<PlayerMove>();
                }
            }
            else if (!isDie)
            {
                if (isChangeConstrains)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    isChangeConstrains = false;
                }
                if (isChoose)
                {
                    if (!bl.activeSelf)
                        bl.SetActive(true);
                    Vector2 currentPos = transform.position;
                    Vector2 targetPos = player.transform.position;

                    // Tính toán vectơ hướng từ vị trí hiện tại đến vị trí của player
                    Vector2 direction = targetPos - currentPos;
                    if (direction.magnitude > 7f)
                    {
                        photonView.RPC("PlayAnimation", RpcTarget.All, "idle");
                        isChoose = false;
                        bl.SetActive(false);
                    }
                    else
                    {
                        // Giới hạn độ dài của vectơ hướng thành 1
                        direction = Vector2.ClampMagnitude(direction, 1f);
                        if (direction.x < 0)
                            photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, true);
                        else
                            photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, false);
                        // Di chuyển quái vật theo vectơ hướng với tốc độ đã đặt
                        transform.position = Vector2.MoveTowards(currentPos, currentPos + direction, 1f * Time.deltaTime);
                        photonView.RPC("PlayAnimation", RpcTarget.All, "run");
                    }
                }
                else if (!isRandom)
                {
                    rb.velocity = Vector2.ClampMagnitude(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 1f);
                    if (rb.velocity.x < 0)
                        photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, true);
                    else
                        photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, false);
                    photonView.RPC("PlayAnimation", RpcTarget.All, "run");
                    isRandom = true;
                    Invoke("resetMove", 3f);
                }
            }
        }    
        
        if (!isDie && blood <= 0f)
        {
            isDie = true;
            photonView.RPC("PlayAnimation", RpcTarget.All, "dealth");
            Invoke("destroy", 0.7f);
        }
        // xử lí hỏa bạo
        if (!isDie && player && player.threeSkill)
        {
            Vector2 v = player.worldPosition - transform.position;
            if (v.magnitude < 3.6f)
            {
                if (!bl.activeSelf)
                    bl.SetActive(true);
                Debug.Log("trung chieu 3");
                isChoose = true;
                player.threeSkill = false;
                blood -= 20f;
                blood_.setBlood(blood);
            }

        }
        // xử lí skill 4
        if (!isDie && player && player.fourSkill)
        {
            Vector2 v = player.worldPosition - transform.position;
            if (v.magnitude < 3.6f)
            {
                isSK4 = true;
                rb.velocity = new Vector2(0, 0);
                photonView.RPC("PlayAnimation", RpcTarget.All, "idle");
            }

        }
        else if (isSK4)
        {
            isSK4 = false;
        }    
    }
    void resetSkill()
    {
        if (GetComponent<PhotonView>().IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(cut);
        }
        player.playerImpact.setBlood(1f);
        Invoke("resteTimeSkill", 2.4f);
    }    
    void resteTimeSkill()
    {
        isSkill = false;
    }    
    void resetMove()
    {
        rb.velocity = new Vector2(0, 0);
        photonView.RPC("PlayAnimation", RpcTarget.All, "idle");
        Invoke("resetRandom", 1f);
    }    
    void resetRandom()
    {
        isRandom = false;
    }    
    void destroy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isEfect = true;
        }    
        if(collision.gameObject.CompareTag("arrow"))
        {
            blood -= 3f;
            blood_.setBlood(blood);
            isChoose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isEfect = false;
        Invoke("CloseBlood", 1.2f);
    }
    void CloseBlood()
    {
        bl.SetActive(false);
    }    
    public void setBloodEff()
    {
        if (!bl.activeSelf)
            bl.SetActive(true);
        blood -= 10f;
        blood_.setBlood(blood);
    }
    [PunRPC]
    void PlayAnimation(string animationName)
    {
        anm.Play(animationName);
    }
    [PunRPC]
    void SyncFlipX(bool flipState)
    {
        spr.flipX = flipState;
    }
}
