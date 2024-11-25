using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D box;
    [SerializeField] public PlayerMove player;
    public void offBoxEffect()
    {
        box.enabled = false;
    }
}
