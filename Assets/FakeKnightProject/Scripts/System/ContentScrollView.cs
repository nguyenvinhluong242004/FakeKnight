using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform item1, item2, item3;
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
                item1.localScale = new Vector2(0.75f + 0.25f * (150f + po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (150f + po.anchoredPosition.x) / 150f);
                item2.localScale = new Vector2(0.75f + 0.25f * (-150f - po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (-150f - po.anchoredPosition.x) / 150f);
                box.offset = new Vector2(450f + (-300f - po.anchoredPosition.x), box.offset.y);
            }    
            else //if (po.anchoredPosition.x <= -274f)
            {
                item2.localScale = new Vector2(0.75f + 0.25f * (450f + po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (450f + po.anchoredPosition.x) / 150f);
                item3.localScale = new Vector2(0.75f + 0.25f * (-450f - po.anchoredPosition.x) / 150f, 0.75f + 0.25f * (-450f - po.anchoredPosition.x) / 150f); 
                box.offset = new Vector2(450f + (-300f - po.anchoredPosition.x), box.offset.y);
            }    
        }
        else
        {
            if (po.anchoredPosition.x >= -150f)
            {
                setItem1();
            }
            else if (po.anchoredPosition.x <= -450f)
            {
                setItem3();
            }
            else
            {
                setItem2();
            }
        }
    }
    void setItem1()
    {
        item = 1;
        po.anchoredPosition = new Vector2(0f, po.anchoredPosition.y);
        item1.localScale = new Vector2(1f, 1f);
        item2.localScale = new Vector2(0.5f, 0.5f);
        item3.localScale = new Vector2(0.5f, 0.5f);
        box.offset = new Vector2(150f, box.offset.y);
    }
    void setItem2()
    {
        item = 2;
        po.anchoredPosition = new Vector2(-300f, po.anchoredPosition.y);
        item2.localScale = new Vector2(1f, 1f);
        item1.localScale = new Vector2(0.5f, 0.5f);
        item3.localScale = new Vector2(0.5f, 0.5f);
        box.offset = new Vector2(450f, box.offset.y);
    }
    void setItem3()
    {
        item = 3;
        po.anchoredPosition = new Vector2(-600f, po.anchoredPosition.y);
        item3.localScale = new Vector2(1f, 1f);
        item2.localScale = new Vector2(0.5f, 0.5f);
        item1.localScale = new Vector2(0.5f, 0.5f);
        box.offset = new Vector2(750f, box.offset.y);
    }
    public void getItem(string direction)
    {
        if (direction=="right")
        {
            if (item==1)
            {
                setItem2();
            }   
            else if (item == 2)
            {
                setItem3();
            }
        }   
        else
        {
            if (item == 2)
            {
                setItem1();
            }
            else if (item == 3)
            {
                setItem2();
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
