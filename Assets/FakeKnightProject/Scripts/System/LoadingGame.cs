using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingGame : MonoBehaviour
{
    [SerializeField] private GameObject bgrLoad, load;
    [SerializeField] private float blood, p;
    [SerializeField] private TMP_Text persent;
    public bool reset, isDone;
    // Start is called before the first frame update
    void Start()
    {
        blood = 0f;
        reset = false;
        isDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (blood<90 && !reset && !isDone)
        {
            setLoadingGame(false);
            reset = true;
            Invoke("resetLoad", Time.deltaTime);
        }    
    }
    public void setLoadingGame(bool type)
    {
        if (type)
        {
            blood = 100;
            Invoke("loadSucess", 0.7f);
        }    
        else
            blood += 2;
        float k = blood / 100f;
        persent.text = blood.ToString("F0") + " %";
        load.transform.localScale = new Vector3(k, load.transform.localScale.y, load.transform.localScale.z);
        load.transform.position = new Vector3(bgrLoad.transform.position.x - p * (1 - k), load.transform.position.y, load.transform.position.z);

    }    
    void loadSucess()
    {
        gameObject.SetActive(false);
        blood = 0f;
    }  
    void resetLoad()
    {
        reset = false;
    }    
}