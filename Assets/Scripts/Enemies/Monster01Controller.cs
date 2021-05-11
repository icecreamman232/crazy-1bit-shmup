﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster01Controller : MonsterWithSplineMove
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            Vector3 target = new Vector3(transform.position.x, -GameHelper.get_current_screenbound().y-2.0f, 0f);
            transform.position = Vector3.MoveTowards(transform.position,
            Vector3.Lerp(transform.position, target, t_lerp), current_movespeed * Time.deltaTime);
            if(transform.position.y <= -GameHelper.get_current_screenbound().y-0.5f)
            {

                isRunning = false;
                this.StopCoroutine(CheckDie());             
                Lean.Pool.LeanPool.Despawn(this.gameObject);
            }
        }
    }

}
