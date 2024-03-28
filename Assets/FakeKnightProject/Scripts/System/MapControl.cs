using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControl : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    void Update()
    {
        if (ObjectManager.instance.map.activeSelf)
        {
            Debug.Log(rect.anchoredPosition.x);
            Debug.Log(rect.anchoredPosition.y);
            if (rect.anchoredPosition.x < -500)
                rect.anchoredPosition = new Vector2(-500, rect.anchoredPosition.y);
            else if (rect.anchoredPosition.x > 500)
                rect.anchoredPosition = new Vector2(500, rect.anchoredPosition.y);
            if (rect.anchoredPosition.y < -300)
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -300);
            else if (rect.anchoredPosition.y > 140)
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 140);
        }    
    }
    public void setPositionMap(int x, int y)
    {
        rect.anchoredPosition = new Vector2(x, y);
    }
}
