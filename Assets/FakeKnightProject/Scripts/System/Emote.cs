using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emote : MonoBehaviour
{
    public Sprite emote;
    public GameObject image, uiShop;
    bool isOn = false;
    Image _image;
    Rigidbody2D rb;
    void Update()
    {
        if(isOn)
        {
            _image.color += new Color(_image.color.r, _image.color.g, _image.color.b, 0.01f);
            rb.velocity = new Vector2(0, 1f);
        }
    }
    public void getEmote()
    {
        image.GetComponent<Emote>()._image = image.GetComponent<Image>();
        image.GetComponent<Emote>()._image.sprite = emote;
        uiShop.SetActive(false);
        image.GetComponent<Emote>().getEmoteToImage();
    }    
    public void getEmoteToImage()
    {
        image.SetActive(true);
        rb = image.GetComponent<Rigidbody2D>();
        isOn = true;
        Invoke("resetEmote", 0.8f);
    }    
    void resetEmote()
    {
        image.SetActive(false);
        rb.velocity = new Vector2(0, 0);
        isOn = false;
        image.GetComponent<RectTransform>().position = uiShop.GetComponent<RectTransform>().position;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
    }    
}
