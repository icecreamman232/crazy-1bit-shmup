using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster01Controller : BaseMonster
{
    
    // Start is called before the first frame update
    void Start()
    {
        InitMonster();
        StartCoroutine(CheckDie());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Run();
        }
    }
    public override void Run()
    {
        Vector3 target = new Vector3(transform.position.x, GameHelper.BotLimitation, 0f);
        StartCoroutine(OnRunning(target));
        
    }
    IEnumerator OnRunning(Vector3 target)
    {
        while(transform.position.y > GameHelper.BotLimitation - 1.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position,
            Vector3.Lerp(transform.position, target, t_lerp), base_movespeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, target, base_movespeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Stop Running");
    }
}
