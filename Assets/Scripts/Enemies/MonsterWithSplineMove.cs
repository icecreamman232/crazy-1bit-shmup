using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;


public class MonsterWithSplineMove : BaseMonster
{
    public splineMove splineMove;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Run()
    {
        base.Run(); 
        splineMove.StartMove();
    }
}
