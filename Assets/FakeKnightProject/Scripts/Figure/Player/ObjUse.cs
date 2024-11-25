using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using Photon.Pun;

public class ObjUse : MonoBehaviour
{
    [SerializeField] public static ObjUse instance;
    [SerializeField] public PhotonView targetPhotonView;
    [SerializeField] public CinemachineVirtualCamera cam;
    [SerializeField] public RectTransform sta, bgSta;
    [SerializeField] public Blood _blood;
    [SerializeField] public Blood _energy;
    [SerializeField] public TMP_Text bl, ene;
    [SerializeField] public TMP_Text textGold;
    [SerializeField] public TMP_Text textDiamondPurple;
    [SerializeField] public TMP_Text canvasTextGold;
    [SerializeField] public TMP_Text canvasTextDiamondRed;
    [SerializeField] public TMP_Text canvasTextDiamondPurple;
    [SerializeField] public BagContent bagContent;
    [SerializeField] public GameObject scanner;
    [SerializeField] public GameObject scannerFire; 
    [SerializeField] public GameObject scannerFires;
    [SerializeField] public Transform sk1;
    [SerializeField] public Transform sk2;
    [SerializeField] public Transform sk3;
    [SerializeField] public Transform sk4;
    [SerializeField] public Skill _sk1;
    [SerializeField] public Skill _sk2;
    [SerializeField] public Skill _sk3;
    [SerializeField] public Skill _sk4;
    [SerializeField] public PlayerMove player;
    [SerializeField] public PlayerImpact playerImpact;
    [SerializeField] public DataPlayer dataPlayer;
    [SerializeField] public MoneyPlayer moneyPlayer;
    [SerializeField] public TMP_Text messageOldMan;
    [SerializeField] public float damageTotal;
    [SerializeField] public float resistanceTotal;
    [SerializeField] public float speedTotal;
    [SerializeField] public TMP_Text exp;
    [SerializeField] public TMP_Text damage;
    [SerializeField] public TMP_Text resistance;
    [SerializeField] public TMP_Text speed;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        damageTotal = 0f;
        resistanceTotal = 0f;
        speedTotal = 0f;
    }
    public void setTextPercent()
    {
        damage.text = "Damage: " + ((int)damageTotal).ToString() + "%";
        resistance.text = "Resistance: " + ((int)resistanceTotal).ToString() + "%";
        speed.text = "Speed: " + ((int)speedTotal).ToString() + "%";
    }
    public void changePercent(float _damage, float _resist, float _speed, string status)
    {
        if (status == "add")
        {
            damageTotal += _damage;
            resistanceTotal += _resist;
            speedTotal += _speed;
        }
        else
        {
            damageTotal -= _damage;
            resistanceTotal -= _resist;
            speedTotal -= _speed;
        }
        setTextPercent();
    }
}
