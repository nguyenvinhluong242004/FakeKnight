using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerImpact : MonoBehaviour
{
    [SerializeField] private float blood, energy;

    [SerializeField] private float damageSkill1;
    [SerializeField] private float damageSkill2;
    [SerializeField] private float damageSkill3;
    [SerializeField] private float damageSkill4;

    [SerializeField] private float percentDamage;     // phần trăm sát thương
    [SerializeField] private float percentResistance; // phần trăm chống chịu
    [SerializeField] private float percentSpeed;      // phần trăm tốc chạy
    [SerializeField] private float heals;     // Số máu hồi
    [SerializeField] private float times;     // Thời gian hiệu lực
    [SerializeField] private bool isHealing;
    [SerializeField] private bool isHealingEnergy;
    public string ValueDamage = "ValueDamage";
    void Start()
    {
        blood = 1000f;
        energy = 100f;
        isHealing = false;
        isHealingEnergy = false;
        heals = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHealingEnergy && GetComponent<PhotonView>().IsMine)
        {
            if (energy < 100f)
            {
                isHealingEnergy = true;
                changeEnergy(1f, true);
                Invoke("resetHealingEnergy", 0.17f);
            }
        }
        if (isHealing)
        {
            isHealing = false;
            Invoke("setHealing", 0.24f);
        }    
        if (Input.GetMouseButtonDown(0))
        {
            setBlood();
        }
    }
    public void setPercent()
    {
        //float totalDamage = 0;     // phần trăm sát thương
        //float totalResistance = 0; // phần trăm chống chịu
        //float totalSpeed = 0;      // phần trăm tốc chạy


        //// xử lí equipment ( giày, giáp, mũ ...)
        //for (int i=0; i<LoadDataPlayer.instance.dataPlayer.equipments.Length; i++)
        //{
        //    int idItem = LoadDataPlayer.instance.dataPlayer.equipments[i];
        //    if (idItem >= 0)
        //    {
        //        Debug.Log(InformationItem.instance.data[0, idItem].name);
        //        totalDamage += InformationItem.instance.data[0, idItem].damage;
        //        totalResistance += InformationItem.instance.data[0, idItem].resist;
        //        totalSpeed += InformationItem.instance.data[0, idItem].speed;
        //    }
        //}

        //percentDamage = totalDamage;
        //percentResistance = totalResistance;
        //percentSpeed = totalSpeed;

        percentDamage = ObjUse.instance.damageTotal;
        percentResistance = ObjUse.instance.resistanceTotal;
        percentSpeed = ObjUse.instance.speedTotal;

        Debug.Log("thong so %");
        Debug.Log(percentDamage);
        Debug.Log(percentResistance);
        Debug.Log(percentSpeed);
    }
    void setPercentResistance(float value, bool check)
    {

    }
    public float getPercentResistance()
    {
        return percentResistance;
    }
    void setPercentSpeed(float value, bool check)
    {

    }
    public float getPercentSpeed()
    {
        return percentSpeed;
    }
    void setPercentDamage(float value, bool check)
    {

    }
    public float getPercentDamage()
    {
        return percentDamage;
    }
    void setHealing()
    {
        if (heals <= 0f)
            return;
        heals -= 5f;
        changeBlood(5f, true);
        isHealing = true;
    }    
    public void setIsHealing()
    {
        isHealing = true;
        heals = 50f;
    }
    public float getBlood()
    {
        return blood;
    }
    public float getEnergy()
    {
        return energy;
    }
    public void setBlood()
    {
        int blood = (int)ObjUse.instance.playerImpact.blood;
        ObjUse.instance._blood.setBlood(blood);
        ObjUse.instance.bl.text = $"{blood} / 1000";
    }
    public void setEnergy()
    {
        ObjUse.instance._energy.setBlood(ObjUse.instance.playerImpact.energy);
        ObjUse.instance.ene.text = $"{ObjUse.instance.playerImpact.energy} / 100";
    }
    public void changeBlood(float value, bool type)
    {
        GameObject _value = PhotonNetwork.Instantiate(this.ValueDamage, new Vector3(Random.Range(transform.position.x - 0.4f, transform.position.x + 0.4f), Random.Range(transform.position.y - 0.1f, transform.position.y + 0.2f), 100f), Quaternion.identity);

        if (type == true) // cộng máu
        {
            _value.GetComponent<ValueDamage>().value.color = new Color(0f, 1f, 0f, 1f);
            _value.GetComponent<ValueDamage>().value.text = "+ " + ((int)value).ToString();
            ObjUse.instance.playerImpact.blood += value;
            if (ObjUse.instance.playerImpact.blood > 1000f) ObjUse.instance.playerImpact.blood = 1000f;
        }
        else // giảm máu
        {
            _value.GetComponent<ValueDamage>().value.color = new Color(1f, 0f, 0f, 1f);
            _value.GetComponent<ValueDamage>().value.text = "- " + ((int)value).ToString();
            ObjUse.instance.playerImpact.blood -= value * (100 - percentResistance) / 100; 
            if (ObjUse.instance.playerImpact.blood < 0f) ObjUse.instance.playerImpact.blood = 0f;
        }
        setBlood();
    }
    public void changeEnergy(float value, bool type)
    {
        if (type == true) // cộng năng lượng
        {
            ObjUse.instance.playerImpact.energy += value;
            if (ObjUse.instance.playerImpact.energy > 100f) ObjUse.instance.playerImpact.energy = 100f;
        }
        else // giảm năng lượng
        {
            ObjUse.instance.playerImpact.energy -= value;
            if (ObjUse.instance.playerImpact.energy < 0f) ObjUse.instance.playerImpact.energy = 0f;
        }
        setEnergy();
    }
    void resetHealingEnergy()
    {
        isHealingEnergy = false;
    }
    public void valueGold(int gold)
    {
        GameObject _value = PhotonNetwork.Instantiate(this.ValueDamage, new Vector3(Random.Range(transform.position.x - 0.4f, transform.position.x + 0.4f), Random.Range(transform.position.y - 0.1f, transform.position.y + 0.2f), 100f), Quaternion.identity);
        _value.GetComponent<ValueDamage>().value.color = new Color(1f, 1f, 0f, 1f);
        _value.GetComponent<ValueDamage>().value.text = "+ " + ((int)gold).ToString();
    }
    public float getDamageSkill1()
    {
        return damageSkill1;
    }
    public float getDamageSkill2()
    {
        return damageSkill2;
    }
    public float getDamageSkill3()
    {
        return damageSkill3;
    }
    public float getDamageSkill4()
    {
        return damageSkill4;
    }
}
