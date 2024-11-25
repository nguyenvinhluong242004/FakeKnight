using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyLevel1 : MonoBehaviour
{
    [SerializeField] public int id;
    [SerializeField] private int gold;
    [SerializeField] private PhotonView photonView;
    [SerializeField] public float blood;
    [SerializeField] public float bloodDefault;
    [SerializeField] private bool isEfect;
    [SerializeField] private bool isEfectSkillBem;
    [SerializeField] private bool isDie;
    [SerializeField] private bool isRandom;
    [SerializeField] private bool isSkill;
    [SerializeField] private bool isChangeConstrains; // thay đổi thuộc tính constrain x,y,z
    [SerializeField] private bool isChoose;
    [SerializeField] private bool isSK4;
    [SerializeField] private Blood blood_;
    [SerializeField] private GameObject bl;
    [SerializeField] private GameObject cut;
    public string Cut = "Cut";
    public string ValueDamage = "ValueDamage";
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Rigidbody2D rb;
    public PlayerMove player;
    [SerializeField] private Animator anm;
    bool _tagThunderbird = false;
    bool _tagLight = false;
    // Start is called before the first frame update
    void Start()
    {
        bloodDefault = blood;
        isEfect = false;
        isEfectSkillBem = false;
        isDie = false;
        isRandom = false;
        isSkill = false;
        isChangeConstrains = false;
        isChoose = false;
        _tagThunderbird = false;
        _tagLight = false;
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
                        photonView.RPC("SyncConstrain", RpcTarget.AllBuffered, true);
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
                        player.hitEnemyAttack();
                        player.playerImpact.changeBlood(7f, false);
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
                                        hitPlayerAttack();
                                        player.oneSkill = false;
                                        isEfectSkillBem = true;
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
                                        hitPlayerAttack();
                                        player.oneSkill = false; isEfectSkillBem = true;
                                        isChoose = true;
                                    }
                                }
                            }
                            else if (player.sprite.flipX)
                            {
                                if (transform.position.x - player.transform.position.x < 0.8f)
                                {
                                    Debug.Log("hitL");
                                    hitPlayerAttack();
                                    player.oneSkill = false;
                                    isEfectSkillBem = true;
                                    isChoose = true;
                                }
                            }
                            else
                            {
                                if (player.transform.position.x - transform.position.x < 0.8f)
                                {
                                    Debug.Log("hitR");
                                    hitPlayerAttack();
                                    player.oneSkill = false;
                                    isEfectSkillBem = true;
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
                                hitPlayerAttack();
                                player.twoSkill = false;
                                photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, 12f);
                            }

                        }
                    }
                }
            }
            else if (!isDie)
            {
                if (isChangeConstrains)
                {
                    photonView.RPC("SyncConstrain", RpcTarget.AllBuffered, false);
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
                        direction = Vector2.ClampMagnitude(direction, 1.4f);
                        if (direction.x < 0)
                            photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, true);
                        else
                            photonView.RPC("SyncFlipX", RpcTarget.AllBuffered, false);
                        // Di chuyển quái vật theo vectơ hướng với tốc độ đã đặt
                        transform.position = Vector2.MoveTowards(currentPos, currentPos + direction, 1.8f * Time.deltaTime);
                        photonView.RPC("PlayAnimation", RpcTarget.All, "run");
                    }
                }
                else if (!isRandom)
                {
                    rb.velocity = Vector2.ClampMagnitude(new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f)), 1.4f);
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
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("PlayAnimation", RpcTarget.All, "dealth");
                Invoke("destroy", 0.7f);
            }
        }
        // xử lí hỏa bạo
        if (!isDie && player && player.threeSkill)
        {
            Vector2 v = player.worldPosition - transform.position;
            if (v.magnitude < 3.6f)
            {
                if (!bl.activeSelf)
                    bl.SetActive(true);
                //hitPlayerAttack();
                Debug.Log("trung chieu 3");
                isChoose = true;
                player.threeSkill = false;
                photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, player.playerImpact.getDamageSkill4());
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
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(cut);
        }
        // xử lí vấn đề all player bị mất máu
        //if (GetComponent<PhotonView>().IsMine)
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
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);

        photonView.RPC("reStart", RpcTarget.All);
        EnemyManager.instance.addPooled(gameObject);
        //PhotonNetwork.Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision) // bug chỗ player của attach trong các xử lí
    {                                                   // chỉ 1 phía tác động được gọi -> nếu all đều gọi thì player 
        if (collision.gameObject.CompareTag("Player"))  // khác nhau => lượng máu khác nhau => sai 
        {                                               // Status: Chưa fix
            isEfect = true;
            player = collision.gameObject.GetComponent<PlayerMove>();
        }

        //test
        if (collision.gameObject.CompareTag("Bem"))
        {
            Debug.Log("effect bemmmmmmmmmmmmmmmmmmmmmm");
        }
        if (isEfectSkillBem && collision.gameObject.CompareTag("Bem"))
        {
            Bem bem = collision.gameObject.GetComponent<Bem>();
            isEfectSkillBem = false;
            bem.offBoxEffect();
            Debug.Log("bemmmmmmmmmmmmmmmmmmmmmm");
            if (bem != null)
            {
                attackBasis(bem.player);
            }
            else
            {
                Debug.LogWarning("Bem component not found on collided object.");
            }
        }
        if (collision.gameObject.CompareTag("arrow"))
        {
            photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, 3f);

            isChoose = true;
        }
        if (!_tagLight && collision.gameObject.CompareTag("Light"))
        {
            SkillLight skillLight = collision.gameObject.GetComponent<SkillLight>();
            _tagLight = true;

            if (skillLight != null)
            {
                StartCoroutine(InvokeWithDelayLight(0.4f, skillLight.player));
            }
            else
            {
                Debug.LogWarning("Light component not found on collided object.");
            }
        }
        if (!_tagThunderbird && collision.gameObject.CompareTag("Thunderbird"))
        {
            SkillThunderbird skillThunderbird = collision.gameObject.GetComponent<SkillThunderbird>();
            _tagThunderbird = true;

            if (skillThunderbird != null)
            {
                StartCoroutine(InvokeWithDelayThunderBird(0.4f, skillThunderbird.player));
            }
            else
            {
                Debug.LogWarning("SkillThunderbird component not found on collided object.");
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            isEfect = false;
            Invoke("CloseBlood", 1.2f);
        }
    }
    void GenerateValueDamage(float value)
    {
        GameObject _value = PhotonNetwork.Instantiate(this.ValueDamage, new Vector3(Random.Range(transform.position.x - 0.4f, transform.position.x + 0.4f), Random.Range(transform.position.y - 0.1f, transform.position.y + 0.2f), 100f), Quaternion.identity);
        _value.GetComponent<ValueDamage>().value.color = new Color(1f, 0f, 0f, 1f);
        _value.GetComponent<ValueDamage>().value.text = "- " + ((int)value).ToString();
    }
    void attackBasis(PlayerMove player)
    {

        float value = player.playerImpact.getDamageSkill1() * (100 + player.playerImpact.getPercentDamage()) / 100;
        if (blood - value <= 0f)
        {
            player.getTotalByEnemy(gold);
        }
        photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, value);
        GenerateValueDamage(value);

        //_tagLight = false;
        //float value = 5f * (100 + player.playerImpact.getPercentDamage()) / 100;
        //photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, value);

        //bl.SetActive(true);
        //Invoke("CloseBlood", 0.8f);
    }
    IEnumerator InvokeWithDelayLight(float delay, PlayerMove player)
    {
        yield return new WaitForSeconds(delay);
        attackLight(player);
    }
    void attackLight(PlayerMove player)
    {
        _tagLight = false;
        float value = player.playerImpact.getDamageSkill2() * (100 + player.playerImpact.getPercentDamage()) / 100;
        if (blood - value <= 0f)
        {
            player.getTotalByEnemy(gold);
        }
        photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, value);
        GenerateValueDamage(value);

        bl.SetActive(true);
        Invoke("CloseBlood", 0.8f);
    }
    IEnumerator InvokeWithDelayThunderBird(float delay, PlayerMove player)
    {
        yield return new WaitForSeconds(delay);
        attackThunderbird(player);
    }
    void attackThunderbird(PlayerMove player)
    {
        _tagThunderbird = false;
        float value = player.playerImpact.getDamageSkill3() * (100 + player.playerImpact.getPercentDamage()) / 100;
        if (blood - value <= 0f)
        {
            player.getTotalByEnemy(gold);
        }
        photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, value);
        GenerateValueDamage(value);

        bl.SetActive(true);
        Invoke("CloseBlood", 0.8f);
    }
    void CloseBlood()
    {
        bl.SetActive(false);
    }
    public void setBloodEff()
    {
        if (!bl.activeSelf)
            bl.SetActive(true);
        photonView.RPC("UpdateBloodRPC", RpcTarget.AllBuffered, 10f);
    }
    void hitPlayerAttack()
    {
        spr.color = new Color(1f, 1f, 1f, 0.5f);
        Invoke("resetColor", 0.12f);
    }
    void resetColor()
    {
        spr.color = new Color(1f, 1f, 1f, 1f);
    }
    [PunRPC]
    void PlayAnimation(string animationName)
    {
        anm.Play(animationName);
    }
    [PunRPC]
    void SyncConstrain(bool state)
    {
        if (state) // đứng yên
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else // di chuyển
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    [PunRPC]
    void SyncFlipX(bool flipState)
    {
        spr.flipX = flipState;
    }
    [PunRPC]
    void UpdateBloodRPC(float k)
    {
        blood -= k;
        blood_.setBlood(blood);
    }
    [PunRPC]
    void reStart()
    {
        blood = bloodDefault;
        isEfect = false;
        isEfectSkillBem = false;
        isDie = false;
        isRandom = false;
        isSkill = false;
        isChangeConstrains = false;
        isChoose = false;
        _tagThunderbird = false;
        _tagLight = false;
        photonView.RPC("PlayAnimation", RpcTarget.All, "idle");
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player = null;
        bl.SetActive(false);
        blood_.setBlood(blood);
    }
}