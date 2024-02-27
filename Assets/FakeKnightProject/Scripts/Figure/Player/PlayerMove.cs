using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private UseSkill useSkill;
    [SerializeField] private AnmPlayer anmPlayer;
    [SerializeField] public Animator anm;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public int number;
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] public Rigidbody2D rb;
    public float horizontalInput;
    public float verticalInput;
    [SerializeField] public float speed;

    [SerializeField] public bool isSetLocation, isSetFire, isFlag, isRightLoca, isU, isD, isLR, isUD, isSkill, oneSkill, twoSkill, threeSkill, fourSkill;
    [SerializeField] public bool isChooseSk1, isChooseSk2, isChooseSk3, isChooseSk4;
    [SerializeField] private Joytick joytick;
    [SerializeField] public PlayerImpact playerImpact;
    [SerializeField] public GameObject Location, location, Fire, fire, Fires, fires;
    public Vector3 worldPosition;
    Vector3 k, kk;
    public Vector2 velocity_;
    private Vector2 startingPoint;
    public Vector3 pastPlayer;
    int leftTouch = 99;
    int rightTouch = 99;
    [SerializeField] public ObjUse objUse;
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        objUse = FindObjectOfType<ObjUse>();
        if (GetComponent<PhotonView>().IsMine)
        {
            objUse.player = gameObject.GetComponent<PlayerMove>();
            objUse.cam.Follow = gameObject.transform;
            GetComponent<PhotonView>().RPC("SyncPlayerID", RpcTarget.AllBuffered, number);
        }
        else
            setPlayer();
        objUse.loadDataPlayer.moneyPlayer = gameObject.GetComponent<MoneyPlayer>();
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
                        if (!oneSkill && touchPos.x > objUse.sk1.position.x - 0.6f && touchPos.x < objUse.sk1.position.x + 0.6f && touchPos.y > objUse.sk1.position.y - 0.6f && touchPos.y < objUse.sk1.position.y + 0.6f)
                        {
                            Debug.Log("use skill 1!");
                            useSkill.setSkillHero();
                            isChooseSk1 = true;
                        }
                        else if (!twoSkill && touchPos.x > objUse.sk2.position.x - 0.6f && touchPos.x < objUse.sk2.position.x + 0.6f && touchPos.y > objUse.sk2.position.y - 0.6f && touchPos.y < objUse.sk2.position.y + 0.6f)
                        {
                            Debug.Log("use skill 2!");
                            if (isChooseSk2)
                            {
                                isSkill = false;
                                objUse._sk2.ResetSK();
                                objUse.scanner.SetActive(false);
                                isChooseSk2 = false;
                            }
                            else
                            {
                                isChooseSk2 = true;
                                useSkill.setSkill2();
                            }
                        }
                        else if (!threeSkill && touchPos.x > objUse.sk3.position.x - 0.6f && touchPos.x < objUse.sk3.position.x + 0.6f && touchPos.y > objUse.sk3.position.y - 0.6f && touchPos.y < objUse.sk3.position.y + 0.6f)
                        {
                            Debug.Log("use skill 3!");
                            if (isChooseSk3)
                            {
                                isSkill = false;
                                objUse._sk3.ResetSK();
                                objUse.scanner.SetActive(false);
                                isChooseSk3 = false;
                            }
                            else
                            {
                                isChooseSk3 = true;
                                useSkill.setSkill3();
                            }
                        }
                        else if (!fourSkill && touchPos.x > objUse.sk4.position.x - 0.6f && touchPos.x < objUse.sk4.position.x + 0.6f && touchPos.y > objUse.sk4.position.y - 0.6f && touchPos.y < objUse.sk4.position.y + 0.6f)
                        {
                            Debug.Log("use skill 4!");
                            if (isChooseSk4)
                            {
                                isSkill = false;
                                objUse._sk4.ResetSK();
                                objUse.scanner.SetActive(false);
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
                        if (touchPos.x > objUse.sta.position.x - 1f && touchPos.x < objUse.sta.position.x + 1f && touchPos.y > objUse.sta.position.y - 1f && touchPos.y < objUse.sta.position.y + 1f)
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

                    objUse.sta.transform.position = new Vector3(objUse.bgSta.transform.position.x + direction.x, objUse.bgSta.transform.position.y + direction.y, objUse.sta.transform.position.z);

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
                    objUse.sta.transform.position = new Vector3(objUse.bgSta.transform.position.x, objUse.bgSta.transform.position.y, objUse.sta.transform.position.z);
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
    [PunRPC]
    void SyncPlayerID(int id)
    {
        number = id;
    }
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }
    public void setPlayer()
    {
        // set figure
        Debug.Log(number);
        objUse.objectManager.imgProfile.sprite = objUse.objectManager.imgPlayers[number];
        if (GetComponent<PhotonView>().IsMine)
        {
            // get Music, Sound Playerr
            objUse.objectManager.isSound = objUse.dataPlayer.isSound;
            objUse.objectManager._sound.isOn = objUse.dataPlayer.isSound;
            if (objUse.dataPlayer.isMusic)
                objUse.objectManager.audio.Play();
            objUse.objectManager._muic.isOn = objUse.dataPlayer.isMusic;
            objUse.objectManager._muic.isOn = objUse.dataPlayer.isMusic;
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
}
