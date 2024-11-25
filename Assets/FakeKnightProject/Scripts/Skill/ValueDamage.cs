using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueDamage : MonoBehaviour
{
    [SerializeField] public TMP_Text value;
    bool _reset = false;
    void Start()
    {
        Invoke("_destroy", 2f);
    }
    void Update()
    {
        if (!_reset)
        {
            _reset = true;
            value.color -= new Color(0f, 0f, 0f, 0.15f);
            Invoke("resetChange", 0.3f);

        }
    }
    void resetChange()
    {
        _reset = false;
    }
    void _destroy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
