using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public PlayerNameID playerNameID;
    [SerializeField] public TMP_Text textName;
    [SerializeField] private UseSkill useSkill;
    [SerializeField] private AnmPlayer anmPlayer;
    [SerializeField] public Animator anm;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public int idPlayer;
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] public Rigidbody2D rb;
    public float horizontalInput;
    public float verticalInput;
    [SerializeField] public float speed;

    [SerializeField] public bool isSetLocation, isSetFire;
    [SerializeField] public bool isFlag, isRightLoca;
    [SerializeField] public bool isU, isD, isLR,isUD;
    [SerializeField] public bool isSkill;
    [SerializeField] public bool oneSkill, twoSkill, threeSkill, fourSkill;
    [SerializeField] public bool isChooseSk1, isChooseSk2, isChooseSk3, isChooseSk4;
    [SerializeField] private Joytick joytick;
    [SerializeField] public PlayerImpact playerImpact;
    public Vector3 worldPosition;
    Vector3 k, kk;
    public Vector2 velocity_;
    private Vector2 startingPoint;
    public Vector3 pastPlayer;
    int leftTouch = 99;
    int rightTouch = 99;
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        if (GetComponent<PhotonView>().IsMine)
        {
            ObjUse.instance.player = gameObject.GetComponent<PlayerMove>();
            ObjUse.instance.playerImpact = gameObject.GetComponent<PlayerImpact>();
            ObjUse.instance.cam.Follow = gameObject.transform;
            ObjUse.instance.moneyPlayer = gameObject.GetComponent<MoneyPlayer>();
        }
        else
        {
            Debug.Log(GetComponent<PhotonView>().Owner.NickName + "own");
            string nickName = GetComponent<PhotonView>().Owner.NickName;
            string[] parts = nickName.Split('-');
            if (parts.Length == 4)
            {
                string displayname = parts[0].Trim();
                string name = parts[1].Trim();
                int id = int.Parse(parts[2].Trim());
                string playfabID = parts[3].Trim();
                Debug.Log("Name: " + name + "Id: " + id + "PlayfabID: " + playfabID);
                textName.text = name;
                setPlayer(id);
                playerNameID.displayName = displayname;
                playerNameID.nickName = name;
                playerNameID.playfabID = playfabID;
            }
        }
        LoadDataPlayer.instance.moneyPlayer = gameObject.GetComponent<MoneyPlayer>();
        LoadDataPlayer.instance.moneyPlayer.setValue();
        isU = false;
        isD = true;
        isLR = false;
        isUD = false;
        isSkill = false;
        oneSkill = false;
        twoSkill = false;
        threeSkill = false;
        isSetLocation = false;
        isRightLoca = false;
        isFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // move from mobile
        if (GetComponent<PhotonView>().IsMine)
        {
            int i = 0;
            while (i < Input.touchCount)
            {
                Touch t = Input.GetTouch(i);
                Vector2 touchPos = getTouchPosition(t.position); // * -1 for perspective cameras
                if (t.phase == TouchPhase.Began)
                {
                    if (t.position.x > Screen.width / 2)
                    {
                        rightTouch = t.fingerId;
                        if (!oneSkill && !isChooseSk1 && touchPos.x > ObjUse.instance.sk1.position.x - 0.6f && touchPos.x < ObjUse.instance.sk1.position.x + 0.6f && touchPos.y > ObjUse.instance.sk1.position.y - 0.6f && touchPos.y < ObjUse.instance.sk1.position.y + 0.6f)
                        {
                            Debug.Log("use skill 1!");
                            // fix late: đang
                            //useSkill.setSkillHero();
                            isSkill = true;
                            oneSkill = true;
                            isChooseSk1 = true;
                            useSkill.getSkillOne();
                            ObjUse.instance._sk1.timeSkill();
                            ObjUse.instance._sk1.isOn = true;
                        }
                        else if (!twoSkill && touchPos.x > ObjUse.instance.sk2.position.x - 0.6f && touchPos.x < ObjUse.instance.sk2.position.x + 0.6f && touchPos.y > ObjUse.instance.sk2.position.y - 0.6f && touchPos.y < ObjUse.instance.sk2.position.y + 0.6f)
                        {
                            Debug.Log("use skill 2!");
                            if (isChooseSk2)
                            {
                                isSkill = false;
                                ObjUse.instance._sk2.ResetSK();
                                ObjUse.instance.scanner.SetActive(false);
                                isChooseSk2 = false;
                            }
                            else
                            {
                                isChooseSk2 = true;
                                useSkill.setSkill2();
                            }
                        }
                        else if (!threeSkill && touchPos.x > ObjUse.instance.sk3.position.x - 0.6f && touchPos.x < ObjUse.instance.sk3.position.x + 0.6f && touchPos.y > ObjUse.instance.sk3.position.y - 0.6f && touchPos.y < ObjUse.instance.sk3.position.y + 0.6f)
                        {
                            Debug.Log("use skill 3!");
                            if (isChooseSk3)
                            {
                                isSkill = false;
                                ObjUse.instance._sk3.ResetSK();
                                ObjUse.instance.scannerFire.SetActive(false);
                                isChooseSk3 = false;
                            }
                            else
                            {
                                isChooseSk3 = true;
                                useSkill.setSkill3();
                            }
                        }
                        else if (!fourSkill && touchPos.x > ObjUse.instance.sk4.position.x - 0.6f && touchPos.x < ObjUse.instance.sk4.position.x + 0.6f && touchPos.y > ObjUse.instance.sk4.position.y - 0.6f && touchPos.y < ObjUse.instance.sk4.position.y + 0.6f)
                        {
                            Debug.Log("use skill 4!");
                            if (isChooseSk4)
                            {
                                isSkill = false;
                                ObjUse.instance._sk4.ResetSK();
                                ObjUse.instance.scannerFires.SetActive(false);
                                isChooseSk4 = false;
                            }
                            else
                            {
                                isChooseSk4 = true;
                                useSkill.setSkill4();
                            }
                        }
                        else if (isChooseSk2 || isChooseSk3 || isChooseSk4)
                            useSkill.getSkill();
                    }
                    else
                    {
                        if (touchPos.x > ObjUse.instance.sta.position.x - 1f && touchPos.x < ObjUse.instance.sta.position.x + 1f && touchPos.y > ObjUse.instance.sta.position.y - 1f && touchPos.y < ObjUse.instance.sta.position.y + 1f)
                        {
                            leftTouch = t.fingerId;
                            startingPoint = touchPos;
                            pastPlayer = transform.position;
                            joytick.choose(true);
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        }
                        else if (isChooseSk2 || isChooseSk3 || isChooseSk4)
                            useSkill.getSkill();
                    }
                }
                else if (t.phase == TouchPhase.Moved && leftTouch == t.fingerId)
                {
                    Debug.Log("move");
                    Vector2 change = transform.position - pastPlayer;
                    pastPlayer = transform.position;
                    startingPoint += change;

                    Vector2 offset = touchPos - startingPoint;
                    Vector2 direction = Vector2.ClampMagnitude(offset, 1f);

                    velocity_ = direction.normalized;

                    ObjUse.instance.sta.transform.position = new Vector3(ObjUse.instance.bgSta.transform.position.x + direction.x, ObjUse.instance.bgSta.transform.position.y + direction.y, ObjUse.instance.sta.transform.position.z);

                }
                else if (t.phase == TouchPhase.Ended && rightTouch == t.fingerId)
                {
                    rightTouch = 99;
                }
                else if (t.phase == TouchPhase.Ended && leftTouch == t.fingerId)
                {
                    Debug.Log("don't move");
                    leftTouch = 99;
                    joytick.choose(false);
                    ObjUse.instance.sta.transform.position = new Vector3(ObjUse.instance.bgSta.transform.position.x, ObjUse.instance.bgSta.transform.position.y, ObjUse.instance.sta.transform.position.z);
                    velocity_ = new Vector2(0, 0);
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                }
                rb.velocity = velocity_ * speed;
                //if (isTouch)
                //{
                //    rb.velocity = new Vector2(rb.velocity.x, 6.5f);
                //    isTouch = false;
                //}
                ++i;
            }
            anmPlayer.UpdateAnimationMobile();
            // Move from keyboard
            /*
            if (Input.GetMouseButtonDown(0))
            {
                getSkill();
            }

            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            if (Input.GetKeyDown("space") && !isSkill)
            {
                setSkillHero();
            }
            if (Input.GetKeyDown("u") && !isSkill)
            {
                setSkill2();
            }
            if (Input.GetKeyDown("i") && !isSkill)
            {
                setSkill3();
            }
            if (Input.GetKeyDown("o") && !isSkill)
            {
                setSkill4();
            }
            setJoytick();
            Vector2 velocity = new Vector2(horizontalInput, verticalInput);
            //rb.velocity = velocity * speed;
            if (!isSetLocation)
            {
                rb.velocity = velocity * speed;
            }
            */
            if (isSetLocation)
            {
                if (isFlag)
                {
                    if (isRightLoca)
                    {
                        kk = new Vector3(-1.5f, 0, 0);

                    }
                    else
                    {
                        kk = new Vector3(1.5f, 0, 0);

                    }
                    isFlag = false;
                    k = worldPosition + kk - transform.position;
                    //if (k.magnitude>2f)
                    {
                        k = k.normalized * 2.4f;
                    }
                    //else { }
                }
                if (Mathf.Abs(transform.position.y - worldPosition.y) < 0.2f && Mathf.Abs(transform.position.x - worldPosition.x - kk.x) < 0.2f)
                {
                    if (isRightLoca)
                    {
                        k.x = 1f;
                        k.y = 0;
                    }
                    else
                    {
                        k.x = -1f;
                        k.y = 0;
                    }
                }
                rb.velocity = new Vector2(k.x, k.y) * 10f;

            }
            //mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        }
    }
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }
    public void setPlayer(int id)
    {
        // set figure
        idPlayer = id;
        Debug.Log(idPlayer);
        anm.runtimeAnimatorController = ObjectManager.instance.anmPlayers[idPlayer];
        sprite.sprite = ObjectManager.instance.imgPlayers[idPlayer];
        if (GetComponent<PhotonView>().IsMine)
        {
            // get Music, Sound Playerr
            ObjectManager.instance.isSound = ObjUse.instance.dataPlayer.isSound;
            ObjectManager.instance.isMusic = ObjUse.instance.dataPlayer.isMusic;
            if (ObjUse.instance.dataPlayer.isMusic)
            {
                ObjectManager.instance.musicAudio.Play();
                ObjectManager.instance.music.GetComponent<Image>().sprite = ObjectManager.instance.musicOn;
            }
            else
            {
                ObjectManager.instance.music.GetComponent<Image>().sprite = ObjectManager.instance.musicOff;
            }
            if (ObjUse.instance.dataPlayer.isSound)
            {
                ObjectManager.instance.sound.GetComponent<Image>().sprite = ObjectManager.instance.musicOn;
            }
            else
            {
                ObjectManager.instance.sound.GetComponent<Image>().sprite = ObjectManager.instance.musicOff;
            }
        }
    }
    void setJoytick()
    {
        if (!joytick.isTick && (horizontalInput != 0f || verticalInput != 0f))
        {
            joytick.choose(true);
            if (isU)
            {
                joytick.setJoy(2);
            }
            else if (isD)
            {
                joytick.setJoy(8);
            }
            else if (isLR)
            {
                if (sprite.flipX)
                {
                    joytick.setJoy(4);
                }
                else
                {
                    joytick.setJoy(6);
                }
            }
        }
        if (horizontalInput == 0f && verticalInput == 0f)
        {
            joytick.choose(false);
        }
    }
    public int GetPlayerNumber()
    {
        return idPlayer;
    }
    public void hitEnemyAttack()
    {
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
        Invoke("resetColor", 0.12f);
    }
    void resetColor()
    {
        sprite.color = new Color(1f, 1f, 1f, 1f);
    }
    [PunRPC]
    void ReceiveMessage(string message, Player sender)
    {
        // Xử lý tin nhắn nhận được tại đâys
        Debug.Log("Received message: " + message);
        PlayfabFriendManager.instance.addInvite(message, sender);
    }
    [PunRPC]
    void ReceivePrivateMessage(string message)
    {
        // Xử lý tin nhắn nhận được tại đâys
        Debug.Log("Received message: " + message);
        string[] parts = message.Split(".|.");
        Debug.Log(parts[0]);
        Debug.Log(playerNameID.displayName);
        if (parts[0].Trim() == playerNameID.displayName)
        {
            PhotonChat.instance.setMessagePrivateReceive(parts[0].Trim(), parts[1].Trim(), parts[2].Trim());
        }
    }
    [PunRPC]
    void AddFriend(string recipient)
    {
        PlayfabFriendManager.instance.addFriendPlayfabBySender(recipient);
    }
}
