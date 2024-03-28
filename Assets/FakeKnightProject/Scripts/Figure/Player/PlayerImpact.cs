using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerImpact : MonoBehaviour
{
    [SerializeField] public float blood, energy;
    [SerializeField] private float percentResistance; // phần trăm chống chịu
    [SerializeField] private float percentSpeed;      // phần trăm tốc chạy
    [SerializeField] private float percentDamage;     // phần trăm sát thương
    [SerializeField] private float heals;     // Số máu hồi
    [SerializeField] private bool isHealing;     
    void Start()
    {
        blood = 1000f;
        energy = 100f;
        isHealing = false;
        heals = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHealing)
        {
            isHealing = false;
            Invoke("setHealing", 0.1f);
        }    
        if (Input.GetMouseButtonDown(0))
        {
            ObjUse.instance._blood.setBlood(blood);
            ObjUse.instance._energy.setBlood(energy);
        }
    }
    void setHealing()
    {
        if (heals <= 0f)
            return;
        heals -= 5f;
        blood += 5f;
        setBlood(0f);
        isHealing = true;
    }    
    public void setIsHealing()
    {
        isHealing = true;
        heals = 50f;
    }
    public void setBlood(float k)
    {
        blood -= k;
        ObjUse.instance._blood.setBlood(ObjUse.instance.playerImpact.blood);
        ObjUse.instance.bl.text = $"{ObjUse.instance.playerImpact.blood} / 1000";
    }
    public void setEnergy(float k)
    {
        energy -= k;
        ObjUse.instance._energy.setBlood(energy);
        ObjUse.instance.ene.text = $"{blood} / 100";
    }
}
