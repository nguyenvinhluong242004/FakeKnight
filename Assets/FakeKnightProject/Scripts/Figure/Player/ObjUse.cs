using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class ObjUse : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera cam;
    [SerializeField] public RectTransform sta, bgSta;
    [SerializeField] public Blood _blood, _energy;
    [SerializeField] public TMP_Text bl, ene;
    [SerializeField] public TMP_Text textGold, textDiamondPurple, canvasTextGold, canvasTextDiamondRed, canvasTextDiamondPurple;
    [SerializeField] public BagContent bagContent;
    [SerializeField] public LoadDataPlayer loadDataPlayer;
    [SerializeField] public GameObject scanner, scannerFire, scannerFires, center;
    [SerializeField] public Transform sk1, sk2, sk3, sk4;
    [SerializeField] public Skill _sk1, _sk2, _sk3, _sk4;
    [SerializeField] public PlayerMove player;
    [SerializeField] public ObjectManager objectManager;
    [SerializeField] public DataPlayer dataPlayer;
}
