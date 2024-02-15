using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private Animator anm;
    [SerializeField] public bool isOn, isAwake;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float time;
    [SerializeField] private BoxCollider2D box;
    bool isEff;

    // Update is called once per frame
    void Update()
    {
        if (!isAwake)
        {
            Invoke("setFire", time);
            isAwake = true;
            isEff = false;
        }    
        if (isOn)
        {

        }
        rb.velocity = 5 * velocity;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEff && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("chammmmm");
            collision.gameObject.GetComponent<EnemyLevel1>().setBloodEff();
            isEff = true;
        }
    }
    void setFire()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        box.enabled = true;
        anm.Play("ff");

        rb.velocity = 3 * velocity;
        velocity = new Vector3(0, 0, 0);
        Invoke("_des", 0.1f);
    }
    void _des()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }    
}
