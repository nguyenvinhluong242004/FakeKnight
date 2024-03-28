using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data
{
    public string name;
    //public Image image;
    public int shop;
    public int key;
    public int price, type;
    public string properties;
    public string infor;
    public float damage;
    public float resist;
    public float healing;

    public Data(string name, int shop, int key, int price, int type, string properties, string infor, float damage, float resist, float healing)
    {
        this.name = name;
        this.shop = shop;
        this.key = key;
        this.price = price;
        this.type = type;
        this.properties = properties;
        this.infor = infor;
        this.damage = damage;
        this.resist = resist;
        this.healing = healing;
    }
}
public class InformationItem : MonoBehaviour
{
    [SerializeField] ObjectManager objectManager;
    public Data[,] data = new Data[4, 17];
    void Start()
    {
        // update data json late
        for(int i=0; i<objectManager.imagesS1.Length; i++)
            data[0, i] = new Data("Item item", 0, i, 7000, 0, "Advanced", "Supplementary items", 7f, 7f, 7f);
        for (int i = 0; i < objectManager.imagesS2.Length; i++)
            data[1, i] = new Data("Item item", 1, i, 150, 1, "Advanced", "Supplementary items", 7f, 7f, 7f);
        for (int i = 0; i < objectManager.imagesS3.Length; i++)
            data[2, i] = new Data("Item item", 2, i, 150, 1, "Advanced", "Supplementary items", 7f, 7f, 7f);
        for (int i = 0; i < objectManager.imagesS4.Length; i++)
            data[3, i] = new Data("Item item", 3, i, 150, 1, "Advanced", "Supplementary items", 7f, 7f, 7f);
    }
    public void getItem(int shop, int key, ref string name, ref int price, ref int type, ref string properties, ref string infor, ref float damage, ref float resist, ref float healing)
    {
        name = data[shop, key].name;
        price = data[shop, key].price;
        type = data[shop, key].type;
        properties = data[shop, key].properties;
        infor = data[shop, key].infor;
        damage = data[shop, key].damage;
        resist = data[shop, key].resist;
        healing = data[shop, key].healing;
    }    
}
