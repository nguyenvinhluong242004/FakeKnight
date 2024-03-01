using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UseSkill : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject arrow;
    [SerializeField] public GameObject location;
    [SerializeField] public GameObject fire;
    [SerializeField] public GameObject fires;
    [SerializeField] public GameObject bem;
    public string Location = "Location";
    public string Fire = "Violent Fire";
    public string Fires = "Fires";
    public string Bem = "Bem";
    public void getSkillOne()
    {
        if (playerMove.isD)
            bem = PhotonNetwork.Instantiate(this.Bem, new Vector3(playerMove.transform.position.x, playerMove.transform.position.y - 0.6f, 0), Quaternion.Euler(0, 0, 0));
        else if (playerMove.isU)
            bem = PhotonNetwork.Instantiate(this.Bem, new Vector3(playerMove.transform.position.x, playerMove.transform.position.y + 0.3f, 0), Quaternion.Euler(0, 0, 180f));
        else if (playerMove.sprite.flipX)
            bem = PhotonNetwork.Instantiate(this.Bem, new Vector3(playerMove.transform.position.x - 0.6f, playerMove.transform.position.y, 0), Quaternion.Euler(0, 0, -90f));
        else
            bem = PhotonNetwork.Instantiate(this.Bem, new Vector3(playerMove.transform.position.x + 0.6f, playerMove.transform.position.y, 0), Quaternion.Euler(0, 0, 90f));
        Invoke("resetSkill1", 1f);
    }   
    public void getSkill()
    {
        playerMove.worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(playerMove.worldPosition);
        if (!playerMove.twoSkill && playerMove.objUse.scanner.activeSelf)
        {
            Vector2 v = playerMove.worldPosition - playerMove.objUse.scanner.transform.position;
            if (v.magnitude < 3.9f)
            {
                playerMove.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                playerMove.isSkill = true;
                playerMove.isChooseSk2 = false;
                playerMove.objUse._sk2.isOn = true;
                playerMove.twoSkill = true;
                playerMove.objUse.scanner.SetActive(false);
                Debug.Log("choose");
                if (playerMove.worldPosition.x - playerMove.objUse.scanner.transform.position.x >= 0)
                {
                    location = PhotonNetwork.Instantiate(this.Location, new Vector3(playerMove.worldPosition.x, playerMove.worldPosition.y, 0), Quaternion.Euler(0, 0, 0f));
                    playerMove.isRightLoca = true;
                    playerMove.sprite.flipX = false;
                }
                else
                {
                    location = PhotonNetwork.Instantiate(this.Location, new Vector3(playerMove.worldPosition.x, playerMove.worldPosition.y, 0), Quaternion.Euler(0, 0, 180f));
                    playerMove.isRightLoca = false;
                    playerMove.sprite.flipX = true;
                }
                playerMove.isSetLocation = true;
                playerMove.isFlag = true;
                //gameObject.SetActive(false);
                playerMove.sprite.enabled = false;
                playerMove.isLR = true;
                playerMove.isUD = false;
                playerMove.isU = false;
                playerMove.isD = false;
                Invoke("resetLocation", 0.6f);
            }
        }
        else if (!playerMove.threeSkill && playerMove.objUse.scannerFire.activeSelf)
        {
            playerMove.isChooseSk3 = false;
            playerMove.isSkill = true;
            playerMove.objUse._sk3.isOn = true;
            playerMove.threeSkill = true;
            fire = PhotonNetwork.Instantiate(this.Fire, new Vector3(playerMove.worldPosition.x, playerMove.worldPosition.y, 0), Quaternion.identity);
            playerMove.isSetFire = true;
            playerMove.objUse.scannerFire.SetActive(false);
            Invoke("resetFire", 0.7f);

        }
        else if (!playerMove.fourSkill && playerMove.objUse.scannerFires.activeSelf)
        {
            playerMove.isChooseSk4 = false;
            playerMove.isSkill = true;
            playerMove.objUse._sk4.isOn = true;
            playerMove.fourSkill = true;
            fires = PhotonNetwork.Instantiate(this.Fires, new Vector3(playerMove.worldPosition.x - 1f, playerMove.worldPosition.y + 3.6f, 0), Quaternion.identity);

            playerMove.objUse.scannerFires.SetActive(false);
            Invoke("resetFires", 1.2f);

        }
    }
    void resetSkill1()
    {
        playerMove.isSkill = false;
        playerMove.oneSkill = false;
        if (GetComponent<PhotonView>().IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(bem);
        }
    }    
    void resetLocation()
    {
        //gameObject.SetActive(true);
        playerMove.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        playerMove.sprite.enabled = true;
        location.SetActive(false);
        if (GetComponent<PhotonView>().IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(location);
        }
        playerMove.isSetLocation = false;
        playerMove.rb.velocity = new Vector2(0, 0);
        playerMove.isSkill = false;
        //playerMove.twoSkill = false;
    }
    void resetFire()
    {
        fire.SetActive(false);
        if (GetComponent<PhotonView>().IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(fire);
        }
        playerMove.isSetFire = false;
        playerMove.isSkill = false;
        //playerMove.threeSkill = false;
    }
    void resetFires()
    {
        fires.SetActive(false);
        if (GetComponent<PhotonView>().IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(fires);
        }
        playerMove.isSkill = false;
        //playerMove.fourSkill = false;
    }

    public void resetSkill()
    {
        playerMove.isSkill = false;
        //oneSkill = false;
    }
    public void setSkillHero()
    {
        playerMove.isSkill = true;
        playerMove.oneSkill = true;
        playerMove.objUse._sk1.timeSkill();
        playerMove.objUse._sk1.isOn = true;
        if (playerMove.idPlayer == 1)
        {
            GameObject _arrow = Instantiate(arrow, transform.position, arrow.transform.rotation);
            Arrow __arrow = _arrow.GetComponent<Arrow>();
            if (playerMove.isU)
            {
                __arrow.y = 7f;
                __arrow.x = 0f;
                __arrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                __arrow.transform.position += new Vector3(0f, 0.1f, 0f);
            }
            else if (playerMove.isD)
            {
                __arrow.y = -7f;
                __arrow.x = 0f;
                __arrow.transform.eulerAngles = new Vector3(0f, 0f, 180f);
                __arrow.transform.position += new Vector3(0f, -0.2f, 0f);
            }
            else
            {
                if (playerMove.sprite.flipX)
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
    public void setSkill2()
    {
        //playerMove.isSkill = true;
        playerMove.objUse.scanner.SetActive(true);
        playerMove.objUse._sk2.timeSkill();
    }
    public void setSkill3()
    {
        //playerMove.isSkill = true;
        playerMove.objUse.scannerFire.SetActive(true);
        playerMove.objUse._sk3.timeSkill();
    }
    public void setSkill4()
    {
        //playerMove.isSkill = true;
        playerMove.objUse.scannerFires.SetActive(true);
        playerMove.objUse._sk4.timeSkill();
    }
}
