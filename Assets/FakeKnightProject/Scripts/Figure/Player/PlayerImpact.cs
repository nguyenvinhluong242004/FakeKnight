using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerImpact : MonoBehaviour
{
    [SerializeField] public float blood, energy;
    [SerializeField] public Blood _blood, _energy;
    [SerializeField] public TMP_Text bl, ene;
    // Start is called before the first frame update
    void Start()
    {
        blood = 100f;
        energy = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _blood.setBlood(blood);
            _energy.setBlood(energy);
        }
    }
    public void setBlood(float k)
    {
        blood -= k;
        _blood.setBlood(blood);
        bl.text = $"{blood} / 100";
    }
    public void setEnergy(float k)
    {
        energy -= k;
        _energy.setBlood(energy);
        ene.text = $"{blood} / 100";
    }
}
