using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform[] items;
    [SerializeField] private RectTransform po;
    [SerializeField] private BoxCollider2D box;
    bool isClick;
    public int item;
    // Start is called before the first frame update
    void Start()
    {
        isClick = false;
        item = 2;
    }
    // Update is called once per frame
    void Update()
    {
        if(isClick)
        {
            if (po.anchoredPosition.x >= -300f)
            {
                items[0].localScale = new Vector2(0.75f + 0.25f * (150f + po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (150f + po.anchoredPosition.x) / 150f);
                items[1].localScale = new Vector2(0.75f + 0.25f * (-150f - po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (-150f - po.anchoredPosition.x) / 150f);
                box.offset = new Vector2(450f + (-300f - po.anchoredPosition.x), box.offset.y);
            }
            else if (po.anchoredPosition.x >= -600f)
            {
                items[1].localScale = new Vector2(0.75f + 0.25f * (450f + po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (450f + po.anchoredPosition.x) / 150f);
                items[2].localScale = new Vector2(0.75f + 0.25f * (-450f - po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (-450f - po.anchoredPosition.x) / 150f);
                box.offset = new Vector2(450f + (-300f - po.anchoredPosition.x), box.offset.y);
            }
            else if (po.anchoredPosition.x >= -900f)
            {
                items[2].localScale = new Vector2(0.75f + 0.25f * (750f + po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (750f + po.anchoredPosition.x) / 150f);
                items[3].localScale = new Vector2(0.75f + 0.25f * (-750f - po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (-750f - po.anchoredPosition.x) / 150f);
                box.offset = new Vector2(750f + (-300f - po.anchoredPosition.x), box.offset.y);
            }
            else
            {
                items[3].localScale = new Vector2(0.75f + 0.25f * (1050f + po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (1050f + po.anchoredPosition.x) / 150f);
                items[4].localScale = new Vector2(0.75f + 0.25f * (-1050f - po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (-1050f - po.anchoredPosition.x) / 150f);
                box.offset = new Vector2(1050f + (-300f - po.anchoredPosition.x), box.offset.y);
            }
        }
        else
        {
            if (po.anchoredPosition.x >= -150f)
            {
                item = 0;
                setItem(item);
            }
            else if (po.anchoredPosition.x <= -1050f)
            {
                item = 4;
                setItem(item);
            }
            else
            {
                item = Mathf.RoundToInt(Mathf.Abs(po.anchoredPosition.x) / 300.0f);
                setItem(item);
            }
        }
    }
    void setItem(int idx)
    {
        Debug.Log(idx);
        po.anchoredPosition = new Vector2(-300 * idx, po.anchoredPosition.y);
        items[idx].localScale = new Vector2(1f, 1f);
        for (int i = 0; i < items.Length; i++)
            if (i != idx)
                items[i].localScale = new Vector2(0.5f, 0.5f);
        box.offset = new Vector2(150 + 300 * idx, box.offset.y);
    }
    public void getItem(string direction)
    {
        if (direction=="right")
        {
            if (item<4)
            {
                item++;
                setItem(item);
            }    
        }   
        else
        {
            if (item > 0)
            {
                item--;
                setItem(item);
            }
        }    
    }    
    private void OnMouseDown()
    {
        Debug.Log("cham");
        isClick = true;
    }
    private void OnMouseUp()
    {
        Debug.Log("tha");
        isClick = false;
    }
}
