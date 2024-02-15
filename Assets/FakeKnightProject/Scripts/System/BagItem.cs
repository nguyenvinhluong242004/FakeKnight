using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagItem : MonoBehaviour
{
    [SerializeField] public int shop;
    [SerializeField] public int key;
    [SerializeField] public int type;
    [SerializeField] public Image img;
    [SerializeField] public TMP_Text count;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] public BagItem bagItem;
    [SerializeField] public BagContent bagContent;
    [SerializeField] private LoadDataPlayer loadDataPlayer;
    [SerializeField] public GameObject add; // only INFOR player have
    [SerializeField] public int idx; // only INFOR player have
    public bool isUse = false;
    // Start is called before the first frame update
    void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
        if (add) // if bagItem on INFOR
        {
            int idx_ = loadDataPlayer.dataPlayer.equipments[idx];
            if (idx_ != -1)
            {
                gameObject.GetComponent<Image>().sprite = objectManager.imagesS1[idx_];
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
                    bagItem.img.sprite = objectManager.imagesS1[idx_];
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
        if (objectManager.bagItem && shop == objectManager.bagItem.shop && key == objectManager.bagItem.key)
        {
            objectManager.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
            objectManager.isSetItem = false;
            objectManager.bagItem = null;
        }
        else
        {
            if (objectManager.bagItem)
                objectManager.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
            objectManager.isSetItem = true;
            objectManager.bagItem = gameObject.GetComponent<BagItem>();
            objectManager.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 1f);
        }    
        //Debug.Log(objectManager.bagItem.img.sprite);
    }
    public void setEquiment()
    {
        if (objectManager.isSetItem)
        {
            if (objectManager.bagItem.type == 0)
            {
                Debug.Log("setEquip");
                bool check = false;
                for (int i=0; i<8; i++)
                    if (loadDataPlayer.dataPlayer.equipments[i] == objectManager.bagItem.key)
                    {
                        check = true;
                        break;
                    }  
                if (!check)
                {
                    Debug.Log("khoongg trunggf");
                    //Debug.Log(objectManager.bagItem.img.sprite);
                    gameObject.GetComponent<Image>().sprite = objectManager.bagItem.img.sprite;
                    objectManager.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
                    shop = objectManager.bagItem.shop;
                    key = objectManager.bagItem.key;
                    type = objectManager.bagItem.type;
                    img = objectManager.bagItem.img;

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
                        //    if (b.shop == 0 && b.key == loadDataPlayer.dataPlayer.equipments[idx])
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
                    int _count = int.Parse(objectManager.bagItem.count.text);
                    objectManager.bagItem.count.text = $"{_count - 1}";
                    if (_count - 1 == 0)
                    {
                        if (bagContent == null)
                            bagContent = FindObjectOfType<BagContent>();
                        bagContent.resetBagItem(objectManager.bagItem.shop, objectManager.bagItem.key);
                        add.SetActive(true);
                    }
                    bagItem = objectManager.bagItem;
                    isUse = true;
                    objectManager.isSetItem = false;
                    objectManager.loadDataPlayer.dataPlayer.equipments[idx] = key;
                    objectManager.loadDataPlayer.SaveDataGamePlayer();
                    saveEquimentForPlayer();
                    add.SetActive(false);
                }    
            }
        }
        else if (!isUse)
        {
            objectManager.bag.SetActive(true);
        }
        else if (isUse)
        {
            objectManager.removeEquip.SetActive(true);
            objectManager.bagItem = gameObject.GetComponent<BagItem>();
            objectManager.equip = gameObject;
            //Debug.Log(objectManager.bagItem.img.sprite);
        }
    }
    void saveEquimentForPlayer()
    {
        Debug.Log("saved");



    }
    public void setItem()
    {
        if (objectManager.isSetItem)
        {
            if (objectManager.bagItem.type != 0)
            {
                //Debug.Log(objectManager.bagItem.img.sprite);
                gameObject.GetComponent<Image>().sprite = objectManager.bagItem.img.sprite;
                objectManager.bagItem.GetComponent<Image>().color = new Color(1f, 1f, 0.5f, 0.5f);
                shop = objectManager.bagItem.shop;
                key = objectManager.bagItem.key;
                type = objectManager.bagItem.type;
                img = objectManager.bagItem.img;
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
                int _count = int.Parse(objectManager.bagItem.count.text);
                objectManager.bagItem.count.text = $"{_count - 1}";
                if (_count - 1 == 0)
                {
                    if (bagContent == null)
                        bagContent = FindObjectOfType<BagContent>();
                    bagContent.resetBagItem(objectManager.bagItem.shop, objectManager.bagItem.key);
                }
                bagItem = objectManager.bagItem;
                isUse = true;
                objectManager.isSetItem = false;
            }
        }
        else if (isUse && type!=0)
        {
            Debug.Log("use item!");
            gameObject.GetComponent<Image>().sprite = objectManager.imgUseItem;
            isUse = false;
            BagItem bag = gameObject.GetComponent<BagItem>();
            loadDataPlayer.setCountItem(bag.shop, bag.key, false);
            bag.shop = -1;
        }    
    }
}
