using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerImpact : MonoBehaviour
{
    [SerializeField] public float blood, energy;
    [SerializeField] private ObjUse objUse;
    void Start()
    {
        objUse = FindObjectOfType<ObjUse>();
        blood = 100f;
        energy = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            objUse._blood.setBlood(blood);
            objUse._energy.setBlood(energy);
        }
    }
    public void setBlood(float k)
    {
        blood -= k;
        objUse._blood.setBlood(blood);
        objUse.bl.text = $"{blood} / 100";
    }
    public void setEnergy(float k)
    {
        energy -= k;
        objUse._energy.setBlood(energy);
        objUse.ene.text = $"{blood} / 100";
    }
}
