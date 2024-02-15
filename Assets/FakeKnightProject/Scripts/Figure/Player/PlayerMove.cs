using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public Animator anm;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public int number;
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D rb;
    float horizontalInput;
    float verticalInput;
    [SerializeField] public float speed;

    [SerializeField] public bool isSetLocation, isSetFire, isFlag, isRightLoca, isU, isD, isLR, isUD, isSkill, oneSkill, twoSkill, threeSkill, fourSkill;
    [SerializeField] public bool isChooseSk1, isChooseSk2, isChooseSk3, isChooseSk4;
    [SerializeField] private Joytick joytick;
    [SerializeField] private GameObject arrow;
    [SerializeField] private CircleCollider2D cir;
    [SerializeField] public PlayerImpact playerImpact;
    [SerializeField] public GameObject scanner, scannerFire, scannerFires, Location, location, Fire, fire, Fires, fires, center;
    public Vector3 worldPosition;
    Vector3 k, kk;
    Vector2 velocity_;
    private Vector2 startingPoint;
    public Vector3 pastPlayer;
    int leftTouch = 99;
    int rightTouch = 99;
    [SerializeField] public RectTransform sta, bgSta;
    [SerializeField] public Transform sk1, sk2, sk3, sk4;
    [SerializeField] public Skill _sk1, _sk2, _sk3, _sk4;
    // Start is called before the first frame update
    void Start()
    {
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
                    if (!oneSkill && touchPos.x > sk1.position.x - 0.6f && touchPos.x < sk1.position.x + 0.6f && touchPos.y > sk1.position.y - 0.6f && touchPos.y < sk1.position.y + 0.6f)
                    {
                        Debug.Log("use skill 1!");
                        setSkillHero();
                        isChooseSk1 = true;
                    }
                    else if (!twoSkill && touchPos.x > sk2.position.x - 0.6f && touchPos.x < sk2.position.x + 0.6f && touchPos.y > sk2.position.y - 0.6f && touchPos.y < sk2.position.y + 0.6f)
                    {
                        Debug.Log("use skill 2!");
                        if (isChooseSk2)
                        {
                            isSkill = false;
                            _sk2.ResetSK();
                            scanner.SetActive(false);
                            isChooseSk2 = false;
                        }    
                        else
                        {
                            isChooseSk2 = true;
                            setSkill2();
                        }    
                    }
                    else if (!threeSkill && touchPos.x > sk3.position.x - 0.6f && touchPos.x < sk3.position.x + 0.6f && touchPos.y > sk3.position.y - 0.6f && touchPos.y < sk3.position.y + 0.6f)
                    {
                        Debug.Log("use skill 3!");
                        if (isChooseSk3)
                        {
                            isSkill = false;
                            _sk3.ResetSK();
                            scanner.SetActive(false);
                            isChooseSk3 = false;
                        }
                        else
                        {
                            isChooseSk3 = true;
                            setSkill3();
                        }
                    }
                    else if (!fourSkill && touchPos.x > sk4.position.x - 0.6f && touchPos.x < sk4.position.x + 0.6f && touchPos.y > sk4.position.y - 0.6f && touchPos.y < sk4.position.y + 0.6f)
                    {
                        Debug.Log("use skill 4!");
                        if (isChooseSk4)
                        {
                            isSkill = false;
                            _sk4.ResetSK();
                            scanner.SetActive(false);
                            isChooseSk4 = false;
                        }
                        else
                        {
                            isChooseSk4 = true;
                            setSkill4();
                        }
                    }
                    else if (isChooseSk2 || isChooseSk3 || isChooseSk4)
                        getSkill();
                }
                else
                {
                    if (touchPos.x > sta.position.x - 1f && touchPos.x < sta.position.x + 1f && touchPos.y > sta.position.y - 1f && touchPos.y < sta.position.y + 1f)
                    {
                        leftTouch = t.fingerId;
                        startingPoint = touchPos;
                        pastPlayer = transform.position;
                        joytick.choose(true);
                    }
                    else if (isChooseSk2 || isChooseSk3 || isChooseSk4)
                        getSkill();
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

                sta.transform.position = new Vector3(bgSta.transform.position.x + direction.x, bgSta.transform.position.y + direction.y, sta.transform.position.z);

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
                sta.transform.position = new Vector3(bgSta.transform.position.x, bgSta.transform.position.y, sta.transform.position.z);
                velocity_ = new Vector2(0, 0);
            }
            rb.velocity = velocity_ * speed;
            //if (isTouch)
            //{
            //    rb.velocity = new Vector2(rb.velocity.x, 6.5f);
            //    isTouch = false;
            //}
            ++i;
        }
        UpdateAnimationMobile();
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
        UpdateAnimation();
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
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }
    void getSkill()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(worldPosition);
        if (!twoSkill && scanner.activeSelf)
        {
            Vector2 v = worldPosition - scanner.transform.position;
            if (v.magnitude < 3.9f)
            {
                isSkill = true;
                isChooseSk2 = false;
                _sk2.isOn = true;
                twoSkill = true;
                scanner.SetActive(false);
                Debug.Log("choose");
                if (worldPosition.x - scanner.transform.position.x >= 0)
                {
                    location = Instantiate(Location, new Vector3(worldPosition.x, worldPosition.y, Location.transform.position.z), Location.transform.rotation);
                    isRightLoca = true;
                    sprite.flipX = false;
                }
                else
                {
                    location = Instantiate(Location, new Vector3(worldPosition.x, worldPosition.y, Location.transform.position.z), Quaternion.Euler(0, 0, 180f));
                    isRightLoca = false;
                    sprite.flipX = true;
                }
                isSetLocation = true;
                isFlag = true;
                //gameObject.SetActive(false);
                sprite.enabled = false;
                isLR = true;
                isUD = false;
                isU = false;
                isD = false;
                Invoke("resetLocation", 0.6f);
            }
        }
        else if (!threeSkill && scannerFire.activeSelf)
        {
            isChooseSk3 = false;
            isSkill = true;
            _sk3.isOn = true;
            threeSkill = true;
            fire = Instantiate(Fire, new Vector3(worldPosition.x, worldPosition.y, Fire.transform.position.z), Fire.transform.rotation);
            isSetFire = true;
            scannerFire.SetActive(false);
            Invoke("resetFire", 0.7f);

        }
        else if (!fourSkill && scannerFires.activeSelf)
        {
            isChooseSk4 = false;
            isSkill = true;
            _sk4.isOn = true;
            center.transform.position = new Vector3(worldPosition.x, worldPosition.y, center.transform.position.z);
            fourSkill = true;
            center.SetActive(true);
            fires = Instantiate(Fires, new Vector3(worldPosition.x - 1f, worldPosition.y + 3.6f, Fires.transform.position.z), Fires.transform.rotation);

            scannerFires.SetActive(false);
            Invoke("resetFires", 1.2f);

        }
    }
    void resetLocation()
    {
        //gameObject.SetActive(true);
        sprite.enabled = true;
        location.SetActive(false);
        Destroy(location);
        isSetLocation = false;
        rb.velocity = new Vector2(0, 0);
        isSkill = false;
        //twoSkill = false;
    }
    void resetFire()
    {
        fire.SetActive(false);
        Destroy(fire);
        isSetFire = false;
        isSkill = false;
        //threeSkill = false;
    }
    void resetFires()
    {
        fires.SetActive(false);
        Destroy(fires);
        center.SetActive(false);
        isSkill = false;
        //fourSkill = false;
    }
    void UpdateAnimation()
    {
        if (!isSkill)
        {
            if (horizontalInput > 0.01f)
            {
                if (sprite.flipX)
                    sprite.flipX = !sprite.flipX;
                anm.Play("runLR");
                isLR = true;
                isUD = false;
                isU = false;
                isD = false;
            }
            else if (horizontalInput < -0.01f)
            {
                if (!sprite.flipX)
                    sprite.flipX = !sprite.flipX;
                anm.Play("runLR");
                isLR = true;
                isUD = false;
                isU = false;
                isD = false;
            }
            else if (horizontalInput == 0f)
            {
                isLR = false;
            }


            if (verticalInput > 0.01f)
            {
                anm.Play("runBehind");
                isUD = true;
                isU = true;
                isD = false;
                isLR = false;
            }
            else if (verticalInput < -0.01f)
            {
                anm.Play("runFront");
                isUD = true;
                isD = true;
                isU = false;
                isLR = false;
            }
            else if (verticalInput == 0f)
            {
                isUD = false;
            }

            if (!isLR && !isUD)
            {
                if (isU)
                    anm.Play("idleBehind");
                else if (isD)
                    anm.Play("idleFront");
                else
                    anm.Play("idleLR");
            }
        }
        else if (isChooseSk1)
        {
            isChooseSk1 = false;
            if (isU)
            {
                anm.Play("skillBehind");
            }
            else if (isD)
            {
                anm.Play("skillFront");
            }
            else
            {
                anm.Play("skillLR");
            }
            Invoke("resetSkill", 0.3f);
        }
    }
    void UpdateAnimationMobile()
    {
        if (!isSkill)
        {
            if (velocity_.x > 0.01f && Mathf.Abs(velocity_.x) >= 0.4f)
            {
                if (sprite.flipX)
                    sprite.flipX = !sprite.flipX;
                anm.Play("runLR");
                isLR = true;
                isUD = false;
                isU = false;
                isD = false;
            }
            else if (velocity_.x < -0.01f && Mathf.Abs(velocity_.x) >= 0.4f)
            {
                if (!sprite.flipX)
                    sprite.flipX = !sprite.flipX;
                anm.Play("runLR");
                isLR = true;
                isUD = false;
                isU = false;
                isD = false;
            }
            else if (velocity_.x == 0f)
            {
                isLR = false;
            }


            if (velocity_.y > 0.01f && Mathf.Abs(velocity_.x) < 0.4f)
            {
                anm.Play("runBehind");
                isUD = true;
                isU = true;
                isD = false;
                isLR = false;
            }
            else if (velocity_.y < -0.01f && Mathf.Abs(velocity_.x) < 0.4f)
            {
                anm.Play("runFront");
                isUD = true;
                isD = true;
                isU = false;
                isLR = false;
            }
            else if (velocity_.y == 0f)
            {
                isUD = false;

            }

            if (!isLR && !isUD)
            {
                if (isU)
                    anm.Play("idleBehind");
                else if (isD)
                    anm.Play("idleFront");
                else
                    anm.Play("idleLR");
            }
        }
        else if (isChooseSk1) 
        {
            isChooseSk1 = false;
            if (isU) 
            {
                anm.Play("skillBehind");
            }
            else if (isD)
            {
                anm.Play("skillFront");
            }
            else
            {
                anm.Play("skillLR");
            }
            Invoke("resetSkill", 0.3f);
        }
    }
    void resetSkill()
    {
        isSkill = false;
        //oneSkill = false;
    }
    void setSkillHero()
    {
        isSkill = true;
        oneSkill = true;
        _sk1.timeSkill();
        _sk1.isOn = true;
        if (number == 1)
        {
            GameObject _arrow = Instantiate(arrow, transform.position, arrow.transform.rotation);
            Arrow __arrow = _arrow.GetComponent<Arrow>();
            if (isU)
            {
                __arrow.y = 7f;
                __arrow.x = 0f;
                __arrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                __arrow.transform.position += new Vector3(0f, 0.1f, 0f);
            }
            else if (isD)
            {
                __arrow.y = -7f;
                __arrow.x = 0f;
                __arrow.transform.eulerAngles = new Vector3(0f, 0f, 180f);
                __arrow.transform.position += new Vector3(0f, -0.2f, 0f);
            }
            else
            {
                if (sprite.flipX)
                {
                    __arrow.y = 0f;
                    __arrow.x = -7f;
                    __arrow.transform.eulerAngles = new Vector3(0f, 0f, 90f);
                    __arrow.transform.position += new Vector3(-0.2f, -0.3f, 0f);
                }
                else
                {
                    __arrow.y = 0f;
                    __arrow.x = 7f;
                    __arrow.transform.eulerAngles = new Vector3(0f, 0f, -90f);
                    __arrow.transform.position += new Vector3(0.2f, -0.3f, 0f);
                }
            }
            __arrow.isOn = true;
        }
    }
    void setSkill2()
    {
        //isSkill = true;
        scanner.SetActive(true);
        _sk2.timeSkill();
    }
    void setSkill3()
    {
        //isSkill = true;
        scannerFire.SetActive(true);
        _sk3.timeSkill();
    }
    void setSkill4()
    {
        //isSkill = true;
        scannerFires.SetActive(true);
        _sk4.timeSkill();
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
    public void setBoxCollider()
    {
        if (number == 0)
        {
            cir.offset = new Vector2(cir.offset.x, -0.72f);
        }
        else if (number == 1)
        {
            cir.offset = new Vector2(cir.offset.x, -0.47f);
        }
    }
}
