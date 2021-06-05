﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField]
    private int damage;  
    public int Damage
    {
        set
        {
            damage = value;
            if(damage < 0)
            {
                damage = 0;
            #if UNITY_EDITOR
                Debug.Log("Damage is negative");    
            #endif
            }
        }
        get
        {
            return damage;
        }
    }
    public float bullet_movespeed;
    Vector3 origin_position;
    public Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        origin_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Culling bullet
        if(transform.position.y >= GameHelper.GetCurrentScreenBounds().y)
        {
            Reset();
        }
        else if(transform.position.y <= -GameHelper.GetCurrentScreenBounds().y)
        {
            Reset();
        }
        else if(transform.position.x >= GameHelper.GetCurrentScreenBounds().x)
        {
            Reset();
        }
        else if(transform.position.x <= -GameHelper.GetCurrentScreenBounds().x)
        {
            Reset();
        }
    }
    public void Shoot()
    {
        //Hàm này dùng cho ship
        rigidbody2D.velocity = new Vector2(0f, bullet_movespeed);
    }
    public void Shoot(Vector3 direction)
    {
        //Hàm này dùng cho monster nên sẽ bắn thẳng từ trên xuống
        var vec = new Vector2(0f, -bullet_movespeed);       
        rigidbody2D.velocity = Quaternion.Euler(direction) * vec;
    }
    public void Reset()
    {
        
        rigidbody2D.velocity = Vector3.zero;
        this.transform.position = origin_position;
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
}
