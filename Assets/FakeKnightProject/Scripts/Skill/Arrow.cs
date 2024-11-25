using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float x, y;
    public bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        //isOn = true;
        //x = 1f;
        //y = 0;
        //transform.eulerAngles = new Vector3(0f, 0f, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(x, y);
        if(isOn)
        {
            Invoke("_destroy", 0.4f);
            isOn = false;
        }    
    }
    void _destroy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
