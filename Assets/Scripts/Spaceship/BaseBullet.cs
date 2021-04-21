﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public int damage;
    public float bullet_movespeed;
    Vector3 origin_position;
    public Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        origin_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y >= GameHelper.TopLimitation)
        {
            Reset();
        }
    }
    public void Shoot()
    {
        rigidbody2D.velocity = new Vector2(0f, bullet_movespeed);
    }
    public void Reset()
    {
        rigidbody2D.velocity = Vector3.zero;
        this.transform.position = origin_position;
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
}