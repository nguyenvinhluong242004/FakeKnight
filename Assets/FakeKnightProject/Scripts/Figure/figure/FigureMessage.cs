using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FigureMessage : MonoBehaviour
{
    [SerializeField] private string nameFigure;
    public TMP_Text message;
    ObjectManager objectManager;
    string[] messOldman =
    {
        "Go explore new lands. Kill monsters to level up, complete missions to quickly get stronger!",
        "Taking advantage of available resources, conquering higher things, that's what a true warrior needs to do!"
    };
    bool isStart;
    int idx;
    // Start is called before the first frame update
    void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isStart)
        {
            Debug.Log("click");
            if (nameFigure == "OldMan")
            {
                if (idx < messOldman.Length)
                {
                    message.text = messOldman[idx];
                    idx++;
                }
                else
                {
                    objectManager.message.SetActive(false);
                }

            }
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("clickkkkkk");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (nameFigure == "OldMan")
            {
                isStart = true;
                message.text = messOldman[0];
                objectManager.message.SetActive(true);
                idx = 1;
            }
        }
    }
}
