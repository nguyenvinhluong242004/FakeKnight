using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guards : MonoBehaviour
{
    [SerializeField] private Animator anm;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private bool isOn, isStart, isL, isSkill;
    public GameObject enemy;
    public GameObject player;
    Vector3 po;
    void Start()
    {
        po = transform.position;
    }
    void Update()
    {
        if (!isSkill)
        {
            if (isOn)
            {
                move(true);
            }
            else if (!isStart)
            {
                move(false);
                if (Mathf.Abs(transform.position.x - po.x) < 0.3f && Mathf.Abs(transform.position.y - po.y) < 0.3f)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    isStart = true;
                    transform.position = po;
                    spr.flipX = isL;
                    anm.Play("idle");
                }
            }
        } 
        if (player)
        {
            if (player.transform.position.y > transform.position.y)
                spr.sortingOrder = 2;
            else
                spr.sortingOrder = 0;
        }    
    }
    void move(bool k)
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = po;
        if (k)
            targetPos = enemy.transform.position;
        // Tính toán vectơ hướng từ vị trí hiện tại đến vị trí của quái vật
        Vector2 direction = targetPos - currentPos;
        if (direction.magnitude > 3f)
        {
            anm.Play("idle");
        }
        else
        {
            // Giới hạn độ dài của vectơ hướng thành 1
            direction = Vector2.ClampMagnitude(direction, 1f);
            if (direction.x < 0)
                spr.flipX = true;
            else
                spr.flipX = false;
            // Di chuyển lính canh theo vectơ hướng với tốc độ đã đặt
            transform.position = Vector2.MoveTowards(currentPos, currentPos + direction, 1f * Time.deltaTime);
            anm.Play("run");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("hello");
            player = collision.gameObject;
        }   
        else if (!isOn && collision.CompareTag("Enemy"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Debug.Log("enemy");
            isOn = true;
            isStart = false;
            enemy = collision.gameObject;
        }    
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(isOn && collision.gameObject.CompareTag("Enemy"))
        {
            isSkill = true;
            anm.Play("skill");
            if (enemy)
                enemy.GetComponent<EnemyLevel1>().blood = -0.1f;
            Invoke("reset", 0.5f);
        }    
    }
    private void reset()
    {
        enemy = null;
        isOn = false;
        isSkill = false;
    }
}
