using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagItem : MonoBehaviour
{
    [SerializeField] public ItemManager itemManager;
    [SerializeField] public int shop;
    [SerializeField] public int key;
    [SerializeField] public int type;
    [SerializeField] public Image img;
    [SerializeField] public TMP_Text count;
    [SerializeField] public BagItem bagItem;
    [SerializeField] public BagContent bagContent;
    [SerializeField] public GameObject add; // only INFOR player have
    [SerializeField] public int idx; // only INFOR player have
    public bool isUse = false;
    // Start is called before the first frame update
    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        if (add) // if bagItem on INFOR
        {
            int idx_ = LoadDataPlayer.instance.dataPlayer.equipments[idx];
            if (idx_ != -1)
            {
                gameObject.GetComponent<Image>().sprite = ObjectManager.instance.imagesS1[idx_];
                shop = 0;
                key = idx_;
                type = 0;
                foreach (GameObject a in bagContent.itemBag)
                {
                    BagItem b = a.GetComponent<BagItem>();
                    if (b.shop == 0 && b.key == idx_)
                    {
                        bagItem = b;
                        break;
                    }    
                }    
                if (bagItem==null)
                {
                    GameObject k = Instantiate(bagContent.item, bagContent.transform.position, bagContent.transform.rotation);
                    bagItem = k.GetComponent<BagItem>();
                    bagItem.shop = shop;
                    bagItem.key = key;
                    bagItem.type = type;
                    bagItem.count.text = "0";
                    bagItem.img.sprite = ObjectManager.instance.imagesS1[idx_];
                    Destroy(k);
                }
                img = bagItem.img;
                isUse = true;
                add.SetActive(false);
            }    
        }
        else
            bagContent = FindObjectOfType<BagContent>();
    }
    public void chooseItem()
    {
        if (ObjectManager.instance.bagItem && shop == ObjectManager.instance.bagItem.shop && key == ObjectManager.instance.bagItem.key)
        {
            ObjectManager.instance.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
            ObjectManager.instance.isSetItem = false;
            ObjectManager.instance.bagItem = null;
        }
        else
        {
            if (ObjectManager.instance.bagItem)
                ObjectManager.instance.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
            ObjectManager.instance.isSetItem = true;
            ObjectManager.instance.bagItem = gameObject.GetComponent<BagItem>();
            ObjectManager.instance.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 1f);
        }    
        //Debug.Log(ObjectManager.instance.bagItem.img.sprite);
    }
    public void setEquiment()
    {
        if (ObjectManager.instance.isSetItem)
        {
            if (ObjectManager.instance.bagItem.type == 0)
            {
                Debug.Log("setEquip");
                bool check = false;
                // chưa xuwwr lí check trùng giáp?
                for (int i=0; i<8; i++)
                    if (LoadDataPlayer.instance.dataPlayer.equipments[i] == ObjectManager.instance.bagItem.key)
                    {
                        check = true;
                        break;
                    }  
                if (!check)
                {
                    Debug.Log("khoongg trunggf");
                    //Debug.Log(ObjectManager.instance.bagItem.img.sprite);
                    gameObject.GetComponent<Image>().sprite = ObjectManager.instance.bagItem.img.sprite;
                    ObjectManager.instance.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
                    shop = ObjectManager.instance.bagItem.shop;
                    key = ObjectManager.instance.bagItem.key;
                    type = ObjectManager.instance.bagItem.type;
                    img = ObjectManager.instance.bagItem.img;

                    float _damage = InformationItem.instance.data[ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key].damage;
                    float _resist = InformationItem.instance.data[ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key].resist;
                    float _speed = InformationItem.instance.data[ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key].speed;
                    ObjUse.instance.changePercent(_damage, _resist, _speed, "add");

                    if (isUse)
                    {
                        // nếu 2 equipment giống nhau nằm trong infor
                        // khi đưa 1 vào bag thì thằng còn lại có bagitem=null,
                        // nên khi 1 item khác thay thế thằng còn lại thì trong bag không tăng item (trước đó) đó lên
                        // mặc dù dữ liệu không thay đổi

                        // Nhưng chỉ cho phép 1 equipment nằm trong infor, không trùng nên chỗ này không cần
                        // nếu cho phép nhiều thằng trùng nhau được thì phải có hàm này, không thì bỏ đi đỡ tốn chi phí
                        //foreach (GameObject a in bagContent.itemBag) 
                        //{
                        //    BagItem b = a.GetComponent<BagItem>();
                        //    if (b.shop == 0 && b.key == LoadDataPlayer.instance.dataPlayer.equipments[idx])
                        //    {
                        //        bagItem = b;
                        //        break;
                        //    }
                        //}
                        ////////////////////////////////////////////
                        Debug.Log("isuse");
                        Debug.Log(bagItem);

                        bagItem.count.text = $"{int.Parse(bagItem.count.text) + 1}";
                        if (int.Parse(bagItem.count.text) == 1)
                        {
                            Debug.Log("init");
                            if (bagContent == null)
                                bagContent = FindObjectOfType<BagContent>();
                            bagContent.initBagItem(bagItem.shop, bagItem.key, 1, type, bagItem.img.sprite);
                        }
                    }
                    int _count = int.Parse(ObjectManager.instance.bagItem.count.text);
                    ObjectManager.instance.bagItem.count.text = $"{_count - 1}";
                    if (_count - 1 == 0)
                    {
                        if (bagContent == null)
                            bagContent = FindObjectOfType<BagContent>();
                        bagContent.resetBagItem(ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key);
                        add.SetActive(true);
                    }
                    bagItem = ObjectManager.instance.bagItem;
                    isUse = true;
                    ObjectManager.instance.isSetItem = false;
                    ObjectManager.instance.bagItem = null;
                    LoadDataPlayer.instance.dataPlayer.equipments[idx] = key;
                    LoadDataPlayer.instance.SaveDataGamePlayer();
                    saveEquimentForPlayer();
                    add.SetActive(false);
                }    
            }
        }
        else if (!isUse)
        {
            ObjectManager.instance.bag.SetActive(true);
        }
        else if (isUse)
        {
            ObjectManager.instance.removeEquip.SetActive(true);
            ObjectManager.instance.bagItem = gameObject.GetComponent<BagItem>();
            ObjectManager.instance.equip = gameObject;
            //Debug.Log(ObjectManager.instance.bagItem.img.sprite);
        }
    }
    void saveEquimentForPlayer()
    {
        Debug.Log("saved");



    }
    public void setItem()
    {
        if (ObjectManager.instance.isSetItem)
        {
            if (ObjectManager.instance.bagItem.type != 0)
            {
                //Debug.Log(ObjectManager.instance.bagItem.img.sprite);
                gameObject.GetComponent<Image>().sprite = ObjectManager.instance.bagItem.img.sprite;
                ObjectManager.instance.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
                shop = ObjectManager.instance.bagItem.shop;
                key = ObjectManager.instance.bagItem.key;
                type = ObjectManager.instance.bagItem.type;
                img = ObjectManager.instance.bagItem.img;
                if (isUse)
                {
                    Debug.Log("isuse");
                    bagItem.count.text = $"{int.Parse(bagItem.count.text) + 1}";
                    if (int.Parse(bagItem.count.text) == 1)
                    {
                        Debug.Log("init");
                        if (bagContent == null)
                            bagContent = FindObjectOfType<BagContent>();
                        bagContent.initBagItem(bagItem.shop, bagItem.key, 1, type, bagItem.img.sprite);
                    }
                }
                int _count = int.Parse(ObjectManager.instance.bagItem.count.text);
                ObjectManager.instance.bagItem.count.text = $"{_count - 1}";
                if (_count - 1 == 0)
                {
                    if (bagContent == null)
                        bagContent = FindObjectOfType<BagContent>();
                    bagContent.resetBagItem(ObjectManager.instance.bagItem.shop, ObjectManager.instance.bagItem.key);
                }
                bagItem = ObjectManager.instance.bagItem;
                isUse = true;
                ObjectManager.instance.isSetItem = false;
                ObjectManager.instance.bagItem = null;
            }
        }
        else if (isUse && type!=0)
        {
            Debug.Log("use item!");
            BagItem bag = gameObject.GetComponent<BagItem>();
            if (itemManager.UseItem(bag.shop, bag.key)) // use
            {
                gameObject.GetComponent<Image>().sprite = ObjectManager.instance.imgUseItem;
                isUse = false;
                LoadDataPlayer.instance.setCountItem(bag.shop, bag.key, false);
                bag.shop = -1;
            }
        }    
    }
}
