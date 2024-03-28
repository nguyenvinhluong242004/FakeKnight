using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagContent : MonoBehaviour
{
    public GameObject[] lineItem;
    public GameObject[] itemBag;
    [SerializeField] public GameObject item, line;
    [SerializeField] private GameObject uiBag; 

    // Start is called before the first frame update
    void Start()
    {
        lineItem = GameObject.FindGameObjectsWithTag("lineBag");
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, lineItem.Length * 97f);
        if (lineItem.Length > 0)
        {
            List<GameObject> childObjects = new List<GameObject>();
            foreach (GameObject lineBagObject in lineItem)
            {
                foreach (Transform child in lineBagObject.transform)
                {
                    if (child.CompareTag("bagItem"))
                    {
                        childObjects.Add(child.gameObject);
                    }
                }
            }
            itemBag = childObjects.ToArray();
        }
        // if bagconten off -> no open + buy item (in bag have) -> bug
        uiBag.SetActive(false);
    }
    public void resetBagItem(int shop, int key)
    {
        Debug.Log("reset");
        bool check = false;
        int idx = itemBag.Length - 1;
        for (int i = 0; i < itemBag.Length; i++)
        {
            if (check)
            {
                if (idx == itemBag.Length - 1)
                {
                    Destroy(itemBag[itemBag.Length - 1]);
                    break;
                }
                else
                {
                    Debug.Log(idx);
                    Debug.Log(i);
                    itemBag[idx] = itemBag[i];
                    idx++;
                }
            }
            if (itemBag[i].GetComponent<BagItem>().shop == shop)
            {
                if (itemBag[i].GetComponent<BagItem>().key == key)
                {
                    idx = i;
                    check = true;
                    Destroy(itemBag[i]);
                }
            }
        }
        System.Array.Resize(ref itemBag, itemBag.Length - 1);
        for (int i = 0; i < lineItem.Length; i++)
        {
            lineItem[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0f, lineItem[i].GetComponent<RectTransform>().sizeDelta.y);
            for (int j = i * 4; j < itemBag.Length && j < i * 4 + 4; j++)
            {
                itemBag[j].transform.SetParent(lineItem[i].transform);
                Debug.Log($"{i}");
                lineItem[i].GetComponent<RectTransform>().sizeDelta += new Vector2(95f, 0f);
            }
        }
        if (itemBag.Length % 4 == 0)
        {
            Destroy(lineItem[lineItem.Length - 1]);
            System.Array.Resize(ref lineItem, lineItem.Length - 1);
        }
    }
    public void initBagItem(int shop, int key, int count, int type, Sprite img)
    {
        int countLine = lineItem.Length;
        int countBagItem = itemBag.Length;
        int idx = countBagItem;
        for (int i = 0; i < itemBag.Length; i++)
        {
            if (itemBag[i].GetComponent<BagItem>().shop == shop)
            {
                if (itemBag[i].GetComponent<BagItem>().key == key)
                {
                    Debug.Log($"chi so: {i}");
                    idx = i;
                    break;
                }
            }
        }
        Debug.Log(idx);
        if (idx == countBagItem)
        {
            Debug.Log("creat line");
            System.Array.Resize(ref itemBag, itemBag.Length + 1);
            if (countBagItem % 4 == 0)
            {
                GameObject _line = Instantiate(line, transform.position, line.transform.rotation);
                _line.transform.SetParent(gameObject.transform);
                System.Array.Resize(ref lineItem, lineItem.Length + 1);
                lineItem[countLine] = _line;
                _line.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            GameObject _item = Instantiate(item, item.transform.position, item.transform.rotation);
            //_item.transform.SetParent(lineItem[countLine - 1].transform);
            _item.transform.SetParent(lineItem[lineItem.Length - 1].transform);
            lineItem[lineItem.Length - 1].GetComponent<RectTransform>().sizeDelta += new Vector2(95f, 0f);
            itemBag[countBagItem] = _item;
            BagItem a = _item.GetComponent<BagItem>();
            a.shop = shop;
            a.key = key;
            a.type = type;
            a.count.text = count.ToString();
            foreach (Transform child in a.transform)
            {
                if (child.name == "Image")
                {
                    child.GetComponent<Image>().sprite = img;
                }
            }
            _item.transform.localScale = new Vector3(1f, 1f, 1f);
            GameObject[] buttonBagItem = GameObject.FindGameObjectsWithTag("buttonBagItem");
            foreach (GameObject k in buttonBagItem)
            {
                BagItem bagItem = k.GetComponent<BagItem>();
                if (bagItem.shop != -1)
                {
                    BagItem __item = _item.GetComponent<BagItem>();
                    if (bagItem.shop == __item.shop && bagItem.key == __item.key)
                    {
                        bagItem.bagItem = __item;
                    }
                }
            }
        }
        else
        {
            itemBag[idx].GetComponent<BagItem>().count.text = $"{int.Parse(itemBag[idx].GetComponent<BagItem>().count.text) + 1}";
        }

    }
}
